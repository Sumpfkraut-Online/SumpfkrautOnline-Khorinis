using GUC.Server.Scripts.Sumpfkraut.Utilities.Threading;
using GUC.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole
{
    public class CommandConsole : ScriptObject
    {

        new public static readonly String _staticName = "CommandConsole (static)";

        //protected Process consoleProcess;
        //protected bool canExecute = false;


        public delegate void ProcessCommand (CommandConsole console, String cmd, String param);
        public static readonly Dictionary<String, ProcessCommand> CmdToProcessFunc =
            new Dictionary<String, ProcessCommand>()
            {
                { "/SETTIME", TestCommands.SetIgTime }, 
            };



        public CommandConsole ()
        {
            SetObjName("CommandConsole (default)");
            SubscribeOnConsole();
        }



        public void SubscribeOnConsole ()
        {
            Log.Logger.OnCommand += HandleCommand;
        }

        public void UnsuscribeOnConsole ()
        {
            Log.Logger.OnCommand -= HandleCommand;
        }



        public void HandleCommand (String commandText)
        {
            String cmd, param = null;
            ProcessCommand processCmd = null;

            Log.Logger.print("> " + commandText);

            Regex rgx_cmd = new Regex("^\\/\\w+");
            cmd = rgx_cmd.Match(commandText, 0).ToString();

            if (cmd == null)
            {
                return;
            }

            cmd = cmd.ToUpper();

            if ((cmd.Length + 1) < commandText.Length)
            {
                param = commandText.Substring(cmd.Length + 1);
            }
            else
            {
                param = "";
            }

            if (CmdToProcessFunc.TryGetValue(cmd, out processCmd))
            {
                processCmd(this, cmd, param);
            }
        }



        //public override void Init ()
        //{
        //    base.Init();

            
        //    ProcessStartInfo startInfo = new ProcessStartInfo();

        //    OperatingSystem os = Environment.OSVersion;
        //    switch (os.Platform)
        //    {
        //        case PlatformID.MacOSX:
        //            canExecute = false;
        //            MakeLogError(String.Format("Invalid operating system platform {0} detected"
        //                + " during Init! The new command console won't be created.", 
        //                PlatformID.MacOSX));
        //            break;
        //        case PlatformID.Unix:
        //            canExecute = true;
        //            startInfo.FileName = "/bin/bash";
        //            break;
        //        case PlatformID.Win32NT:
        //            canExecute = true;
        //            startInfo.FileName = "cmd.exe";
        //            break;
        //        case PlatformID.Win32S:
        //            canExecute = true;
        //            startInfo.FileName = "cmd.exe";
        //            break;
        //        case PlatformID.Win32Windows:
        //            canExecute = true;
        //            startInfo.FileName = "cmd.exe";
        //            break;
        //        case PlatformID.WinCE:
        //            canExecute = true;
        //            startInfo.FileName = "cmd.exe";
        //            break;
        //        case PlatformID.Xbox:
        //            canExecute = false;
        //            MakeLogError(String.Format("Invalid operating system platform {0} detected"
        //                + " during Init! The new command console won't be created.", 
        //                PlatformID.MacOSX));
        //            break;
        //    }

        //    //startInfo.RedirectStandardInput = true;
        //    //startInfo.RedirectStandardError = true;
        //    //startInfo.RedirectStandardOutput = true;
        //    //startInfo.UseShellExecute = false;
        //    //startInfo.CreateNoWindow = false;

        //    //startInfo.CreateNoWindow = true;
        //    //startInfo.UseShellExecute = true;
        //    //startInfo.RedirectStandardInput = false;

        //    //startInfo.RedirectStandardInput = false;
        //    //startInfo.RedirectStandardError = true;
        //    //startInfo.RedirectStandardOutput = true;
        //    //startInfo.UseShellExecute = false;

        //    startInfo.RedirectStandardInput = true;
        //    //startInfo.RedirectStandardError = true;
        //    //startInfo.RedirectStandardOutput = true;
        //    startInfo.UseShellExecute = false;
        //    //startInfo.CreateNoWindow = true;

        //    startInfo.WindowStyle = ProcessWindowStyle.Normal;

        //    MakeLog(String.Format("Creating new process for command prompt on os-platform {0}.",
        //        os.Platform));
        //    consoleProcess = new Process();
        //    consoleProcess.StartInfo = startInfo;
        //    consoleProcess.Start();

        //}

        //public override void Run ()
        //{
        //    base.Run();

        //    if (consoleProcess == null)
        //    {
        //        Init();
        //    }
        //    if (!canExecute)
        //    {
        //        MakeLogWarning("Cannot execute and use command prompt! The command console thread"
        //            + " will be suspended.");
        //        Suspend();
        //    }

        //    using (StreamWriter sw = consoleProcess.StandardInput)
        //    {
        //        String line;

        //        while ((line = Console.ReadLine()) != null)
        //        {
        //            Print(">>>" + line);
        //        }
        //    }

        //    //using (StreamWriter sw = consoleProcess.StandardInput)
        //    //{
        //    //    using (StreamReader sr = consoleProcess.StandardOutput)
        //    //    {
        //    //        String line;

        //    //        while ((line = Console.ReadLine()) != null)
        //    //        {
        //    //            Print(">>>" + line);
        //    //        }
        //    //    }
        //    //}
        //}

    }
}
