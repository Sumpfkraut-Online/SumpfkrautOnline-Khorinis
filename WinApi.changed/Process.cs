﻿using System;
using System.Collections.Generic;
using System.Text;
using WinApi.Kernel;
using System.Runtime.InteropServices;
using System.IO;

namespace WinApi
{
    public enum FlagRegisters
    {
        CF = 0,
        PF = 2,
        AF = 4,
        ZF = 6,
        SF = 7,
        OF = 11,
    }

    public enum Registers // PUSHAD ORDER !!!
    {
        EDI,
        ESI,
        EBP,
        ESP,
        EBX,
        EDX,
        ECX,
        EAX
    }

    public static unsafe class Process
    {
        #region Initialization

        static IntPtr handle;
        internal static IntPtr Handle { get { return handle; } }

        static uint procID;
        internal static uint ID { get { return procID; } }

        static Process()
        {
            try
            {
                procID = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
                System.Diagnostics.Process.EnterDebugMode();
                handle = ProcessImports.OpenProcess((uint)ProcessImports.ProcessAccess.PROCESS_ALL_ACCESS, false, procID);
                if (handle == IntPtr.Zero)
                    Win32Exception.ThrowLastError();

                Guid CLSID_CLRRuntimeHost = new Guid(0x90F1A06E, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);
                Guid IID_ICLRRuntimeHost = new Guid(0x90F1A06C, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);

                runtimeInterface = RuntimeEnvironment.GetRuntimeInterfaceAsIntPtr(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost).ToInt32();
                if (runtimeInterface == 0)
                    throw new Exception("Runtime interface not found!");

                IntPtr user32Handle = ProcessImports.GetModuleHandle("user32.dll");
                if (user32Handle == IntPtr.Zero)
                    Win32Exception.ThrowLastError("GetModuleHandle user32.dll");

                IntPtr wptr = ProcessImports.GetProcAddress(user32Handle, "wsprintfW");
                if (wptr == IntPtr.Zero)
                    Win32Exception.ThrowLastError("GetProcAddress wsprintfW");

                wsprintfWPtr = wptr.ToInt32();
                wsprintfWTypeArg = AllocString("%p");
                hookArg_DLLPath = AllocString(typeof(Process).Assembly.Location);
                hookArg_Namespace = AllocString(typeof(Process).FullName);
                hookArg_MethodName = AllocString(typeof(Process).GetMethod("ApiHook").Name);
                hookArg_Result = Alloc(4).ToInt32();
            }
            catch (Exception e)
            {
                MessageBox(IntPtr.Zero, e.ToString(), "WinApi Exception!", 0);
            }
        }

        static int wsprintfWPtr;
        static int wsprintfWTypeArg;
        static readonly int hookArg_DLLPath;
        static readonly int hookArg_Namespace;
        static readonly int hookArg_MethodName;
        static readonly int hookArg_Result;


        #endregion

        #region Utils

        static int AllocString(string str, Encoding enc = null)
        {
            byte[] bytes = (enc == null ? Encoding.Unicode : enc).GetBytes(str + "\0");
            int ptr = Alloc((uint)bytes.Length).ToInt32();
            Write(ptr, bytes);
            return ptr;
        }

        static void FreeString(int address, string str, Encoding enc = null)
        {
            Free(new IntPtr(address), (uint)(enc == null ? Encoding.Unicode : enc).GetByteCount(str + "\0"));
        }

        #endregion

        #region Memory Methods

        public static bool VirtualProtect(int address, uint size)
        {
            return ProcessImports.VirtualProtect(new IntPtr(address), size, ProcessImports.MemoryProtection.ExecuteReadWrite, out ProcessImports.MemoryProtection k);
        }

        public static IntPtr Alloc(uint size)
        {
            if (size == 0)
                return IntPtr.Zero;
            IntPtr ptr = ProcessImports.VirtualAllocEx(handle, IntPtr.Zero, size, ProcessImports.AllocationType.Reserve | ProcessImports.AllocationType.Commit, ProcessImports.MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                Win32Exception.ThrowLastError();

            return ptr;
        }

        public static bool Free(IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero || size == 0)
                return false;
            if (!ProcessImports.VirtualFreeEx(handle, ptr, size, ProcessImports.AllocationType.Decommit))
                Win32Exception.ThrowLastError();
            if (!ProcessImports.VirtualFreeEx(handle, ptr, 0, ProcessImports.AllocationType.Release))
                Win32Exception.ThrowLastError();

            return true;
        }

