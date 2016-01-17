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
            IntPtr ptr = WinApi.Kernel.Process.VirtualAllocEx(Handle, IntPtr.Zero, size, WinApi.Kernel.Process.AllocationType.Reserve | WinApi.Kernel.Process.AllocationType.Commit, WinApi.Kernel.Process.MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                Kernel.Error.GetLastError();

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
                throw new Exception("Process.Write position is 0!");

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

        public static HookInfos Hook(String dll, MethodInfo methods, int addr, int size, int sizeParam)
        {
            if (addr == 0 || size < 5 || methods == null)
                throw new Exception("Process.Hook: addr == 0 || size < 5 || methods == null");

            IntPtr injectFunction = Kernel.Process.GetProcAddress(Kernel.Process.GetModuleHandle("NetInject.dll"), "LoadNetDllEx");

            if (injectFunction == IntPtr.Zero)
                throw new Exception("Add Hook : Handle or Function not Found");

            HookInfos rValue = new HookInfos();
            IntPtr varPtr = Alloc(4 + (uint)sizeParam * 4 + 4 + 4);
            IntPtr ecxPtr = varPtr;
            IntPtr eaxPtr = varPtr + 4 + sizeParam * 4;
            IntPtr ebxPtr = varPtr + 4 + sizeParam * 4 + 4;
            int length = 0;

            if (varPtr == IntPtr.Zero)
                throw new Exception("Add Hook : Allocate Error");

            //Alte Anweisung sichern
            if (!VirtualProtect(addr, (uint)(size + 3)))
                throw new Exception("Add Hook : Virtual Protection Error");

            byte[] oldFunc = ReadBytes(addr, (uint)size);

            //Neue funktion:
            List<byte> list = new List<byte>();

            //This pointer (ecx) in Speicher schreiben
            list.Add(0x89); list.Add(0x0D);// mov [Address], ecx
            list.AddRange(BitConverter.GetBytes(ecxPtr.ToInt32()));

            list.Add(0xA3); // mov [Address], eax
            list.AddRange(BitConverter.GetBytes(eaxPtr.ToInt32()));

            list.Add(0x89); list.Add(0x1D); // mov [Address], ebx
            list.AddRange(BitConverter.GetBytes(ebxPtr.ToInt32()));

            //Statt die Parameter an den dafür vorgesehenen Platz zu schreiben, könnten auch die Register-Werte eingespeichert werden.
            for (int i = 1; i <= sizeParam; i++)
            {
                list.Add(0x8B); list.Add(0x4C); list.Add(0x24);//8B 4C 24 08 -> mov     ecx, [esp+8]
                list.Add((Byte)(4 * i));
                list.Add(0x89); list.Add(0x0D);// mov [Address], ecx
                list.AddRange(BitConverter.GetBytes(ecxPtr.ToInt32() + 4 * (i)));
            }

            //pointer zurück in ecx schreiben
            list.Add(0x8B); list.Add(0x0D);
            list.AddRange(BitConverter.GetBytes(ecxPtr.ToInt32()));

            list.Add(0x60);//pushad

            NETINJECTPARAMS parameters = NETINJECTPARAMS.Create(dll, methods.DeclaringType.FullName, methods.Name, varPtr.ToInt32() + "");
            list.Add(0x68);//Parameter für LoadNetDllEx pushen
            list.AddRange(BitConverter.GetBytes(parameters.Address));

            length = list.Count + 1 + 4 + 1 + 1 + oldFunc.Length + 1 + 4 + 1 + 5;
            IntPtr newASM = Alloc((uint)length);
            ////Funktion callen
            list.Add(0xE8);
            list.AddRange(BitConverter.GetBytes(injectFunction.ToInt32() - (newASM.ToInt32() + list.Count) - 4));

            list.Add(0x59);//pop
            list.Add(0x61);//popad

            //Gespeichertes EAX ins Register schreiben
            list.Add(0xA1);
            list.AddRange(BitConverter.GetBytes(eaxPtr.ToInt32()));

            rValue.oldFuncInNewFunc = new IntPtr(newASM.ToInt32() + list.Count);
            //Alten Code:
            list.AddRange(oldFunc);

            list.Add(0x68);
            list.AddRange(BitConverter.GetBytes(addr + size));
            list.Add(0xC3);//RTN



            //IntPtr newASM = Alloc((uint)size + 120 + 3);//Neue Funktion

            if (Write(list.ToArray(), newASM.ToInt32()) == 0)//Neue Funktion schreiben
                throw new Exception("Add Hook : Writing Failed!");
            byte[] newJMP = new byte[size];
            byte[] asmAddress = BitConverter.GetBytes(newASM.ToInt32() - addr - 5);
            newJMP[0] = 0xE9;
            newJMP[1] = asmAddress[0];
            newJMP[2] = asmAddress[1];
            newJMP[3] = asmAddress[2];
            newJMP[4] = asmAddress[3];

            if (Write(newJMP, addr) == 0)
                throw new Exception("Add Hook : Writing Failed 2!");

            rValue.oldFuncAddr = new IntPtr(addr);
            rValue.oldFuncSize = size;
            rValue.NewFuncAddr = newASM;
            rValue.NewFuncSize = length;
            rValue.oldFunc = oldFunc;

            return rValue;
        }

        public static void RemoveHook(HookInfos hi)
        {
            Write(hi.oldFunc, hi.oldFuncAddr.ToInt32());
            Free(hi.NewFuncAddr, (uint)hi.NewFuncSize);
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
