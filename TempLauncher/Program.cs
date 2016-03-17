using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TempLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    if (!File.Exists("Server.txt"))
                        File.WriteAllText("Server.txt", "127.0.0.1:9054");

                    if (!Directory.Exists("UntoldChapters"))
                    {
                        throw new DirectoryNotFoundException("'UntoldChapters' not found!");
                    }
                    if (!File.Exists("..\\Gothic2.exe"))
                    {
                        throw new FileNotFoundException("'Gothic2.exe not found!");
                    }

                    Console.WriteLine("Tippe die Nummer des Projekts ein um es zu starten:");

                    string[] projectDirs = Directory.GetDirectories("UntoldChapters").Where(p =>
                        File.Exists(p + "\\NetInject.dll") && File.Exists(p + "\\WinApi.dll")
                        && File.Exists(p + "\\GUC.dll") && File.Exists(p + "\\Gothic.dll")
                        && File.Exists(p + "\\RakNet.dll") && File.Exists(p + "\\RakNetSwig.dll")
                        && File.Exists(p + "\\Scripts\\ClientScripts.dll")).ToArray();

                    for (int i = 0; i < projectDirs.Length; i++)
                    {
                        projectDirs[i] = projectDirs[i].Substring(15, projectDirs[i].Length - 15);
                        Console.WriteLine(i + ": " + projectDirs[i]);
                    }

                    int num;
                    while (true)
                    {
                        Console.WriteLine();

                        string str = Console.ReadLine();
                        if (int.TryParse(str, out num) && num >= 0 && num < projectDirs.Length)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Projektnummer existiert nicht.");
                        }
                    }

                    string projectName = projectDirs[num];
                    string address = File.ReadAllText("Server.txt");

                    ProcessStartInfo psi;
                    bool zSpy = false;
                    if (File.Exists("..\\..\\_work\\tools\\zSpy\\zSpy.exe"))
                    {
                        psi = new ProcessStartInfo();
                        string logPath = Path.GetFullPath("UntoldChapters\\" + projectName + "\\Log");
                        if (!Directory.Exists(logPath))
                            Directory.CreateDirectory(logPath);

                        psi.WorkingDirectory = logPath;

                        psi.FileName = Path.GetFullPath("..\\..\\_work\\tools\\zSpy\\zSpy.exe");
                        Process.Start(psi);
                        zSpy = true;

                        System.Threading.Thread.Sleep(1000); // wait for zSpy to start
                    }

                    //Gothic starten
                    psi = new ProcessStartInfo();
                    psi.UseShellExecute = false;
                    psi.WorkingDirectory = Path.GetFullPath("..");
                    psi.EnvironmentVariables.Add("GUCProject", projectName);
                    psi.EnvironmentVariables.Add("ServerAddress", address);

                    if (zSpy)
                    {
                        psi.Arguments = "-zlog:5,s";
                    }

                    psi.FileName = Path.GetFullPath("..\\Gothic2.exe");
                    Process process = Process.Start(psi);


                    //dll injection
                    if (LoadLibary(process, Path.GetFullPath("UntoldChapters\\" + projectName + "\\NetInject.dll")) == IntPtr.Zero)
                    {
                        throw new Exception(Marshal.GetLastWin32Error().ToString());
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Source);
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }

        [Flags]
        public enum AllocationType
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
        public enum MemoryProtection : uint
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
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UInt32 nSize, out UInt32 lpNumberOfBytesWritten);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out uint lpThreadId);

        public static IntPtr LoadLibary(Process process, String dll)
        {
            if (process == null || dll == null || String.IsNullOrWhiteSpace(dll))
                return IntPtr.Zero;

            byte[] dllb = Encoding.ASCII.GetBytes(dll);
            if (dllb == null || dllb.Length == 0)
                return IntPtr.Zero;

            //Alloc
            uint len = (uint)dllb.Length + 1;
            IntPtr dllbPtr = VirtualAllocEx(process.Handle, IntPtr.Zero, len, AllocationType.Reserve | AllocationType.Commit, MemoryProtection.ReadWrite);
            if (dllbPtr == IntPtr.Zero)
            {
                throw new Exception(Marshal.GetLastWin32Error().ToString());
            }

            //Write dll name
            uint tmp = 0;
            if (!WriteProcessMemory(process.Handle, dllbPtr, dllb, (uint)dllb.Length, out tmp))
            {
                throw new Exception(Marshal.GetLastWin32Error().ToString());
            }

            IntPtr loadlib = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");

            return CreateRemoteThread(process.Handle, IntPtr.Zero, 0, loadlib, dllbPtr, 0, out tmp);
        }
    }
}