        const int PointerBorder = int.MaxValue; //0xB61000;
        const int ProcessBorder = 0x401000;

        #region Write

        public static void Write(int address, bool value)
        {
            Write(address, value ? 1 : 0);
        }

        public static void Write(int address, int value)
        {
            if (address < PointerBorder)
            {
                if (address < ProcessBorder)
                    throw new ArgumentOutOfRangeException("Address: " + address.ToString("X4"));

                IntPtr ptr = new IntPtr(value);
                if (!ProcessImports.WriteProcessMemory(handle, new IntPtr(address), out ptr, 4, out IntPtr byteWritten))
                    Win32Exception.ThrowLastError();
            }
            else
            {
                *(int*)address = value;
            }
        }

        public static void Write(int address, byte value)
        {
            if (address < PointerBorder)
            {
                Write(address, new byte[1] { value });
            }
            else
            {
                *(byte*)address = value;
            }
        }

        public static void Write(int address, float value)
        {
            if (address < PointerBorder)
            {
                Write(address, BitConverter.GetBytes(value));
            }
            else
            {
                *(float*)address = value;
            }
        }

        public static void Write(int address, short value)
        {
            if (address < PointerBorder)
            {
                Write(address, BitConverter.GetBytes(value));
            }
            else
            {
                *(short*)address = value;
            }
        }

        public static void Write(int address, ushort value)
        {
            if (address < PointerBorder)
            {
                Write(address, BitConverter.GetBytes(value));
            }
            else
            {
                *(ushort*)address = value;
            }
        }

        public static void Write(int address, params byte[] bytes)
        {
            Write(address, bytes, bytes.Length);
        }

        public static void Write(int address, byte[] arr, int count)
        {
            if (arr.Length < count)
                throw new ArgumentException("Length < Count");

            if (address < PointerBorder)
            {
                if (address < ProcessBorder)
                    throw new ArgumentOutOfRangeException("Address: " + address.ToString("X4"));

                if (!ProcessImports.WriteProcessMemory(handle, new IntPtr(address), arr, (uint)count, out uint byteWritten))
                    Win32Exception.ThrowLastError();
            }
            else
            {
                // FIXME: use memcpy or ints
                for (int i = 0; i < count; i++)
                    *(byte*)(address + i) = arr[i];
            }
        }

        public static void Nop(int address, int count)
        {
            if (address < PointerBorder)
            {
                byte[] buf = new byte[count];
                for (int i = 0; i < count; i++)
                    buf[i] = 0x90;

                Write(address, buf);
            }
            else
            {
                for (int i = 0; i < count; i++)
                    *(byte*)(address + i) = 0x90;
            }
        }

        #endregion

        #region Read

        public static bool ReadBool(int address)
        {
            return ReadInt(address) != 0;
        }

        public static int ReadInt(int address)
        {
            if (address < ProcessBorder)
                throw new ArgumentException("Address: " + address.ToString("X4"));

            /*if (!ProcessImports.ReadProcessMemory(handle, new IntPtr(address), out IntPtr puffer, new UIntPtr(4), out IntPtr rw))
                Error.ThrowLastError();
            return puffer.ToInt32();*/

            return *(int*)address;

        }

        public static byte ReadByte(int address)
        {
            return *(byte*)address;
        }

        public static float ReadFloat(int address)
        {
            return *(float*)address;
        }

        public static short ReadShort(int address)
        {
            return *(short*)address;
        }

        public static ushort ReadUShort(int address)
        {
            return *(ushort*)address;
        }

        public static byte[] ReadBytes(int address, uint count)
        {
            byte[] arr = new byte[count];
            Read(address, arr, count);
            return arr;
        }

        public static void Read(int address, byte[] buffer, uint count)
        {
            if (address < ProcessBorder)
                throw new ArgumentException("Address: " + address.ToString("X4"));

            if (buffer.Length < count)
                throw new ArgumentException("Length < Count");

            /*uint bytesRead = 0;
            if (!ProcessImports.ReadProcessMemory(handle, new IntPtr(address), buffer, count, ref bytesRead))
                Error.ThrowLastError();*/

            // FIXME: use memcpy or ints
            for (int i = 0; i < count; i++)
                buffer[i] = *(byte*)(address + i);
        }

        #endregion

        #endregion

        #region Calls

        // fixme ?
        //[UnmanagedFunctionPointer(CallingConvention.StdCall)]
        //delegate void STDCallHandler(int arg0);

