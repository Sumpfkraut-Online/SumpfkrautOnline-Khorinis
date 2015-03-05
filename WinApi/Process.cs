using System;
using System.Collections.Generic;
using System.Text;
using WinApi.Kernel;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Forms;

namespace WinApi
{
    public class Process
    {
        bool process_thisprocess = false;
        #region static Functions
        /// <summary>
        /// Liefer eine Instanz der Klasse Process zurück, mit der Process ID.
        /// </summary>
        /// <param name="caption">Name des Fensters</param>
        /// <returns>Process, bei Fehler wird eine Ausnahme aufgerufen</returns>
        public static Process getProcessByWindowCaption(String caption)
        {
            IntPtr hwnd = User.Window.FindWindowByCaption(new IntPtr(), caption);
            if (hwnd == IntPtr.Zero)
                Error.GetLastError();


            uint processid = User.Window.GetWindowThreadProcessId(hwnd);
            Process Process= new Process(processid);
            Process.Handle = hwnd;
            return Process;
        }

        /// <summary>
        /// Programm wird gestartet
        /// </summary>
        /// <param name="filename">Dateiname des zu startenen Programmes</param>
        /// <returns></returns>
        public static Process Start(string filename)
        {
            return new Process(System.Diagnostics.Process.Start(filename).Handle);
        }

        /// <summary>
        /// Programm wird gestartet, Argumente können angegeben werden.
        /// </summary>
        /// <param name="filename">Dateiname des zu startenen Programmes</param>
        /// <param name="arguments">Parameter mit denen das Programm gestartet werden soll</param>
        /// <returns></returns>
        public static Process Start(string filename, String arguments)
        {
            return new Process(System.Diagnostics.Process.Start(filename, arguments).Handle);
        }

        /// <summary>
        /// Programm wird gestartet
        /// </summary>
        /// <param name="psi"></param>
        /// <returns></returns>
        public static Process Start(System.Diagnostics.ProcessStartInfo psi)
        {
            return new Process(System.Diagnostics.Process.Start(psi).Handle);
        }

        /// <summary>
        /// Liefert eine Instanz der Process Klasse zurück.
        /// </summary>
        /// <returns></returns>
        public static Process ThisProcess()
        {
            Process rv = new Process((uint)System.Diagnostics.Process.GetCurrentProcess().Id);
            System.Diagnostics.Process.EnterDebugMode();
            rv.connect(Kernel.Process.ProcessAccess.PROCESS_ALL_ACCESS);
            rv.process_thisprocess = true;
            return rv;
        }
        #endregion

        public uint ProcessID { get; protected set; }
        public IntPtr Handle { get; protected set; }




        public Process(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                throw new Exception("Ungültiger handle");
            this.Handle = handle;
        }

        public Process(uint processid)
        {
            this.ProcessID = processid;
        }

        public Process(Process process)
        {
            this.ProcessID = process.ProcessID;
            this.Handle = process.Handle;
        }

        public void connect(WinApi.Kernel.Process.ProcessAccess access)
        {
            Handle = Kernel.Process.OpenProcess((uint)access, false, this.ProcessID);
            if (Handle == IntPtr.Zero)
                Kernel.Error.GetLastError();            
        }

        public T STDCALL<T>(UInt32 funcPtr, CallValue[] args) where T : CallValue, new()
        {
            return MAKECALL<T>(CALLTYPE.STDCALL, 0, 0, funcPtr, args);
        }

        public T CDECLCALL<T>(UInt32 funcPtr, CallValue[] args) where T : CallValue, new()
        {
            return MAKECALL<T>(CALLTYPE.CDECLCALL, 0,0, funcPtr, args);
        }

        public T THISCALL<T>(UInt32 thisPtr, UInt32 funcPtr, CallValue[] args) where T : CallValue, new()
        {
            if (thisPtr == 0)
                throw new Exception("This-pointer not found!");
            return MAKECALL<T>(CALLTYPE.THISCALL ,thisPtr,0, funcPtr, args);
        }

        public T FASTCALL<T>(UInt32 para1, UInt32 para2, UInt32 funcPtr, CallValue[] args) where T : CallValue, new()
        {
            return MAKECALL<T>(CALLTYPE.FASTCALL, para1, para2, funcPtr, args);
        }

        enum CALLTYPE
        {
            STDCALL,
            THISCALL,
            CDECLCALL,
            FASTCALL
        }

