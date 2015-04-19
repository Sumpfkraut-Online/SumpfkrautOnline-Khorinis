using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Types;
using GUC.Server.Log;
using GUC.Enumeration;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.Communication;

using GUC.Server.Scripts.AI;
using GUC.Server.Scripts.AI.Waypoints;


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
            Logger.log(Logger.LogLevel.INFO, "#################### Initialise SOKChat ###################");

            Chat = new Server.Sumpfkraut.Chat();
            Chat.OnReceiveMessage += ReceiveMessage;
            Player.sOnPlayerSpawns += new Events.PlayerEventHandler(OnPlayerSpawn);
            Player.sOnPlayerDisconnects += new Events.PlayerEventHandler(OnPlayerDisconnect);

            InitCommands();
        }

        private void ReceiveMessage(Player sender, string message)
        {
            if (message.StartsWith("/"))
                ExecuteCommandByMessage(sender, message);
            else if(message.StartsWith("@"))
                CommandList["@"].DynamicInvoke(sender, GetCommandParametersByMessage(message.Substring(1)));
            else
                SendTextBasedOnChatType(sender, message, ChatTextType.Say);
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
        private void GetTalkingDistances(ChatTextType ChatType, out float maxDistGood, out float maxDistMiddle, out float maxDistBad)
        {
            maxDistGood = 0;
            maxDistBad = 0;
            maxDistMiddle = 0;
            if (ChatType == ChatTextType.Say) // Reden
            {
                maxDistGood = 900;
                maxDistMiddle = 1100;
                maxDistBad = 1500;
            }
            else if (ChatType == ChatTextType.Shout) // Rufen
            {
                maxDistGood = 2000;
                maxDistMiddle = 2500;
                maxDistBad = 3000;
            }
            else if (ChatType == ChatTextType.Whisper) // Flüstern
            {
                maxDistGood = 200;
                maxDistMiddle = 250;
                maxDistBad = 300;
            }
            return;
        }

        private void SendTextBasedOnChatType(Player sender, string message, ChatTextType ChatType)
        {
            //Talking Distance Qualities + Maximum High
            float maxDistGood, maxDistMiddle, maxDistBad;
            GetTalkingDistances(ChatType, out maxDistGood, out maxDistMiddle, out maxDistBad);

            // The real distances
            float distX, distY, distZ;

            Vec3f senderPosition = sender.Position;
            Vec3f otherPosition;
            string newMessage = message;
            foreach (var pair in AllPlayers)
            {
                if (pair.Value != sender)
                {
                    otherPosition = pair.Value.Position;
                    distX = Math.Abs(senderPosition.X - otherPosition.X);
                    distY = Math.Abs(senderPosition.Y - otherPosition.Y);
                    distZ = Math.Abs(senderPosition.Z - otherPosition.Z);

                    if ((distX * distX + distY * distY + distZ * distZ) < maxDistGood * maxDistGood)
                        newMessage = " (GoodQ) " + ExchangeTextQuality(message, 0);
                    else if ((distX * distX + distY * distY + distZ * distZ) < maxDistMiddle * maxDistMiddle)
                        // Middle Talking Quality
                        newMessage = " (MiddleQ) " + ExchangeTextQuality(message, 1);
                    else if ((distX * distX + distY * distY + distZ * distZ) < maxDistBad * maxDistBad)
                        // Bad Talking Qualitiy -> percentage
                        newMessage = " (BadQ) " + ExchangeTextQuality(message, 2);
                    else
                        newMessage = "";
                }

                if (ChatType == ChatTextType.Say)
                    Chat.SendSay(sender, pair.Value, newMessage);
                else if (ChatType == ChatTextType.Shout)
                    Chat.SendShout(sender, pair.Value, newMessage);
                else if (ChatType == ChatTextType.Whisper)
                    Chat.SendWhisper(sender, pair.Value, newMessage);
            }
            return;
        }

        private string ExchangeTextQuality(string message, int qualityType)
        {
            // Hier Nachricht entsprechend nach QualityType verändern.
            return message;
        }

        private void SendErrorMessage(Player pl, string txt)
        {
            Chat.SendErrorMessage(pl, txt);
            Chat.SendGlobal(txt);
        }

        private void SendHintMessage(Player pl, string txt)
        {
            Chat.SendPM(pl, pl, txt);
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
                        str += pair.Key + " ";
                    }
                    SendHintMessage(player,str);
                }
                else
                {
                    if (CommandParameterList.ContainsKey(parameters[1]))
                        SendHintMessage(player, CommandParameterList[parameters[1]]);
                }
            };
            #endregion

            #region Whisper
            CommandDelegate Whisper = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 1)
                    return;

                string message = "";
                for (int i = 1; i < parameters.Length; i++)
                    message += parameters[i] + " ";
                SendTextBasedOnChatType(player, message, ChatTextType.Whisper);
            };
            #endregion

            #region Shout
            CommandDelegate Shout = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 1)
                    return;

                string message = "";
                for(int i = 1; i < parameters.Length; i ++)
                    message += parameters[i] + " ";
                SendTextBasedOnChatType(player, message, ChatTextType.Shout);
            };
            #endregion

            #region PlayDialogueAnimation
            CommandDelegate PlayDialogueAnimation = delegate(Player player, string[] parameters)
            {
                player.startDialogAnimation();
            };
            #endregion

            #region Revive
            CommandDelegate Revive = delegate(Player player, string[] parameters)
            {
                if (!IsPlayerAllowedToUseCommand(player))
                    return;

                if (parameters.Length == 2)
                    if (AllPlayers.ContainsKey(parameters[1]))
                        AllPlayers[parameters[1]].revive();
                    else
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                if (parameters.Length == 1)
                    player.revive();
            };
            #endregion

            #region toWaypoint
            CommandDelegate toWaypoint = delegate(Player player, string[] parameters)
            {
                if (!IsPlayerAllowedToUseCommand(player))
                    return;

                if (parameters.Length == 2)
                {
                    FreeOrWayPoint wp = AISystem.getWaypoint(player.Map, parameters[1]);

                    if (wp != null)
                        player.setPosition(wp.Position);
                    else
                        SendErrorMessage(player, "Wegpunkt \"" + parameters[1] + "\" ist nicht verfügbar.");
                }
                else if (parameters.Length == 3)
                {
                    Player victim;
                    if (AllPlayers.ContainsKey(parameters[1]))
                        victim = AllPlayers[parameters[1]];
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }

                    FreeOrWayPoint wp = AISystem.getWaypoint(player.Map, parameters[2]);

                    if (wp != null)
                        victim.setPosition(wp.Position);
                    else
                        SendErrorMessage(player, "Wegpunkt \""+parameters[2]+"\" ist nicht verfügbar.");

                }
                else
                    SendHintMessage(player, "Verwendung: " + CommandParameterList[parameters[0]]);
            };
            #endregion

            #region Teleport
            CommandDelegate Teleport = delegate(Player player, string[] parameters)
            {
                if (!IsPlayerAllowedToUseCommand(player))
                    return;

                Player victim, target;

                if (parameters.Length == 2)
                {
                    if (AllPlayers.ContainsKey(parameters[1]))
                        target = AllPlayers[parameters[1]];
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }
                    player.setPosition(target.Position);
                    return;
                }

                if (parameters.Length == 3)
                {
                    if (AllPlayers.ContainsKey(parameters[1]) && AllPlayers.ContainsKey(parameters[2]))
                    {
                        victim = AllPlayers[parameters[1]];
                        target = AllPlayers[parameters[2]];
                    }
                    else if (!AllPlayers.ContainsKey(parameters[1]))
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[2] + " wurde nicht gefunden.");
                        return;
                    }

                    victim.setPosition(target.Position);
                    return;
                }

                if (parameters.Length == 5)
                {
                    if (AllPlayers.ContainsKey(parameters[1]))
                        victim = AllPlayers[parameters[1]];
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }

                    float X, Y, Z;
                    if (!float.TryParse(parameters[2], out X) || !float.TryParse(parameters[3], out Y) || !float.TryParse(parameters[4], out Z))
                    {
                        SendErrorMessage(player, "Die X,Y,Z Koordinaten wurden nicht richtig angegeben.");
                        return;
                    }
                    victim.setPosition(new Vec3f(X, Y, Z));
                    return;
                }

                SendHintMessage(player, "Verwendung: " + CommandParameterList[parameters[0]]);

            };
            #endregion

            #region playAnimation
            CommandDelegate playAnimation = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 2)
                {
                    player.playAnimation(parameters[1]);
                }
                else
                    SendHintMessage(player, "Verwendung: " + CommandParameterList[parameters[0]]);      
            };
            #endregion

            #region Sprint
            CommandDelegate Sprint = delegate(Player player, string[] parameters)
            {
                if (!IsPlayerAllowedToUseCommand(player))
                    return;
                player.ApplyOverlay("HUMANS_SPRINT.MDS");
            };
            #endregion

            #region PersonalMessage
            CommandDelegate PersonalMessage = delegate(Player player, string[] parameters)
            {
                if(parameters.Length >= 2)
                    if (AllPlayers.ContainsKey(parameters[0]))
                    {
                        string message = "";
                        for (int i = 1; i < parameters.Length; i++)
                            message += parameters[i] + " ";

                        Chat.SendPM(player, AllPlayers[parameters[0]], message);
                        return;
                    }
                    else
                        SendErrorMessage(player, "Spieler "+parameters[0]+" wurde nicht gefunden.");
                SendHintMessage(player, "Verwendung: " + CommandParameterList["@"]);
            };
            #endregion

            AddCommand("help", "/help <befehl>", Help);
            AddCommand("whisper", "/whisper <text>", Whisper);
            AddCommand("shout", "/shout <text>", Shout);
            AddCommand("pa", "/pa <animationName>", playAnimation);
            AddCommand("StartDialogueAnimation", PlayDialogueAnimation);
            AddCommand("@","@<SpielerName> <text>", PersonalMessage);

            // Admin Commands
            AddCommand("revive", "/revive <player>", Revive);
            AddCommand("tp", "/tp <Spieler/Ziel> <Ziel/X> <Y> <Z>", Teleport);
            AddCommand("wp", "/wp <Spieler/Ziel> <Ziel>", toWaypoint);
            AddCommand("sprint", Sprint);

        }
        #endregion

    }
}