        enum CALLTYPE
        {
            STDCALL,
            THISCALL,
            CDECLCALL,
            FASTCALL
        }

        public static T STDCALL<T>(int funcPtr, params CallValue[] args) where T : CallValue, new()
        {
            return MAKECALL<T>(CALLTYPE.STDCALL, 0, 0, funcPtr, args);
        }

        public static T CDECLCALL<T>(int funcPtr, params CallValue[] args) where T : CallValue, new()
        {
            return MAKECALL<T>(CALLTYPE.CDECLCALL, 0, 0, funcPtr, args);
        }

        public static T THISCALL<T>(int thisPtr, int funcPtr, params CallValue[] args) where T : CallValue, new()
        {
            if (thisPtr == 0)
                throw new Exception("Process.THISCALL: This-pointer not found!");
            return MAKECALL<T>(CALLTYPE.THISCALL, thisPtr, 0, funcPtr, args);
        }

        public static T FASTCALL<T>(int para1, int para2, int funcPtr, params CallValue[] args) where T : CallValue, new()
        {
            return MAKECALL<T>(CALLTYPE.FASTCALL, para1, para2, funcPtr, args);
        }

        delegate void call();
        private static T MAKECALL<T>(CALLTYPE type, int thisPtr, int fcallArg, int funcPtr, params CallValue[] args) where T : CallValue, new()
        {
            if (funcPtr == 0)
                throw new Exception("Process.MAKECALL: Method-pointer not found");

            List<byte> list = new List<byte>();

            list.Add(0x60);//pushad

            int argsCount = 0;

            //Argumente pushen
            if (args != null)
            {
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    CallValue arg = args[i];
                    if (arg != null)
                    {
                        List<byte[]> callParams = args[i].GetCallParams();
                        for (int i2 = callParams.Count - 1; i2 >= 0; i2--)
                        {
                            argsCount++;
                            list.Add(0x68);
                            list.AddRange(callParams[i2]);
                        }
                    }
                }
            }

            //This-Pointer in ecx schreiben
            if (type == CALLTYPE.THISCALL || type == CALLTYPE.FASTCALL)
            {
                list.Add(0xB9);
                list.AddRange(BitConverter.GetBytes(thisPtr));
            }

            //Fastcall pointer in edx schreiben
            if (type == CALLTYPE.FASTCALL)
            {
                list.Add(0xBA);
                list.AddRange(BitConverter.GetBytes(fcallArg));
            }

            T returnValue = new T();

            uint length = (uint)(list.Count + 1 + 4 + 1 + 1);
            if (returnValue.ValueLength() != 0)
                length += 1 + returnValue.ValueLength();

            if (type == CALLTYPE.CDECLCALL)
                length += 3;


            IntPtr baseadress = Alloc(length);

            IntPtr returnAddress = IntPtr.Zero;
            if (returnValue.ValueLength() != 0)
                returnAddress = Alloc(returnValue.ValueLength());

            //call
            list.Add(0xE8);
            list.AddRange(BitConverter.GetBytes(funcPtr - (baseadress.ToInt32() + list.Count) - 4)); // - Aktuelle Addresse - 4

            //Return schreiben
            if (returnValue.ValueLength() != 0)
            {
                list.Add(0xA3);
                list.AddRange(BitConverter.GetBytes((uint)returnAddress.ToInt32()));
            }

            if (type == CALLTYPE.CDECLCALL)
            {
                list.AddRange(new byte[] { 0x83, 0xC4 });
                list.Add((byte)(argsCount * 4));
            }

            list.Add(0x61);//popad
            list.Add(0xC3);//RTN

            //Write the new function
            Write(baseadress.ToInt32(), list.ToArray());

            //Call the new function
            call mc = (call)Marshal.GetDelegateForFunctionPointer(baseadress, typeof(call));
            mc();

            //Bisschen aufräumen
            Free(baseadress, length);

            if (returnValue.ValueLength() != 0)
            {
                returnValue.Initialize(returnAddress.ToInt32());//Adresse in der die return Value gespeichert wurde
                Free(returnAddress, returnValue.ValueLength());
                return returnValue;
            }
            else
            {
                return null;
            }

        }

        #endregion

        #region Hooks

        static int runtimeInterface;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

        static List<Hook> hooks = new List<Hook>();