        public delegate void call();
        private T MAKECALL<T>(CALLTYPE type, UInt32 thisPtr, UInt32 fcallArg, UInt32 funcPtr, CallValue[] args) where T : CallValue, new()
        {
            if (funcPtr == 0)
                throw new Exception("Funcptr not found");
            T returnValue = new T();

            List<byte> list = new List<byte>();

            list.Add(0x60);//pushad

            int argsCount = 0;

            //Argumente pushen
            if (args != null)
            {
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    for (int i2 = args[i].getCallParam().Count - 1; i2 >= 0; i2--)
                    {
                        argsCount++;
                        list.Add(0x68);
                        list.AddRange(args[i].getCallParam()[i2]);
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
            

            uint length = (uint)(list.Count + 1 + 4 + 1 + 1);
            if (returnValue.ValueLength() != 0)
                length += 1 + returnValue.ValueLength();

            if (type == CALLTYPE.CDECLCALL)
                length += 3;
            

            IntPtr baseadress = Alloc(length);

            IntPtr returnAddress = IntPtr.Zero;
            if(returnValue.ValueLength() != 0)
                returnAddress = Alloc(returnValue.ValueLength());


            //call
            list.Add(0xE8);
            list.AddRange(BitConverter.GetBytes((uint)(funcPtr - (baseadress.ToInt32() + list.Count) - 4))); // - Aktuelle Addresse - 4
            
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
            if (process_thisprocess)
            {
                call mc = (call)Marshal.GetDelegateForFunctionPointer(baseadress, typeof(call));
                mc();
            }
            else
            {
                uint threadID;
                IntPtr hThread = Kernel.Process.CreateRemoteThread(Handle, IntPtr.Zero, 0, baseadress, IntPtr.Zero, 0, out threadID);
                WaitForThreadToExit(hThread);
                CloseHandle(hThread);
            }
            
            //Bisschen aufräumen
            Free(baseadress, length);

            if (returnValue.ValueLength() != 0)
            {
                returnValue.Initialize(this, ReadInt(returnAddress.ToInt32()));//Adresse in der die return Value gespeichert wurde
                Free(returnAddress, returnValue.ValueLength());
                return returnValue;
            }
            else
            {
                return null;
            }
            
        }

        public void CloseHandle(IntPtr handle)
        {
            if(Kernel.Process.CloseHandle(handle) == 0)
                Kernel.Error.GetLastError();
        }

        public HookInfos Hook(String dll, MethodInfo methods, int addr, int size, int sizeParam)
        {
            if (addr == 0 || size < 5 || methods == null)
                throw new Exception();

            IntPtr injectFunction = Kernel.Process.GetProcAddress(Kernel.Process.GetModuleHandle("NetInject.dll"), "LoadNetDllEx");

            if (injectFunction == IntPtr.Zero)
                throw new Exception("Add Hook : Handle or Function not Found");

            HookInfos rValue = new HookInfos();
            IntPtr varPtr = Alloc(4 + (uint)sizeParam*4 + 4 + 4);
            IntPtr ecxPtr = varPtr;
            IntPtr eaxPtr = varPtr + 4 + sizeParam * 4;
            IntPtr ebxPtr = varPtr + 4 + sizeParam * 4 + 4;
            int length = 0;

            if(varPtr == IntPtr.Zero)
                throw new Exception("Add Hook : Allocate Error");

            //Alte Anweisung sichern
            if (!VirtualProtect(addr, size + 3))
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
            
            NETINJECTPARAMS parameters = NETINJECTPARAMS.Create(this, dll, methods.DeclaringType.FullName, methods.Name, varPtr.ToInt32() + "");
            list.Add(0x68);//Parameter für LoadNetDllEx pushen
            list.AddRange(BitConverter.GetBytes(parameters.Address));

            length = list.Count + 1 + 4 + 1 + 1 + oldFunc.Length + 1 + 4 + 1;
            IntPtr newASM = Alloc((uint)length);
            ////Funktion callen
            list.Add(0xE8);
            list.AddRange(BitConverter.GetBytes(injectFunction.ToInt32() - (newASM.ToInt32() + list.Count) - 4));

            list.Add(0x59);//pop
            list.Add(0x61);//popad

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

        public void EndHook(HookInfos hi)
        {
            Write(hi.oldFunc, hi.oldFuncAddr.ToInt32());
            Free(hi.NewFuncAddr, (uint)hi.NewFuncSize);
        }

        public bool VirtualProtect(int address, int size)
        {
            Kernel.Process.MemoryProtection k;
            return Kernel.Process.VirtualProtect(new IntPtr(address), (uint)size, Kernel.Process.MemoryProtection.ExecuteReadWrite, out k);
        }


        public IntPtr Alloc(uint size)
        {
            if (size == 0)
                return IntPtr.Zero;
            IntPtr ptr=  WinApi.Kernel.Process.VirtualAllocEx(Handle, IntPtr.Zero, size, WinApi.Kernel.Process.AllocationType.Reserve | WinApi.Kernel.Process.AllocationType.Commit, WinApi.Kernel.Process.MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                Kernel.Error.GetLastError();
            return ptr;
        }

        public bool Free(IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero || size == 0)
                return false;
            if (!WinApi.Kernel.Process.VirtualFreeEx(Handle, ptr, (uint)size, Kernel.Process.AllocationType.Decommit))
                Kernel.Error.GetLastError();
            if (!WinApi.Kernel.Process.VirtualFreeEx(Handle, ptr, 0, Kernel.Process.AllocationType.Release))
                Kernel.Error.GetLastError();
            return true;
        }

        static byte[] swap(byte[] value)
        {
            byte[] sw = new byte[value.Length];
            for (int i = value.Length-1, i2 = 0; i >= 0; i--, i2++)
            {
                sw[i] = value[i2];
            }
            return sw;
        }

        public static void WaitForThreadToExit(IntPtr hThread)
        {
            Kernel.Process.WaitForSingleObject(hThread, Kernel.Process.INFINITE);
            uint exitCode;
            Kernel.Process.GetExitCodeThread(hThread, out exitCode);

        }

        

        public uint Write(int obj, int position)
        {
            if (position == 0)
                throw new Exception("Position = 0");
            IntPtr byteWritten = IntPtr.Zero;
            IntPtr ptr = new IntPtr(obj);
            if (!Kernel.Process.WriteProcessMemory(Handle, new IntPtr(position), out ptr, (uint)sizeof(int), out byteWritten))
                Error.GetLastError();

            return (uint)byteWritten.ToInt32();
        }

        public uint Write(float obj, int position)
        {
            byte[] arr = BitConverter.GetBytes(obj);
            return Write(arr, position);
        }

        public uint Write(ushort obj, int position)
        {
            byte[] arr = BitConverter.GetBytes(obj);
            return Write(arr, position);
        }

        public uint Write(byte[] obj, int position)
        {
            uint byteWritten = 0;
            if (!Kernel.Process.WriteProcessMemory(Handle, new IntPtr(position), obj, (uint)obj.Length, out byteWritten))
                Error.GetLastError();

            return (uint)byteWritten;
        }

        public uint FillWithNull(int startPosition, int EndPosition)
        {
            byte[] arr = new byte[EndPosition - startPosition];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = 0x90;
            return Write(arr, startPosition);
        }

        public int ReadInt(int position)
        {
            IntPtr rw = IntPtr.Zero;
            IntPtr puffer = IntPtr.Zero;
            if (!Kernel.Process.ReadProcessMemory(Handle, new IntPtr(position), out puffer, new UIntPtr(sizeof(int)), out rw))
                Error.GetLastError();
            return (int)puffer.ToInt32();
        }

        public float ReadFloat(int position)
        {
            byte[] floatArr = ReadBytes(position, 4);
            return BitConverter.ToSingle(floatArr, 0);
        }

        public ushort ReadUShort(int position)
        {
            byte[] floatArr = ReadBytes(position, 2);
            return BitConverter.ToUInt16(floatArr, 0);
        }

        public byte[] ReadBytes(int position, UInt32 count)
        {
            byte[] bytes = new byte[count];
            UInt32 size = 0;
            if (!Kernel.Process.ReadProcessMemory(Handle, new IntPtr(position), bytes, count, ref size))
                Error.GetLastError();
            return bytes;
        }

        public byte ReadByte(int position)
        {
            UInt32 count = 1;
            byte[] bytes = new byte[count];
            UInt32 size = 0;

            if (!Kernel.Process.ReadProcessMemory(Handle, new IntPtr(position), bytes, count, ref size))
                Error.GetLastError();
            return bytes[0];
        }

        public IntPtr CreateThread(IntPtr funcHandle, IntPtr paramPtr)
        {
            uint t = 0;
            return WinApi.Kernel.Process.CreateRemoteThread(Handle, IntPtr.Zero, 0, funcHandle, paramPtr, 0, out t);
        }

        public IntPtr LoadLibary(String dll)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            byte[] dllb = enc.GetBytes(dll);
            IntPtr dllbPtr = Alloc((uint)dllb.Length + 1);
            Write(dllb, dllbPtr.ToInt32());
            IntPtr loadlib = WinApi.Kernel.Process.GetProcAddress(WinApi.Kernel.Process.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            return CreateThread(loadlib, dllbPtr);
        }

        public bool SetWindowText(String text )
        {
            return WinApi.Kernel.Process.SetWindowText(this.Handle, text);
        }

        public String GetWindowText()
        {
            int length = WinApi.Kernel.Process.GetWindowTextLength(this.Handle);
            StringBuilder sb = new StringBuilder(length + 1);
            WinApi.Kernel.Process.GetWindowText(this.Handle, sb, sb.Capacity);
            return sb.ToString();
        }
    }


}
