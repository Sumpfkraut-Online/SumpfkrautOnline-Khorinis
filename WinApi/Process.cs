using System;
using System.Collections.Generic;
using System.Text;
using WinApi.Kernel;
using System.Runtime.InteropServices;
using System.Reflection;

namespace WinApi
{
    public static class Process
    {
        #region Injection

        /// <summary>
        /// Inject a dll into the process
        /// </summary>
        /// <param name="process"></param>
        /// <param name="dll"></param>
        /// <returns></returns>
        public static IntPtr LoadLibary(System.Diagnostics.Process process, String dll)
        {
            if (process == null || dll == null || String.IsNullOrWhiteSpace(dll))
                return IntPtr.Zero;

            byte[] dllb = Encoding.ASCII.GetBytes(dll);
            if (dllb == null || dllb.Length == 0)
                return IntPtr.Zero;

            //Alloc
            uint len = (uint)dllb.Length + 1;
            IntPtr dllbPtr = Kernel.Process.VirtualAllocEx(process.Handle, IntPtr.Zero, len, Kernel.Process.AllocationType.Reserve | Kernel.Process.AllocationType.Commit, Kernel.Process.MemoryProtection.ReadWrite);
            if (dllbPtr == IntPtr.Zero)
            {
                Kernel.Error.GetLastError();
            }

            //Write dll name
            uint tmp = 0;
            if (!Kernel.Process.WriteProcessMemory(process.Handle, dllbPtr, dllb, (uint)dllb.Length, out tmp))
            {
                Error.GetLastError();
            }

            IntPtr loadlib = Kernel.Process.GetProcAddress(WinApi.Kernel.Process.GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            return Kernel.Process.CreateRemoteThread(process.Handle, IntPtr.Zero, 0, loadlib, dllbPtr, 0, out tmp);
        }

        #endregion

        #region Initialization

        static IntPtr Handle = GetThisHandle();
        static uint ProcessID;

        static IntPtr GetThisHandle()
        {
            ProcessID = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
            System.Diagnostics.Process.EnterDebugMode();
            IntPtr retH = Kernel.Process.OpenProcess((uint)Kernel.Process.ProcessAccess.PROCESS_ALL_ACCESS, false, ProcessID);
            if (retH == IntPtr.Zero)
            {
                Kernel.Error.GetLastError();
            }
            return retH;
        }
        #endregion

        #region Memory Methods

        public static bool VirtualProtect(int address, uint size)
        {
            Kernel.Process.MemoryProtection k;
            return Kernel.Process.VirtualProtect(new IntPtr(address), size, Kernel.Process.MemoryProtection.ExecuteReadWrite, out k);
        }

        public static IntPtr Alloc(uint size)
        {
            if (size == 0)
                return IntPtr.Zero;
            IntPtr ptr = Kernel.Process.VirtualAllocEx(Handle, IntPtr.Zero, size, WinApi.Kernel.Process.AllocationType.Reserve | WinApi.Kernel.Process.AllocationType.Commit, WinApi.Kernel.Process.MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                Error.GetLastError();

            return ptr;
        }

        public static bool Free(IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero || size == 0)
                return false;
            if (!WinApi.Kernel.Process.VirtualFreeEx(Handle, ptr, (uint)size, Kernel.Process.AllocationType.Decommit))
                Kernel.Error.GetLastError();
            if (!WinApi.Kernel.Process.VirtualFreeEx(Handle, ptr, 0, Kernel.Process.AllocationType.Release))
                Kernel.Error.GetLastError();

            return true;
        }

        #region Write & Read

        public static uint Write(bool obj, int position)
        {
            return Write(obj == true ? 1 : 0, position);
        }

        public static uint Write(int obj, int position)
        {
            if (position == 0)
                throw new Exception("Process.Write position is 0!");

            IntPtr byteWritten = IntPtr.Zero;
            IntPtr ptr = new IntPtr(obj);
            if (!Kernel.Process.WriteProcessMemory(Handle, new IntPtr(position), out ptr, 4, out byteWritten))
                Error.GetLastError();

            return (uint)byteWritten.ToInt32();
        }

        public static uint Write(byte obj, int position)
        {
            return Write(new byte[1] { obj }, position);
        }

        public static uint Write(float obj, int position)
        {
            return Write(BitConverter.GetBytes(obj), position);
        }

        public static uint Write(ushort obj, int position)
        {
            return Write(BitConverter.GetBytes(obj), position);
        }

        public static uint Write(byte[] obj, int position)
        {
            if (position == 0)
                throw new Exception("Write position is 0!");

            uint byteWritten = 0;
            if (!Kernel.Process.WriteProcessMemory(Handle, new IntPtr(position), obj, (uint)obj.Length, out byteWritten))
                Error.GetLastError();

            return byteWritten;
        }

        public static bool ReadBool(int position)
        {
            return ReadInt(position) != 0;
        }

        public static int ReadInt(int position)
        {
            if (position == 0)
                throw new Exception("Process.Read position is 0!");

            IntPtr rw = IntPtr.Zero;
            IntPtr puffer = IntPtr.Zero;
            if (!Kernel.Process.ReadProcessMemory(Handle, new IntPtr(position), out puffer, new UIntPtr(sizeof(int)), out rw))
                Error.GetLastError();
            return (int)puffer.ToInt32();
        }

        public static byte ReadByte(int position)
        {
            return ReadBytes(position, 1)[0];
        }

        public static float ReadFloat(int position)
        {
            return BitConverter.ToSingle(ReadBytes(position, 4), 0);
        }

        public static ushort ReadUShort(int position)
        {
            return BitConverter.ToUInt16(ReadBytes(position, 2), 0);
        }

        public static byte[] ReadBytes(int position, uint count)
        {
            if (position == 0)
                throw new Exception("Process.Read position is 0!");

            byte[] bytes = new byte[count];
            UInt32 size = 0;
            if (!Kernel.Process.ReadProcessMemory(Handle, new IntPtr(position), bytes, count, ref size))
                Error.GetLastError();
            return bytes;
        }

        #endregion

        #endregion

        #region Calls

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
            Write(list.ToArray(), baseadress.ToInt32());

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

        static Hook[] hooks = new Hook[0];

        static readonly int runtimeInterface;
        static readonly int hookArg_DLLPath;
        static readonly int hookArg_Namespace;
        static readonly int hookArg_MethodName;
        static readonly int hookArg_Result;

        static Process()
        {
            Guid CLSID_CLRRuntimeHost = new Guid(0x90F1A06E, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);
            Guid IID_ICLRRuntimeHost = new Guid(0x90F1A06C, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);

            runtimeInterface = RuntimeEnvironment.GetRuntimeInterfaceAsIntPtr(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost).ToInt32();
            if (runtimeInterface == 0) throw new Exception("Runtime interface not found!");

            hookArg_DLLPath = AllocUnicodeString(typeof(Process).Assembly.Location);
            hookArg_Namespace = AllocUnicodeString(typeof(Process).FullName);
            hookArg_MethodName = AllocUnicodeString(typeof(Process).GetMethod("ApiHook").Name);
            hookArg_Result = Alloc(4).ToInt32();
        }

        static int AllocUnicodeString(string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            int ptr = Alloc((uint)(bytes.Length + 1)).ToInt32();
            Write(bytes, ptr);
            return ptr;
        }

        static void FreeUnicodeString(int address, string str)
        {
            Free(new IntPtr(address), (uint)Encoding.Unicode.GetByteCount(str) + 1);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);

        public static int ApiHook(string message)
        {
            try
            {
                int hookID;
                if (int.TryParse(message, out hookID) && hookID >= 0 && hookID < hooks.Length)
                {
                    var hook = hooks[hookID];
                    if (hook != null)
                        hook.Callback(hook);
                }
            }
            catch (Exception e)
            {
                MessageBox(IntPtr.Zero, e.ToString(), "Error", 0);
            }
            return 0;
        }

        public static Hook AddHook(HookCallback callback, int address, uint length, uint argCount = 0)
        {
            if (callback == null)
                throw new ArgumentNullException("Callback is null!");
            if (address == 0)
                throw new ArgumentNullException("Address is zero!");
            if (length < 5)
                throw new ArgumentException("Length is < 5!");

            // Check if there's already a hook
            for (int i = 0; i < hooks.Length; i++)
            {
                if (hooks[i] == null) continue;
                if ((address >= hooks[i].HookAddress && address < hooks[i].HookAddress + hooks[i].HookLength) ||
                    (address + length >= hooks[i].HookAddress && address + length < hooks[i].HookAddress + hooks[i].HookLength))
                {
                    throw new Exception(callback.Method.Name + " hook address is already taken by " + hooks[i].Callback.Method.Name);
                }
            }

            int eaxPtr = Alloc(28 + argCount * 4).ToInt32();
            int ebxPtr = eaxPtr + 4;
            int ecxPtr = ebxPtr + 4;
            int edxPtr = ecxPtr + 4;
            int ediPtr = edxPtr + 4;
            int ebpPtr = ediPtr + 4;
            int esiPtr = ebpPtr + 4;

            uint funcLen = 104 + argCount * 9 + length;
            int funcPtr = Alloc(funcLen).ToInt32();
            List<byte> funcBytes = new List<byte>((int)funcLen);

            // save registers and arguments
            funcBytes.Add(0xA3); funcBytes.AddRange(BitConverter.GetBytes(eaxPtr)); // mov [eaxPtr], EAX
            funcBytes.Add(0x89); funcBytes.Add(0x1D); funcBytes.AddRange(BitConverter.GetBytes(ebxPtr)); // mov [ebxPtr], EBX
            funcBytes.Add(0x89); funcBytes.Add(0x0D); funcBytes.AddRange(BitConverter.GetBytes(ecxPtr)); // mov [ecxPtr], ECX
            funcBytes.Add(0x89); funcBytes.Add(0x15); funcBytes.AddRange(BitConverter.GetBytes(edxPtr)); // mov [edxPtr], EDX
            funcBytes.Add(0x89); funcBytes.Add(0x3D); funcBytes.AddRange(BitConverter.GetBytes(ediPtr)); // mov [ediPtr], EDI
            funcBytes.Add(0x89); funcBytes.Add(0x2D); funcBytes.AddRange(BitConverter.GetBytes(ebpPtr)); // mov [ebpPtr], EBP
            funcBytes.Add(0x89); funcBytes.Add(0x35); funcBytes.AddRange(BitConverter.GetBytes(esiPtr)); // mov [esiPtr], ESI

            for (int i = 1; i <= argCount; i++)
            {
                funcBytes.Add(0x8B); funcBytes.Add(0x44); funcBytes.Add(0x24); funcBytes.Add((byte)(4 * i)); // mov EAX, [esp + 4 * i]
                funcBytes.Add(0xA3); funcBytes.AddRange(BitConverter.GetBytes(esiPtr + 4 * i)); // mov [esiPtr + 4*i], EAX
            }

            // call .NET method
            funcBytes.Add(0x60);//pushad

            // find hook ID
            int id = hooks.Length;
            for (int i = 0; i < hooks.Length; i++)
            {
                if (hooks[i] == null)
                {
                    id = i;
                    break;
                }
            }

            // reallocation needed
            if (id == hooks.Length)
            {
                Hook[] newArr = new Hook[hooks.Length + 1];
                Array.Copy(hooks, newArr, hooks.Length);
                hooks = newArr;
            }

            int idAddress = AllocUnicodeString(id.ToString());
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(hookArg_Result)); // PUSH arg5
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(idAddress)); // PUSH arg4 
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(hookArg_MethodName)); // PUSH arg3
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(hookArg_Namespace)); // PUSH arg2
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(hookArg_DLLPath)); // PUSH arg1
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(runtimeInterface)); // PUSH this

            funcBytes.Add(0x8B); funcBytes.Add(0x0D); funcBytes.AddRange(BitConverter.GetBytes(runtimeInterface)); // mov ECX, [runtimeInterface]
            funcBytes.Add(0xFF); funcBytes.Add(0x51); funcBytes.Add(0x2C);// call [ECX+0x2C]

            funcBytes.Add(0x61);//popad

            // copy registers back
            funcBytes.Add(0xA1); funcBytes.AddRange(BitConverter.GetBytes(eaxPtr)); // mov EAX, [eaxPtr]
            funcBytes.Add(0x8B); funcBytes.Add(0x1D); funcBytes.AddRange(BitConverter.GetBytes(ebxPtr)); // mov EBX, [ebxPtr]
            funcBytes.Add(0x8B); funcBytes.Add(0x0D); funcBytes.AddRange(BitConverter.GetBytes(ecxPtr)); // mov ECX, [ecxPtr]
            funcBytes.Add(0x8B); funcBytes.Add(0x15); funcBytes.AddRange(BitConverter.GetBytes(edxPtr)); // mov EDX, [edxPtr]
            funcBytes.Add(0x8B); funcBytes.Add(0x3D); funcBytes.AddRange(BitConverter.GetBytes(ediPtr)); // mov EDI, [ediPtr]
            funcBytes.Add(0x8B); funcBytes.Add(0x2D); funcBytes.AddRange(BitConverter.GetBytes(ebpPtr)); // mov EBP, [ebpPtr]
            funcBytes.Add(0x8B); funcBytes.Add(0x35); funcBytes.AddRange(BitConverter.GetBytes(esiPtr)); // mov ESI, [esiPtr]

            // old code
            byte[] oldCode = ReadBytes(address, length);
            Hook h = new Hook((uint)id, idAddress, address, callback, eaxPtr, funcPtr, funcLen, funcPtr + funcBytes.Count, oldCode, argCount);
            hooks[id] = h;
            funcBytes.AddRange(oldCode);

            // jmp back
            funcBytes.Add(0xE9); funcBytes.AddRange(BitConverter.GetBytes((int)(address + length) - (funcPtr + funcBytes.Count + 4)));

            // write hook function
            Write(funcBytes.ToArray(), funcPtr);

            // write hook jmp
            byte[] hookJmp = new byte[length];
            hookJmp[0] = 0xE9;
            byte[] jmpAddr = BitConverter.GetBytes(funcPtr - (address + 5));
            hookJmp[1] = jmpAddr[0];
            hookJmp[2] = jmpAddr[1];
            hookJmp[3] = jmpAddr[2];
            hookJmp[4] = jmpAddr[3];

            for (int i = 5; i < length; i++) // doesn't matter actually, but some programs like CheatEngine can't handle 0x00 instead of 0x90
                hookJmp[i] = 0x90;

            Write(hookJmp, address);

            return h;
        }

        public static void RemoveHook(Hook hook)
        {
            hooks[hook.ID] = null;

            // write old code
            Write(hook.GetOldCode(), hook.HookAddress);

            // free hook function
            Free(new IntPtr(hook.NewCodeAddress), (uint)hook.NewCodeLength);

            // free info 
            Free(new IntPtr(hook.InfoAddress), 20 + 4 * hook.ArgumentCount);

            // free id
            FreeUnicodeString(hook.IDAddress, hook.ID.ToString());
        }

        #endregion

        public static bool SetWindowText(String text)
        {
            return Kernel.Process.SetWindowText(Handle, text);
        }

        public static String GetWindowText()
        {
            int length = Kernel.Process.GetWindowTextLength(Handle);
            StringBuilder sb = new StringBuilder(length + 1);
            Kernel.Process.GetWindowText(Handle, sb, sb.Capacity);
            return sb.ToString();
        }

        public static bool IsForeground()
        {
            return User.Window.GetWindowThreadProcessId(User.Window.GetForegroundWindow()) == ProcessID;
        }
    }
}
