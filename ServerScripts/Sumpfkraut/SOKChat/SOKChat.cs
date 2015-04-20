﻿using System;
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
        Dictionary<string, DateTime[]> MutedPlayers = new Dictionary<string, DateTime[]>();

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
                SendTextBasedOnChatType(sender, GetStringArray(message), ChatTextType.Say);
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

        private void SendTextBasedOnChatType(Player sender, string[] message, ChatTextType ChatType)
        {

            if (IsMuted(sender.Name))
            {
                SendHintMessage(sender, "Der Chat ist derzeit für dich nicht verfügbar.");
                return;
            }
            //Talking Distance Qualities + Maximum High
            float maxDistGood, maxDistMiddle, maxDistBad;
            GetTalkingDistances(ChatType, out maxDistGood, out maxDistMiddle, out maxDistBad);

            // The real distances
            //float distX, distY, distZ;

            Vec3f senderPosition = sender.Position;
            Vec3f otherPosition;
            string[] newMessage = message;
            foreach (var pair in AllPlayers)
            {
                if (pair.Value != sender)
                {
                    otherPosition = pair.Value.Position;
                    //distX = Math.Abs(senderPosition.X - otherPosition.X);
                    //distY = Math.Abs(senderPosition.Y - otherPosition.Y);
                    //distZ = Math.Abs(senderPosition.Z - otherPosition.Z); (distX * distX + distY * distY + distZ * distZ)
                    float distance = (senderPosition - otherPosition).Length;

                    if ( distance < maxDistGood)
                        newMessage = ExchangeTextQuality(message, 0);
                    /*else if ((distX * distX + distY * distY + distZ * distZ) < maxDistMiddle * maxDistMiddle)
                        // Middle Talking Quality
                        newMessage = " (MiddleQ) " + ExchangeTextQuality(message, 1);*/
                    else if (distance < maxDistBad)
                        // Bad Talking Quality -> percentage
                        newMessage = ExchangeTextQuality(message, distance / (maxDistBad / 100));
                    else
                        newMessage = new string[] { "" };
                }

                string strMessage = "";
                for (int i = 0; i < newMessage.Length; i++)
                    strMessage += newMessage[i] + " ";

                strMessage = strMessage.Trim();
                
                if (ChatType == ChatTextType.Say)
                    Chat.SendSay(sender, pair.Value, strMessage);
                else if (ChatType == ChatTextType.Shout)
                    Chat.SendShout(sender, pair.Value, strMessage);
                else if (ChatType == ChatTextType.Whisper)
                    Chat.SendWhisper(sender, pair.Value, strMessage);
                else if (ChatType == ChatTextType.OOC)
                {
                    if (!newMessage.Equals("")) // Sendet OOC nur an erreichbare Spieler
                        Chat.SendOOC(sender, pair.Value, strMessage);
                }
                else if (ChatType == ChatTextType.Ambient)
                {
                    if (!newMessage.Equals(""))
                        Chat.SendAmbient(sender, pair.Value, strMessage);
                }
            }
            return;
        }

        private string[] ExchangeTextQuality(string[] message, float qualityPercentage)
        {
            Random rnd = new Random();
            float number;
            int v;

            for (int i = 0; i < message.Length; i++ )
            {
                number = rnd.Next(1, 100);
                if (number < qualityPercentage && !message[i].Equals(""))
                {
                    if (i > 0)
                    {
                        v = 0;
                        while (v < i)
                        {
                            v++;
                            if (!message[i - v].Equals(""))
                                break;
                        }

                        if (!message[i - v].Equals("...") && !message[i - v].Equals(""))
                            message[i] = "...";
                        else
                            message[i] = "";
                    }
                    else
                        message[i] = "...";
                }
            }
            return message;
        }

        private void SendErrorMessage(Player pl, string txt)
        {
            Chat.SendErrorMessage(pl, txt); // test
        }

        private void SendHintMessage(Player pl, string txt)
        {
            Chat.SendHintMessage(pl, txt);
        }

        private bool IsPlayerAllowedToUseCommand(Player pl)
        {
            return true;
        }

        private void ControlMute(string plName, bool mute, int minutes)
        {
            if (mute)
            {
                if (MutedPlayers.ContainsKey(plName))
                {
                    MutedPlayers[plName][0] = DateTime.Now;
                    MutedPlayers[plName][1] = MutedPlayers[plName][0].AddMinutes(minutes);
                }
                else
                {
                    MutedPlayers.Add(plName,new DateTime[] {DateTime.Now,DateTime.Now.AddMinutes(minutes)});
                }
            }
            else
            {
                if(MutedPlayers.ContainsKey(plName))
                {
                    MutedPlayers.Remove(plName);
                }
            }
        }

        private bool IsMuted(string plName)
        {
            if (MutedPlayers.ContainsKey(plName))
            {
                MutedPlayers[plName][0] = DateTime.Now;
                TimeSpan difference = MutedPlayers[plName][1] - MutedPlayers[plName][0];
                if (difference.TotalSeconds > 0)
                {
                    return true;
                }
                else
                {
                    MutedPlayers.Remove(plName);
                    return false;
                }
            }
            else
                return false;
        }
        #endregion

        #region CommandExecution
        private void ExecuteCommandByMessage(Player pl, string message)
        {
            string[] parameters = GetCommandParametersByMessage(message.Substring(1));

            if (CommandList.ContainsKey(parameters[0]))
            {
                CommandList[parameters[0]].DynamicInvoke(pl, parameters);
            }
            else
                SendErrorMessage(pl, "Der Befehl \""+parameters[0]+"\" ist nicht vorhanden.");
        }

        private string[] GetCommandParametersByMessage(string message)
        {
            foreach(var command in CommandList)
            {
                if (message.StartsWith(command.Key))
                {
                    // Wenn Befehle ohne Leerzeichen gesendet werden: /oocHallo dann => /ooc Hallo
                    string newMessage = message.Substring(command.Key.Length);
                    message = command.Key + " " + newMessage;
                    break;
                }
            }
            return message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private string[] GetStringArray(string message)
        {
            return message.Split(new char[] { ' ' });
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
                parameters[0] = ""; // where cmd is
                SendTextBasedOnChatType(player, parameters, ChatTextType.Whisper);
            };
            #endregion

            #region Shout
            CommandDelegate Shout = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 1)
                    return;
                parameters[0] = ""; // where cmd is
                SendTextBasedOnChatType(player, parameters, ChatTextType.Shout);
            };
            #endregion

            #region OOC
            CommandDelegate OOC = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 1)
                    return;

                string message = "";
                for (int i = 1; i < parameters.Length; i++)
                    message += parameters[i] + " ";

                if(message.StartsWith("/"))
                    ExecuteCommandByMessage(player, message);
                else if (message.StartsWith("@"))
                    CommandList["@"].DynamicInvoke(player, GetCommandParametersByMessage(message.Substring(1)));
                else
                    SendTextBasedOnChatType(player, parameters, ChatTextType.OOC);
            };
            #endregion

            #region StartDialogueAnimation
            CommandDelegate StartDialogueAnimation = delegate(Player player, string[] parameters)
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
                    {
                        SendErrorMessage(player, "Spieler " + parameters[0] + " wurde nicht gefunden.");
                        return;
                    }
                SendHintMessage(player, "Verwendung: " + CommandParameterList["@"]);
            };
            #endregion

            #region Me
            CommandDelegate Me = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 1)
                    return;
                SendTextBasedOnChatType(player, parameters, ChatTextType.Ambient);   
            };
            #endregion

            #region Mute
            CommandDelegate Mute = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 2)
                {
                    if (AllPlayers.ContainsKey(parameters[1]))
                    {
                        ControlMute(parameters[1], true, 60*24);
                        return;
                    }
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }
                }
                if (parameters.Length == 3)
                {
                    if (AllPlayers.ContainsKey(parameters[1]))
                    {
                        int minutes = 0;
                        if(!Int32.TryParse(parameters[2], out minutes))
                        {
                            SendErrorMessage(player, "\""+parameters[2]+"\" ist keine gültige Angabe für Minuten.");
                            return;
                        }
                        ControlMute(parameters[1], true, minutes);
                        return;
                    }
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }
                }

                SendHintMessage(player, "Verwendung: " + CommandParameterList[parameters[0]]);
                return;       
            };
            #endregion

            #region Unmute
            CommandDelegate Unmute = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 2)
                {
                    if (AllPlayers.ContainsKey(parameters[1]))
                    {
                        ControlMute(parameters[1], false, 0);
                    }
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }
                }
                SendHintMessage(player, "Verwendung: " + CommandParameterList[parameters[0]]);
                return;
            };
            #endregion

            #region IsMuted
            CommandDelegate IsMuted = delegate(Player player, string[] parameters)
            {
                if (parameters.Length == 2)
                {
                    if (AllPlayers.ContainsKey(parameters[1]))
                    {
                        if (MutedPlayers.ContainsKey(parameters[1]))
                        {
                            MutedPlayers[parameters[1]][0] = DateTime.Now;
                            TimeSpan diff = MutedPlayers[parameters[1]][1] - MutedPlayers[parameters[1]][0];
                            SendHintMessage(player, "Der Spieler " + parameters[1] + " ist noch für " + Math.Floor(diff.TotalMinutes) + " Minuten bzw. " + Math.Floor(diff.TotalSeconds) + " Sekunden gestummt");
                            return;
                        }
                        else
                        {
                            SendHintMessage(player, "Der Spieler \"" + parameters[1] + "\" ist nicht gestummt.");
                            return;
                        }
                    }
                    else
                    {
                        SendErrorMessage(player, "Spieler " + parameters[1] + " wurde nicht gefunden.");
                        return;
                    }
                }
                SendHintMessage(player, "Verwendung: " + CommandParameterList[parameters[0]]);
                return;
            };
            #endregion

            AddCommand("help", "/help <befehl>", Help);
            AddCommand("whisper", "/whisper <text>", Whisper);
            AddCommand("shout", "/shout <text>", Shout);
            AddCommand("pa", "/pa <animationName>", playAnimation);
            AddCommand("StartDialogueAnimation", StartDialogueAnimation);
            AddCommand("@","@<SpielerName> <text>", PersonalMessage);
            AddCommand("ooc", "/ooc <text>", OOC);
            AddCommand("me", "/me <text>", Me);

            // Admin Commands
            AddCommand("revive", "/revive <player>", Revive);
            AddCommand("tp", "/tp <Spieler/Ziel> <Ziel/X> <Y> <Z>", Teleport);
            AddCommand("wp", "/wp <Spieler/Ziel> <Ziel>", toWaypoint);
            AddCommand("sprint", Sprint);
            AddCommand("mute", "/mute <Spieler> <Minuten>", Mute);
            AddCommand("unmute", "/unmute <Spieler>", Unmute);
            AddCommand("ismuted", "/ismute <Spieler>", IsMuted);

        }
        #endregion

    }
}