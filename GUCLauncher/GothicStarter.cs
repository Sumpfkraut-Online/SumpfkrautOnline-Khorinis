using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GUCLauncher
{
    static class GothicStarter
    {
        public static void Start(string folder, string ip, ushort port, byte[] pw)
        {
            string projectFolder = Path.GetFullPath(folder);
            string dllName = Path.Combine(projectFolder, "GUC.dll");
            string g2App = Configuration.GothicApp;
            string zSpyApp = Configuration.zSpyApp;

            if (!File.Exists(dllName))
            {
                throw new FileNotFoundException(dllName + " not found!");
            }

            if (!File.Exists(g2App))
            {
                throw new FileNotFoundException(g2App + " not found!");
            }

            if (!Configuration.ValidateFileHash(g2App, Configuration.HashFile.Gothic2))
            {
                throw new Exception(g2App + " is wrong version!");
            }

            ProcessStartInfo g2Psi = new ProcessStartInfo(g2App);
            
            if (File.Exists(zSpyApp))
            {
                string logFolder = Path.GetFullPath(Path.Combine(projectFolder, "Log"));
                if (!Directory.Exists(logFolder))
                    Directory.CreateDirectory(logFolder);

                ProcessStartInfo zSpyPsi = new ProcessStartInfo(zSpyApp);
                zSpyPsi.WorkingDirectory = logFolder;
                Process.Start(zSpyPsi);
                g2Psi.Arguments = string.Format("-zlog:{0},s", Configuration.zSpyLevel);
            }

            g2Psi.UseShellExecute = false;
            g2Psi.EnvironmentVariables.Add("GUCProjectPath", projectFolder);
            g2Psi.EnvironmentVariables.Add("GUCServerIP", ip);
            g2Psi.EnvironmentVariables.Add("GUCServerPassword", Convert.ToBase64String(pw == null ? new byte[0] : pw));
            g2Psi.EnvironmentVariables.Add("GUCServerPort", port.ToString());
            g2Psi.EnvironmentVariables.Add("GUCGothicPath", Configuration.GothicPath);
            
            Process process = Process.Start(g2Psi);
            SuspendProcess(process);

            Inject(process, dllName);

            ResumeProcess(process);
        }

        #region Injection

        [Flags]
        enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        enum MemoryProtection : uint
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, out UInt32 lpNumberOfBytesWritten);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        /*[DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);*/

        static void Inject(Process process, string dllName)
        {
            const int funcLen = 1 + 10 + 20 + 34 + 34 + 19 + 44 + 1 + 6 + 5;
            List<byte> funcBytes = new List<byte>((int)funcLen);
            IntPtr funcPtr = Alloc(process, funcLen);

            funcBytes.Add(0x60);//pushad

            #region Load MSCOREE.dll

            IntPtr kernelH = GetModuleHandle("kernel32.dll");
            if (kernelH == IntPtr.Zero) throw new Exception("GetKernel: " + Marshal.GetLastWin32Error());
            IntPtr loadLibH = GetProcAddress(kernelH, "LoadLibraryA");
            if (loadLibH == IntPtr.Zero) throw new Exception("GetLoadLibrary: " + Marshal.GetLastWin32Error());

            byte[] libBytes = Encoding.ASCII.GetBytes("mscoree.dll");
            IntPtr libArg = Alloc(process, (uint)(libBytes.Length + 1));
            Write(process, libArg, libBytes);

            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(libArg.ToInt32())); // PUSH
            funcBytes.Add(0xE8); funcBytes.AddRange(BitConverter.GetBytes(loadLibH.ToInt32() - (funcPtr.ToInt32() + funcBytes.Count + 4))); // CALL

            #endregion

            #region Create CLRInstance

            Guid CLSID_CLRMetaHost = new Guid(0x9280188d, 0xe8e, 0x4867, 0xb3, 0xc, 0x7f, 0xa8, 0x38, 0x84, 0xe8, 0xde);
            Guid IID_ICLRMetaHost = new Guid(0xD332DB9E, 0xB9B3, 0x4125, 0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16);

            IntPtr mscoreeH = GetModuleHandle("mscoree.dll");
            if (mscoreeH == IntPtr.Zero) throw new Exception("GetMscoree: " + Marshal.GetLastWin32Error());
            IntPtr ciH = GetProcAddress(mscoreeH, "CLRCreateInstance");
            if (ciH == IntPtr.Zero) throw new Exception("GetCLRCreateInstance: " + Marshal.GetLastWin32Error());

            IntPtr ciArg1 = Alloc(process, 16); Write(process, ciArg1, CLSID_CLRMetaHost.ToByteArray());
            IntPtr ciArg2 = Alloc(process, 16); Write(process, ciArg2, IID_ICLRMetaHost.ToByteArray());
            IntPtr pMetaHost = Alloc(process, 4);

            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(pMetaHost.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(ciArg2.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(ciArg1.ToInt32())); // PUSH

            funcBytes.Add(0xE8); funcBytes.AddRange(BitConverter.GetBytes(ciH.ToInt32() - (funcPtr.ToInt32() + funcBytes.Count + 4))); // CALL

            #endregion

            #region Get CLRRuntimeInfo

            Guid IID_ICLRRuntimeInfo = new Guid(0xBD39D1D2, 0xBA2F, 0x486a, 0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91);

            byte[] verBytes = Encoding.Unicode.GetBytes("v4.0.30319");
            IntPtr rtArg1 = Alloc(process, (uint)(verBytes.Length + 1));
            Write(process, rtArg1, verBytes);

            IntPtr rtArg2 = Alloc(process, 16);
            Write(process, rtArg2, IID_ICLRRuntimeInfo.ToByteArray());

            IntPtr pRuntimeInfo = Alloc(process, 4);

            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(pRuntimeInfo.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(rtArg2.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(rtArg1.ToInt32())); // PUSH

            funcBytes.Add(0xA1); funcBytes.AddRange(BitConverter.GetBytes(pMetaHost.ToInt32())); // mov EAX, pMetaHost
            funcBytes.Add(0x8B); funcBytes.Add(0x08); // mov ECX, [EAX]
            funcBytes.Add(0x8B); funcBytes.Add(0x41); funcBytes.Add(0xC); // mov EAX, [ECX+0x0C]
            funcBytes.Add(0x8B); funcBytes.Add(0x15); funcBytes.AddRange(BitConverter.GetBytes(pMetaHost.ToInt32()));// mov EDX, pMetaHost
            funcBytes.Add(0x52); // push EDX
            funcBytes.Add(0xFF); funcBytes.Add(0xD0); // call EAX

            #endregion

            #region Get Interface

            Guid CLSID_CLRRuntimeHost = new Guid(0x90F1A06E, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);
            Guid IID_ICLRRuntimeHost = new Guid(0x90F1A06C, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);

            IntPtr giArg1 = Alloc(process, 16);
            Write(process, giArg1, CLSID_CLRRuntimeHost.ToByteArray());

            IntPtr giArg2 = Alloc(process, 16);
            Write(process, giArg2, IID_ICLRRuntimeHost.ToByteArray());

            IntPtr pClrRuntimeHost = Alloc(process, 4);

            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(pClrRuntimeHost.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(giArg2.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(giArg1.ToInt32())); // PUSH

            funcBytes.Add(0xA1); funcBytes.AddRange(BitConverter.GetBytes(pRuntimeInfo.ToInt32())); // mov EAX, pRuntimeInfo
            funcBytes.Add(0x8B); funcBytes.Add(0x08); // mov ECX, [EAX]
            funcBytes.Add(0x8B); funcBytes.Add(0x41); funcBytes.Add(0x24); // mov EAX, [ECX+0x24]
            funcBytes.Add(0x8B); funcBytes.Add(0x15); funcBytes.AddRange(BitConverter.GetBytes(pRuntimeInfo.ToInt32()));// mov EDX, pRuntimeInfo
            funcBytes.Add(0x52); // push EDX
            funcBytes.Add(0xFF); funcBytes.Add(0xD0); // call EAX

            #endregion

            #region Start CLR

            funcBytes.Add(0xA1); funcBytes.AddRange(BitConverter.GetBytes(pClrRuntimeHost.ToInt32())); // mov EAX, pClrRuntimeHost
            funcBytes.Add(0x8B); funcBytes.Add(0x08);// mov ECX, [EAX]
            funcBytes.Add(0x8B); funcBytes.Add(0x41); funcBytes.Add(0xC);// mov EAX, [ECX+0x0C]
            funcBytes.Add(0x8B); funcBytes.Add(0x15); funcBytes.AddRange(BitConverter.GetBytes(pClrRuntimeHost.ToInt32()));// mov EDX, pClrRuntimeHost
            funcBytes.Add(0x52); // push EDX
            funcBytes.Add(0xFF); funcBytes.Add(0xD0); // call EAX

            #endregion

            #region Call .NET GUC-Main

            byte[] dllBytes = Encoding.Unicode.GetBytes(dllName);
            IntPtr dllPath = Alloc(process, (uint)(dllBytes.Length + 1));
            Write(process, dllPath, dllBytes);

            byte[] typeBytes = Encoding.Unicode.GetBytes("GUC.Program");
            IntPtr typeName = Alloc(process, (uint)(typeBytes.Length + 1));
            Write(process, typeName, typeBytes);

            byte[] methodBytes = Encoding.Unicode.GetBytes("Main");
            IntPtr methodName = Alloc(process, (uint)(methodBytes.Length + 1));
            Write(process, methodName, methodBytes);

            //byte[] callArgBytes = Encoding.Unicode.GetBytes(address + '\n' + projectName);
            IntPtr callArg = IntPtr.Zero;//Alloc(process, (uint)(callArgBytes.Length + 1));
            //Write(process, callArg, callArgBytes);

            IntPtr result = Alloc(process, 4);

            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(result.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(callArg.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(methodName.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(typeName.ToInt32())); // PUSH
            funcBytes.Add(0x68); funcBytes.AddRange(BitConverter.GetBytes(dllPath.ToInt32())); // PUSH

            funcBytes.Add(0xA1); funcBytes.AddRange(BitConverter.GetBytes(pClrRuntimeHost.ToInt32())); // mov EAX, pClrRuntimeHost
            funcBytes.Add(0x8B); funcBytes.Add(0x08);// mov ECX, [EAX]
            funcBytes.Add(0x8B); funcBytes.Add(0x41); funcBytes.Add(0x2C);// mov EAX, [ECX+0x2C]
            funcBytes.Add(0x8B); funcBytes.Add(0x15); funcBytes.AddRange(BitConverter.GetBytes(pClrRuntimeHost.ToInt32()));// mov EDX, [pClrRuntimeHost]
            funcBytes.Add(0x52); // push EDX
            funcBytes.Add(0xFF); funcBytes.Add(0xD0); // call EAX

            #endregion

            funcBytes.Add(0x61);//popad

            //original code
            funcBytes.AddRange(new byte[6] { 0x64, 0xA1, 0, 0, 0, 0 });

            //jmp back
            funcBytes.Add(0xE9); funcBytes.AddRange(BitConverter.GetBytes(0x004267D6 - (funcPtr.ToInt32() + funcBytes.Count + 4)));

            Write(process, funcPtr, funcBytes.ToArray());

            // write hook jmp
            byte[] hookJmp = new byte[6];
            hookJmp[0] = 0xE9;
            byte[] jmpAddr = BitConverter.GetBytes(funcPtr.ToInt32() - 0x4267D5);
            hookJmp[1] = jmpAddr[0];
            hookJmp[2] = jmpAddr[1];
            hookJmp[3] = jmpAddr[2];
            hookJmp[4] = jmpAddr[3];
            hookJmp[5] = 0x90;

            Write(process, new IntPtr(0x4267D0), hookJmp);

            /*
            uint threadID;
            IntPtr threadH = CreateRemoteThread(process.Handle, IntPtr.Zero, 0, funcPtr, IntPtr.Zero, 0, out threadID);
            if (threadH == IntPtr.Zero) throw new Exception("CreateRemoteThread: " + Marshal.GetLastWin32Error());

            WaitForSingleObject(threadH, 0xFFFFFFFF);

            #region Freedom

            Free(process, libArg, (uint)(libBytes.Length + 1));

            Free(process, ciArg1, 16);
            Free(process, ciArg2, 16);
            Free(process, pMetaHost, 4);

            Free(process, rtArg1, (uint)(verBytes.Length + 1));
            Free(process, rtArg2, 16);
            Free(process, pRuntimeInfo, 4);

            Free(process, giArg1, 16);
            Free(process, giArg2, 16);
            Free(process, pClrRuntimeHost, 4);

            Free(process, dllPath, (uint)(dllBytes.Length + 1));
            Free(process, typeName, (uint)(typeBytes.Length + 1));
            Free(process, methodName, (uint)(methodBytes.Length + 1));
            //Free(process, callArg, (uint)(callArgBytes.Length + 1));
            Free(process, result, 4);

            Free(process, funcPtr, funcLen);

            #endregion*/
        }

        static IntPtr Alloc(Process process, uint size)
        {
            if (size == 0)
                return IntPtr.Zero;
            IntPtr ptr = VirtualAllocEx(process.Handle, IntPtr.Zero, size, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ReadWrite);
            if (ptr == IntPtr.Zero)
                throw new Exception("VirtualAllocEx: " + Marshal.GetLastWin32Error());
            return ptr;
        }

        /*public static bool Free(Process process, IntPtr ptr, uint size)
        {
            if (ptr == IntPtr.Zero || size == 0)
                return false;
            if (!VirtualFreeEx(process.Handle, ptr, (uint)size, AllocationType.Decommit))
                throw new Exception("VirtualFreeEx Decommit: " + Marshal.GetLastWin32Error());
            if (!VirtualFreeEx(process.Handle, ptr, 0, AllocationType.Release))
                throw new Exception("VirtualFreeEx Release: " + Marshal.GetLastWin32Error());

            return true;
        }*/

        static uint Write(Process process, IntPtr ptr, byte[] bytes)
        {
            uint tmp;
            if (!WriteProcessMemory(process.Handle, ptr, bytes, (uint)bytes.Length, out tmp))
                throw new Exception("WriteProcessMemory: " + Marshal.GetLastWin32Error());
            return tmp;
        }

        #endregion

        #region Suspend & Resume Process

        [Flags]
        enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        static void SuspendProcess(Process process)
        {
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                SuspendThread(pOpenThread);
                CloseHandle(pOpenThread);
            }
        }

        static void ResumeProcess(Process process)
        {
            foreach (ProcessThread pT in process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    continue;
                }

                while (ResumeThread(pOpenThread) > 1)
                {
                }

                CloseHandle(pOpenThread);
            }
        }

        #endregion
    }
}
