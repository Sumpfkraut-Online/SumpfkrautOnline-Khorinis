using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Server.Network.Messages;
using GUC.Enumeration;
using GUC.Types;

//TODO: messagelistener
//MessageListener.Add((byte)NetworkID.ChatMessage, ChatMessage.Read);

namespace GUC.Server.Interface
{
    public class Chat
    {
        public delegate void CommandDelegate(NPC player, string[] parameters);
        Dictionary<uint, NPC> PlayerList = sWorld.PlayerDict;
        Dictionary<string, Delegate> Commands = new Dictionary<string, Delegate>();
        Dictionary<string, string> CommandDescription = new Dictionary<string, string>();

        private static Chat ChatCtrl;
        public static Chat GetChat()
        {
            if (ChatCtrl == null)
            {
                ChatCtrl = new Chat();
            }
            return ChatCtrl;
        }

        public Chat()
        {
            InitCommands();
        }

        public void MessageReceived(string message, NPC sender)
        {
            foreach (KeyValuePair<string, Delegate> pair in Commands)
            {
                if (message.StartsWith(pair.Key))
                {
                    // calls the command function with a string[] filled with the parameters using zerobased index
                    Commands[pair.Key].DynamicInvoke(
                            sender,
                            message.Substring(pair.Key.Length).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                        );
                    return;
                }
            }
            ProcessMessage(message.Split(new char[] { ' ' }), sender.CustomName + ": ", sender, ChatTextType.Say);
        }

        public void ProcessMessage(string[] message, string addition, NPC sender, ChatTextType type)
        {
            float minDistance, maxDistance;
            GetTalkingDistance(type, out minDistance, out maxDistance);

            foreach (KeyValuePair<uint, NPC> pair in PlayerList)
            {
                if (Math.Abs((pair.Value.Position - sender.Position).GetLength()) < maxDistance || maxDistance == 0)
                {
                    ChatMessage.SendMessage(addition + string.Join(" ", message), pair.Value, type);
                }
            }
        }

        public void GetTalkingDistance(ChatTextType type, out float minDistance, out float maxDistance)
        {
            minDistance = 0; maxDistance = 0;
            switch (type)
            {
                case ChatTextType.Whisper:
                    minDistance = 200;
                    maxDistance = 300;
                    break;
                case ChatTextType.Say:
                    minDistance = 1000;
                    maxDistance = 1200;
                    break;
                case ChatTextType.Shout:
                    minDistance = 3000;
                    maxDistance = 3500;
                    break;
                case ChatTextType.OOC:
                    minDistance = 3000; // change
                    maxDistance = 3500;
                    break;
                case ChatTextType.Ambient:
                    minDistance = 3500; // change
                    maxDistance = 3500;
                    break;
                default:
                    minDistance = 0;
                    maxDistance = 0;
                    break;
            }
        }

        public void Error(NPC to, string msg)
        {
            ChatMessage.SendMessage(msg, to, ChatTextType._Error);
        }

        public void Hint(NPC to, string msg)
        {
            ChatMessage.SendMessage(msg, to, ChatTextType._Hint);
        }

        public void AddCommand(string command, string descr, CommandDelegate del)
        {
            Commands.Add(command, del);
            CommandDescription.Add(command, descr);
        }

        public NPC GetPlayer(string name)
        {
            return PlayerList.Values.FirstOrDefault(p => p.CustomName == name);
        }

        public void InitCommands()
        {

            #region Commands
            CommandDelegate whisper = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + " flüstert: ", sender, ChatTextType.Whisper);
            };

            CommandDelegate shout = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + " ruft: ", sender, ChatTextType.Shout);
            };

            CommandDelegate ambient = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + " ", sender, ChatTextType.Ambient);
            };

            CommandDelegate ooc = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + ": ", sender, ChatTextType.OOC);
            };

            CommandDelegate oocGlobal = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + ": ", sender, ChatTextType.OOCGlobal);
            };

            CommandDelegate oocEvent = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + " ", sender, ChatTextType.OOCEvent);
            };

            CommandDelegate rpGlobal = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + ": ", sender, ChatTextType.RPGlobal);
            };

            CommandDelegate rpEvent = delegate (NPC sender, string[] parameters)
            {
                ProcessMessage(parameters, sender.CustomName + " ", sender, ChatTextType.RPEvent);
            };

            CommandDelegate tp = delegate (NPC sender, string[] parameters)
            {
                // /tp zuSpieler
                // /tp Spieler 12 14 0
                // /tp Spieler zuSpieler
                NPC npc = GetPlayer(parameters[0]);
                if (npc == null)
                {
                    Error(sender, "<player> Spieler \"" + parameters[0] + "\" nicht gefunden.");
                    return;
                }

                if (parameters.Length == 1)
                {
                    sender.Position = npc.Position;
                }
                else if (parameters.Length == 2)
                {
                    NPC target = GetPlayer(parameters[1]);
                    if (target != null)
                        npc.Position = target.Position;
                    else
                        Error(sender, "<target> Spieler \"" + parameters[1] + "\" nicht gefunden.");
                }
                else if (parameters.Length == 4)
                {

                    float X, Y, Z;
                    if (!float.TryParse(parameters[1], out X) || !float.TryParse(parameters[2], out Y) || !float.TryParse(parameters[3], out Z))
                    {
                        Error(sender, "Die X,Y,Z-Koordinaten wurden nicht richtig angegeben.");
                        return;
                    }
                    npc.Position = new Vec3f(X + 1, Z, Y + 1);
                }
            };

            CommandDelegate getPlayerPosition = delegate (NPC sender, string[] parameters)
            {
                if (parameters.Length == 0)
                {
                    Hint(sender, "Your Position(X=" + sender.Position.X + " Y=" + sender.Position.Y + " Z=" + sender.Position.Z + ")");
                    return;
                }
                NPC npc = GetPlayer(parameters[0]);
                if (npc == null)
                {
                    Error(sender, "Spieler \"" + parameters[0] + "\" nicht gefunden.");
                    return;
                }
                Hint(sender, "Found " + parameters[0] + " at (X=" + npc.Position.X + " Y=" + npc.Position.Y + " Z=" + npc.Position.Z + ")");
            };

            CommandDelegate revive = delegate (NPC sender, string[] parameters)
            {
                if (parameters.Length == 0)
                {
                    sender.AttrHealth = sender.AttrHealthMax;
                    return;
                }
                NPC npc = GetPlayer(parameters[0]);
                if (npc == null)
                {
                    Error(sender, "Spieler \"" + parameters[0] + "\" nicht gefunden.");
                    return;
                }
                npc.AttrHealth = npc.AttrHealthMax;
            };

            #endregion

            AddCommand("/w", "/w message", whisper);
            AddCommand("/s", "/s message", shout);
            AddCommand("/me", "/me message", ambient);
            AddCommand("/ooc", "/ooc message", ooc);
            AddCommand("/global", "/global message", rpGlobal);
            AddCommand("/event", "/event message", rpEvent);
            AddCommand("/oocG", "/oocG message", oocGlobal);
            AddCommand("/oocE", "/oocE message", oocEvent);
            AddCommand("/tp", "/tp <player> [<target>] [<X> <Z> <Y>]", tp);
            AddCommand("/getpos", "/getpos [<player>]", getPlayerPosition);
            AddCommand("/revive", "/revive [<player>]", revive);
        }
    }
}