        public static int ApiHook(string message)
        {
            try
            {
                if (!int.TryParse(message, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out int address))
                    return 0;

                int espValue = ReadInt(address + 0x12);

                int hookID = ReadInt(espValue);
                if (hookID >= 0 && hookID < hooks.Count)
                {
                    Hook hook = hooks[hookID];
                    hook.Callback(hook, new RegisterMemory(espValue + 4, hook));
                }
                else
                {
                    throw new Exception(hookID + " HookID is out of range! 0.." + hooks.Count);
                }
            }
            catch (Exception e)
            {
                MessageBox(IntPtr.Zero, e.ToString(), "ApiHook catched exception!", 0);
            }
            return 0;
        }

        /* 

         Hooks work like this:         
         1. Allocate a small memory location
         2. Write the hex address of the memory location into it (wchar)
         3. Copy registers & arguments into the memory location
         4. Call the .NET method via CLR with the memory location pointer as argument
         5. Copy registers & arguments from the memory location
         6. Free the memory location

        */

        public static Hook AddHook(HookCallback callback, int address, uint length)
        {
            if (callback == null)
                throw new ArgumentNullException("Callback is null!");
            if (address == 0)
                throw new ArgumentNullException("Address is zero!");
            if (length < 5)
                throw new ArgumentException("Length is < 5!");

            // calculate size of memory location

            const byte memoryLen = 2 * (8 + 1)  // written address (wchar)
                                     + 4;        // esp address

            uint funcLen = 92 + length;

            Hook result;
            int hookID = hooks.Count;

            int funcPtr = Alloc(funcLen).ToInt32();
            using (MemoryStream ms = new MemoryStream((int)funcLen))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                // push registers, flags and hook id
                bw.Write((byte)0x60); // pushad, EAX, ECX, EDX, EBX, EBP, ESP (original value), EBP, ESI, and EDI

                bw.Write((byte)0x66); // pushf 
                bw.Write((byte)0x9C);

                bw.Write((byte)0x68); // push hookID
                bw.Write(hookID);

                // save esp
                bw.Write((byte)0x89); // mov esi, esp
                bw.Write((byte)0xE6);

                // alloc memory location
                bw.Write((byte)0x6A); // push memoryLen
                bw.Write(memoryLen);

                bw.Write((byte)0xFF); // call [malloc] (imported)
                bw.Write((byte)0x15);
                bw.Write(0x0082E30C);

                /*bw.Write((byte)0x85); // test eax, eax
                bw.Write((byte)0xC0);

                bw.Write((byte)0x74); // je to popf
                bw.Write((byte)0x00); // FILL ME*/

                // save memory address
                bw.Write((byte)0x8B); // mov ebp, eax
                bw.Write((byte)0xE8);

                // write memory address as wchar into memory location
                bw.Write((byte)0x50); // push eax

                bw.Write((byte)0x68); // push L"%p"
                bw.Write(wsprintfWTypeArg);

                bw.Write((byte)0x50); // push eax

                bw.Write((byte)0xE8); // call wsprintfWPtr
                bw.Flush(); ms.Flush();
                bw.Write(wsprintfWPtr - (funcPtr + (int)ms.Length + 4));

                // write ESP into memory location
                bw.Write((byte)0x89); // mov [ebp + 18], esi
                bw.Write((byte)0x75);
                bw.Write((byte)0x12);

                // call .NET method
                bw.Write((byte)0x68); // push result (arg5)
                bw.Write(hookArg_Result);

                bw.Write((byte)0x55); // push ebp (memory location ptr = arg4)

                bw.Write((byte)0x68); // push result (arg3)
                bw.Write(hookArg_MethodName);

                bw.Write((byte)0x68); // push result (arg2)
                bw.Write(hookArg_Namespace);

                bw.Write((byte)0x68); // push result (arg1)
                bw.Write(hookArg_DLLPath);

                bw.Write((byte)0x68); // push this (arg0 = runtimeInterface)
                bw.Write(runtimeInterface);

                bw.Write((byte)0x8B); // mov ECX, [runtimeInterface]
                bw.Write((byte)0x0D);
                bw.Write(runtimeInterface);

                bw.Write((byte)0xFF); // call [ECX+0x2C] https://msdn.microsoft.com/de-de/library/ms164411(v=vs.110).aspx
                bw.Write((byte)0x51);
                bw.Write((byte)0x2C);

                // free memory location
                bw.Write((byte)0x55); // push ebp (memory location ptr)

                bw.Write((byte)0xFF); // call [free] (imported)
                bw.Write((byte)0x15);
                bw.Write(0x0082E308);

                bw.Write((byte)0x83); // add esp, 24 -> flag push
                bw.Write((byte)0xC4);
                bw.Write((byte)0x18);

                // pop registers & flags
                bw.Write((byte)0x66); // popf
                bw.Write((byte)0x9D);

                bw.Write((byte)0x61); // popad

                // write old code
                byte[] oldCode = ReadBytes(address, length);
                bw.Write(oldCode);

                // write jump back
                bw.Write((byte)0xE9);
                bw.Write((address + length) - (funcPtr + funcLen - 4));

                // write whole function
                Write(funcPtr, ms.ToArray());

                ms.Position = 0;
                ms.SetLength(0);

                // write hook jmp
                bw.Write((byte)0xE9);
                bw.Write(funcPtr - (address + 5));
                for (int i = 5; i < length; i++)
                    bw.Write((byte)0x90);

                Write(address, ms.ToArray());

                result = new Hook(address, callback, funcPtr, funcLen, funcPtr + (int)(funcLen - 9 - length), oldCode);
            }

