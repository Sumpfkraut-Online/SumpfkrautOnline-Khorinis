using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using GUC.Client.Hooks;
using GUC.Client.Network;

namespace GUC.Client
{
    static class Injection
    {
        #region Process Suspending
        [Flags]
        public enum ThreadAccess : int
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

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);
        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);
        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);
        #endregion

        public static Int32 Main(String message)
        {
            try
            {
                #region Suspend Gothic
                var process = Process.GetCurrentProcess();
                var threads = process.Threads;

                for (int i = 0; i < threads.Count; i++)
                {
                    if (i == Thread.CurrentThread.ManagedThreadId/*pT.Id == AppDomain.GetCurrentThreadId()*/)
                        continue;

                    IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)threads[i].Id);

                    if (pOpenThread == IntPtr.Zero)
                        continue;

                    SuspendThread(pOpenThread);
                    CloseHandle(pOpenThread);
                }
                #endregion

                Program.ProjectName = message;
                Program.ProjectPath = GetProjectPath(message);
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

                SplashScreen.Create();
                SplashScreen.SetUpHooks();

                WinApi.Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)

                WinApi.Process.SetWindowText("Gothic II - Untold Chapters");

                #region Resume Gothic
                for (int i = 0; i < threads.Count; i++)
                {
                    if (i == Thread.CurrentThread.ManagedThreadId/*pT.Id == AppDomain.GetCurrentThreadId()*/)
                        continue;

                    IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)threads[i].Id);

                    if (pOpenThread == IntPtr.Zero)
                        continue;

                    var suspendCount = 0;
                    do
                    {
                        suspendCount = ResumeThread(pOpenThread);
                    } while (suspendCount > 0);

                    CloseHandle(pOpenThread);
                }
                #endregion

                hParser.AddHooks();
                hGame.AddHooks();

                #region Some more editing
                // Blocking Call Init Scripts!
                WinApi.Process.Write((byte)0xC3, 0x006C1F60);
                // Blocking Call Startup Scripts!
                WinApi.Process.Write((byte)0xC3, 0x006C1C70);

                // disable interface buttons
                WinApi.Process.Write((byte)0xEB, 0x7A55D8);

                // Blocking time!
                WinApi.Process.Write((byte)0xC3, 0x00780D80);
                Logger.Log("Hooking & editing of gothic process completed. (for now...)");
                #endregion

                GameClient.Connect();
            
                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Environment.CurrentDirectory + " " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }
            return 0;
        }

        static string GetProjectPath(string projectName)
        {
            string current = Path.GetFullPath(Environment.CurrentDirectory); // It's Gothic2/System when the process starts, Gothic2/ afterwards.

            if (File.Exists(current + "\\Gothic2.exe"))
            { // Gothic2/System/
                return Path.GetFullPath(current + "\\Multiplayer\\UntoldChapters\\" + projectName + "\\");
            }
            else if (File.Exists(current + "\\System\\Gothic2.exe"))
            { // Gothic2/
                return Path.GetFullPath(current + "\\System\\Multiplayer\\UntoldChapters\\" + projectName + "\\");
            }

            throw new Exception("Gothic 2 not found!");
        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            if (name.ToUpper() == "GUC.RESOURCES")
            {
                //load from resource
                var resxStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                byte[] buffer = new byte[resxStream.Length];
                resxStream.Read(buffer, 0, (int)resxStream.Length);
                return Assembly.Load(buffer);
            }

            return Assembly.LoadFrom(Program.ProjectPath + name + ".dll");
        }
    }
}
