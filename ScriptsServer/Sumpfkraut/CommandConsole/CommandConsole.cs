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
    public class CommandConsole : GUC.Utilities.ExtendedObject
    {

        new public static readonly String _staticName = "CommandConsole (static)";

        //protected Process consoleProcess;
        //protected bool canExecute = false;


        public delegate void ProcessCommand (object sender, String cmd, String[] param, 
            out Dictionary<string, object> returnVal);
        public static readonly Dictionary<String, ProcessCommand> CmdToProcessFunc =
            new Dictionary<String, ProcessCommand>()
            {
                //{ "/G", TestCommands.SetIgTime }, // send global text-message (TO DO)
                //{ "/GETPOS", TestCommands.SetIgTime }, // get pos. of youself or another player/vob? (TO DO)
                //{ "/KILL", TestCommands.SetIgTime }, // kill vob in focus (TO DO)
                //{ "/PLAYERLIST", TestCommands.GetPlayerList }, // get list of players
                //{ "/SETTIME", TestCommands.SetIgTime }, // set ig-time
                //{ "/SETWEATHER", TestCommands.SetIgWeather }, // set ig-weather
                //{ "/SPAWNITEM", TestCommands.SetIgTime }, // spawns item of (type + amount) at position (TO DO)
                //{ "/TPTO", TestCommands.TeleportVobTo }, // teleport vob (to pos.) (TO DO)
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
            String cmd, paramStr = null;
            String[] param;
            ProcessCommand processCmd = null;

            Regex rgx_cmd = new Regex("^\\/\\w+");
            cmd = rgx_cmd.Match(commandText, 0).ToString();

            if (cmd == null)
            {
                return;
            }

            //// delete unnecessary "/" and enforce command format
            //cmd = cmd.Substring(1);
            cmd = cmd.ToUpper();

            if ((cmd.Length + 1) < commandText.Length)
            {
                paramStr = commandText.Substring(cmd.Length + 1);
            }
            else
            {
                paramStr = "";
            }

            if (!CmdToProcessFunc.TryGetValue(cmd, out processCmd))
            {
                return;
            }

            param = paramStr.Split(' ');
            if (param == null)
            {
                param = new String[0];
            }

            Dictionary<string, object> returnVal;
            processCmd(this, cmd, param, out returnVal);
        }

    }
}
