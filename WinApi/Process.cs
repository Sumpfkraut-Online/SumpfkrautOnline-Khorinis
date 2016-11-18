using System;
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
    
    public static class Process
    {
        #region Initialization

        static IntPtr Handle;
        static uint ProcessID;

        static Process()
        {
            try
            {
                ProcessID = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
                System.Diagnostics.Process.EnterDebugMode();
                Handle = ProcessImports.OpenProcess((uint)ProcessImports.ProcessAccess.PROCESS_ALL_ACCESS, false, ProcessID);
                if (Handle == IntPtr.Zero)
                    Error.GetLastError();

                Guid CLSID_CLRRuntimeHost = new Guid(0x90F1A06E, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);
                Guid IID_ICLRRuntimeHost = new Guid(0x90F1A06C, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);

                runtimeInterface = RuntimeEnvironment.GetRuntimeInterfaceAsIntPtr(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost).ToInt32();
                if (runtimeInterface == 0)
                    throw new Exception("Runtime interface not found!");

                IntPtr user32Handle = ProcessImports.GetModuleHandle("user32.dll");
                if (user32Handle == IntPtr.Zero)
                    Error.GetLastError("GetModuleHandle user32.dll");

                IntPtr wptr = ProcessImports.GetProcAddress(user32Handle, "wsprintfW");
                if (wptr == IntPtr.Zero)
                    Error.GetLastError("GetProcAddress wsprintfW");

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
            ProcessImports.MemoryProtection k;
            return ProcessImports.VirtualProtect(new IntPtr(address), size, ProcessImports.MemoryProtection.ExecuteReadWrite, out k);
        }

        public static IntPtr Alloc(uint size)
        {
            if (size == 0)
                return IntPtr.Zero;
            IntPtr ptr = ProcessImports.VirtualAllocEx(Handle, IntPtr.Zero, size, ProcessImports.AllocationType.Reserve | ProcessImports.AllocationType.Commit, ProcessImports.MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                Error.GetLastError();

            return ptr;
        }

        public static bool Free(IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero || size == 0)
                return false;
            if (!ProcessImports.VirtualFreeEx(Handle, ptr, size, ProcessImports.AllocationType.Decommit))
                Error.GetLastError();
            if (!ProcessImports.VirtualFreeEx(Handle, ptr, 0, ProcessImports.AllocationType.Release))
                Error.GetLastError();

            return true;
        }

        #region Write

        public static uint Write(int address, bool value)
        {
            return Write(address, value ? 1 : 0);
        }

        public static uint Write(int address, int value)
        {
            if (address == 0)
                throw new Exception("Write position is 0!");

            IntPtr byteWritten = IntPtr.Zero;
            IntPtr ptr = new IntPtr(value);
            if (!ProcessImports.WriteProcessMemory(Handle, new IntPtr(address), out ptr, 4, out byteWritten))
                Error.GetLastError();

            return (uint)byteWritten.ToInt32();
        }

        public static uint Write(int address, byte value)
        {
            return Write(address, new byte[1] { value });
        }

        public static uint Write(int address, float value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }

        public static uint Write(int address, short value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }

        public static uint Write(int address, ushort value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }

        public static uint Write(int address, params byte[] bytes)
        {
            return Write(address, bytes, bytes.Length);
        }

        public static uint Write(int address, byte[] arr, int count)
        {
            if (address == 0)
                throw new ArgumentException("Write address is 0!");

            uint byteWritten;
            if (!ProcessImports.WriteProcessMemory(Handle, new IntPtr(address), arr, (uint)count, out byteWritten))
                Error.GetLastError();

            return byteWritten;
        }

        public static uint Nop(int address, int count)
        {
            byte[] buf = new byte[count];
            for (int i = 0; i < count; i++)
                buf[i] = 0x90;

            return Write(address, buf);

        }

        #endregion

        #region Read

        public static bool ReadBool(int address)
        {
            return ReadInt(address) != 0;
        }

        public static int ReadInt(int address)
        {
            if (address == 0)
                throw new Exception("Read address is 0!");

            IntPtr rw;
            IntPtr puffer;
            if (!ProcessImports.ReadProcessMemory(Handle, new IntPtr(address), out puffer, new UIntPtr(4), out rw))
                Error.GetLastError();
            return puffer.ToInt32();
        }

        public static byte ReadByte(int address)
        {
            return ReadBytes(address, 1)[0];
        }

        public static float ReadFloat(int address)
        {
            return BitConverter.ToSingle(ReadBytes(address, 4), 0);
        }

        public static short ReadShort(int address)
        {
            return BitConverter.ToInt16(ReadBytes(address, 2), 0);
        }

        public static ushort ReadUShort(int address)
        {
            return BitConverter.ToUInt16(ReadBytes(address, 2), 0);
        }

        public static byte[] ReadBytes(int address, uint count)
        {
            byte[] arr = new byte[count];
            Read(address, arr, count);
            return arr;
        }

        public static uint Read(int address, byte[] buffer, uint count)
        {
            if (address == 0)
                throw new Exception("Read address is 0!");

            uint bytesRead = 0;
            if (!ProcessImports.ReadProcessMemory(Handle, new IntPtr(address), buffer, count, ref bytesRead))
                Error.GetLastError();

            return bytesRead;
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
                int address;
                if (!int.TryParse(message, System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out address))
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

        public static bool SetWindowText(String text)
        {
            return ProcessImports.SetWindowText(Handle, text);
        }

        public static String GetWindowText()
        {
            int length = ProcessImports.GetWindowTextLength(Handle);
            StringBuilder sb = new StringBuilder(length + 1);
            ProcessImports.GetWindowText(Handle, sb, sb.Capacity);
            return sb.ToString();
        }

        public static bool IsForeground()
        {
            return User.Window.GetWindowThreadProcessId(User.Window.GetForegroundWindow()) == ProcessID;
        }
    }
}
