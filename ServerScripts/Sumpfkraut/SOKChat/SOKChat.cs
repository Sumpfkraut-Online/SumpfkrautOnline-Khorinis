using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Types;
using GUC.Server.Log;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Communication;


namespace GUC.Server.Scripts.Sumpfkraut.SOKChat
{
    public delegate void CommandDelegate(Player player, string[] parameters);

    class SOKChat
    {
        private Server.Sumpfkraut.Chat Chat;
        Dictionary<string, Player> AllPlayers = new Dictionary<string, Player>();
        Dictionary<string, Delegate> CommandList = new Dictionary<string, Delegate>();
        Dictionary<string, string> CommandParameterList = new Dictionary<string, string>();

        public SOKChat()
        {
            Chat = new Server.Sumpfkraut.Chat();
            Chat.OnReceiveMessage += ReceiveMessage;
            Player.sOnPlayerSpawns += new Events.PlayerEventHandler(OnPlayerSpawn);
            Player.sOnPlayerDisconnects += new Events.PlayerEventHandler(OnPlayerDisconnect);

            InitCommands();
        }

        private void ReceiveMessage(Player sender, string message)
        {
            ColorRGBA color = new ColorRGBA(255, 0, 0, 255);
            if (message.StartsWith("/"))
                ExecuteCommandByMessage(sender, message);
            else {}
        }

        private void OnPlayerSpawn(Player pl)
        {
            AllPlayers.Add(pl.Name, pl);
        }

        private void OnPlayerDisconnect(Player pl)
        {
            AllPlayers.Remove(pl.Name);
        }

        #region Utilities
        private void SendErrorMessage(Player pl, string txt)
        {
            Chat.SendErrorMessage(pl, txt);
            Chat.SendGlobal(txt);
        }

        private bool IsPlayerAllowedToUseCommand(Player pl)
        {
            return true;
        }
        #endregion

        #region CommandExecution
        private void ExecuteCommandByMessage(Player pl, string message)
        {
            string[] parameters = GetCommandParametersByMessage(message.Substring(1));

            if (!IsPlayerAllowedToUseCommand(pl))
                return;

            if (CommandList.ContainsKey(parameters[0]))
            {
                CommandList[parameters[0]].DynamicInvoke(pl, parameters);
            }
            else
                SendErrorMessage(pl, "Der Befehl \""+parameters[0]+"\" ist nicht vorhanden.");
        }

        private string[] GetCommandParametersByMessage(string message)
        {
            return message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private void AddCommand(string command, CommandDelegate func)
        {
            CommandList.Add(command, func);
        }

        private void AddCommand(string command, string parameterDescription, CommandDelegate func)
        {
            CommandList.Add(command, func);
            CommandParameterList.Add(command, parameterDescription);
        }
        #endregion

        #region Commands
        private void InitCommands()
        {
            #region Help
            CommandDelegate Help = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 1)
                {
                    string str = "Mögliche Befehle: ";
                    foreach (var pair in CommandParameterList)
                    {
                        str += pair.Key + "  ";
                    }
                    Chat.SendGlobal(str);
                }
                else
                {
                    if (CommandParameterList.ContainsKey(parameters[1]))
                        Chat.SendGlobal(CommandParameterList[parameters[1]]);
                }
            };
            #endregion

            AddCommand("help", "/help [<befehl>]", Help);
        }
        #endregion

    }
}