            hooks.Add(result);
            return result;
        }


        #endregion

        #region FastHook

        [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 34)]
        public unsafe struct HookStruct
        {
            short flags;
            int edi;
            int esi;
            int ebp;
            int esp;
            int ebx;
            int edx;
            int ecx;
            int eax;

            public int EDI
            {
                get { return this.edi; }
                set
                {
                    *(int*)(esp - 32) = value;
                }
            }

            public int ESI
            {
                get { return this.esi; }
                set
                {
                    *(int*)(esp - 28) = value;
                }
            }

            public int EBP
            {
                get { return this.ebp; }
                set
                {
                    *(int*)(esp - 24) = value;
                }
            }

            public int ESP
            {
                get { return this.esp; }
                /*set
                {
                    *(int*)(esp - 20) = value;
                }*/
            }

            public int EBX
            {
                get { return this.ebx; }
                set
                {
                    *(int*)(esp - 16) = value;
                }
            }

            public int EDX
            {
                get { return this.edx; }
                set
                {
                    *(int*)(esp - 12) = value;
                }
            }

            public int ECX
            {
                get { return this.ecx; }
                set
                {
                    *(int*)(esp - 8) = value;
                }
            }

            public int EAX
            {
                get { return this.eax; }
                set
                {
                    *(int*)(esp - 4) = value;
                }
            }

            public int GetArgument(int index)
            {
                return *(int*)(esp + 4 * (index + 1));
            }

            public void SetArgument(int index, int value)
            {
                *(int*)(esp + 4 * (index + 1)) = value;
            }
        }

        static List<FastHookCallback> fastHooks = new List<FastHookCallback>();
        
        public delegate void FastHookCallback(HookStruct args);
        public static Hook FastHook(FastHookCallback callback, int address, uint length)
        {
            if (callback == null)
                throw new ArgumentNullException("Callback is null!");
            if (address == 0)
                throw new ArgumentNullException("Address is zero!");
            if (length < 5)
                throw new ArgumentException("Length is < 5!");

            fastHooks.Add(callback);
            byte[] oldCode = ReadBytes(address, length);

            int cavity = Alloc(19 + length).ToInt32();
            using (MemoryStream ms = new MemoryStream(19 + (int)length))
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                // JMP
                bw.Write((byte)0xE9);
                bw.Write(cavity - address - 5);
                for (int i = 5; i < length; i++)
                    bw.Write((byte)0x90);
                bw.Flush();
                Write(address, ms.ToArray());

                // HOOK
                ms.Position = 0;
                ms.SetLength(0);

                bw.Write((byte)0x60); // pushad
                bw.Write((byte)0x66); // pushf
                bw.Write((byte)0x9C);

                bw.Write((byte)0xE8);
                bw.Write(Marshal.GetFunctionPointerForDelegate(callback).ToInt32() - cavity - 8);
                bw.Write((byte)0x83); // sub, esp 34
                bw.Write((byte)0xEC);
                bw.Write((byte)36);

                bw.Write((byte)0x66); // popf
                bw.Write((byte)0x9D);
                bw.Write((byte)0x61); // popad

                bw.Write(oldCode);
                bw.Write((byte)0xE9);
                bw.Write(address - cavity - 19);
                bw.Flush();
                Write(cavity, ms.ToArray());
            }

            return new Hook(address, null, cavity, 19 + length, cavity + 14, oldCode);
        }

        #endregion
    }
}
