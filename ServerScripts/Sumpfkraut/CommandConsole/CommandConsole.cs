using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole
{
    public class CommandConsole : Runnable
    {

        new public static readonly String _staticName = "CommandConsole (static)";

        protected Process consoleProcess;
        protected bool canExecute = false;



        public CommandConsole ()
            : this(true)
        { }

        public CommandConsole (bool startOnCreate)
            : base(startOnCreate, TimeSpan.Zero, false)
        {
            SetObjName("CommandConsole (default)");
        }



        public override void Init ()
        {
            base.Init();

            
            ProcessStartInfo startInfo = new ProcessStartInfo();

            OperatingSystem os = Environment.OSVersion;
            switch (os.Platform)
            {
                case PlatformID.MacOSX:
                    canExecute = false;
                    MakeLogError(String.Format("Invalid operating system platform {0} detected"
                        + " during Init! The new command console won't be created.", 
                        PlatformID.MacOSX));
                    break;
                case PlatformID.Unix:
                    canExecute = true;
                    startInfo.FileName = "/bin/bash";
                    break;
                case PlatformID.Win32NT:
                    canExecute = true;
                    startInfo.FileName = "cmd.exe";
                    break;
                case PlatformID.Win32S:
                    canExecute = true;
                    startInfo.FileName = "cmd.exe";
                    break;
                case PlatformID.Win32Windows:
                    canExecute = true;
                    startInfo.FileName = "cmd.exe";
                    break;
                case PlatformID.WinCE:
                    canExecute = true;
                    startInfo.FileName = "cmd.exe";
                    break;
                case PlatformID.Xbox:
                    canExecute = false;
                    MakeLogError(String.Format("Invalid operating system platform {0} detected"
                        + " during Init! The new command console won't be created.", 
                        PlatformID.MacOSX));
                    break;
            }

            //startInfo.RedirectStandardInput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = false;

            //startInfo.CreateNoWindow = true;
            //startInfo.UseShellExecute = true;
            //startInfo.RedirectStandardInput = false;

            //startInfo.RedirectStandardInput = false;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = false;

            startInfo.RedirectStandardInput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            //startInfo.CreateNoWindow = true;

            startInfo.WindowStyle = ProcessWindowStyle.Normal;

            MakeLog(String.Format("Creating new process for command prompt on os-platform {0}.",
                os.Platform));
            consoleProcess = new Process();
            consoleProcess.StartInfo = startInfo;
            consoleProcess.Start();

        }

        public override void Run ()
        {
            base.Run();

            if (consoleProcess == null)
            {
                Init();
            }
            if (!canExecute)
            {
                MakeLogWarning("Cannot execute and use command prompt! The command console thread"
                    + " will be suspended.");
                Suspend();
            }

            using (StreamWriter sw = consoleProcess.StandardInput)
            {
                String line;

                while ((line = Console.ReadLine()) != null)
                {
                    Print(">>>" + line);
                }
            }

            //using (StreamWriter sw = consoleProcess.StandardInput)
            //{
            //    using (StreamReader sr = consoleProcess.StandardOutput)
            //    {
            //        String line;

            //        while ((line = Console.ReadLine()) != null)
            //        {
            //            Print(">>>" + line);
            //        }
            //    }
            //}
        }
    }
}
