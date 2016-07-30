using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Server.Network.Messages;
using GUC.Enumeration;
using GUC.Types;
using GUC.WorldObjects;


//das obere irgendwo in die sumpfkrautskripts einfügen wahrscheinlich bei MenuMsgID:
/*
public enum SumpfkrautIDs
    {
        Msg1,
        Msg2,
        Msg3,
        MaxMessages
    }

    public enum MenuMsgID
    {
        ClientInfoGroup = SumpfkrautIDs.MaxMessages,
        ClientConnect,
        ClientDisconnect,
        
        ClientTeam,
        ClientClass,
        ClientName,
        ClientNPC,

        PhaseMsg,
        WinMsg,

        OpenScoreboard,
        CloseScoreboard,

        AllChat,
        TeamChat
    }*/

//TODO: messagelistener
//MessageListener.Add((byte)NetworkID.ChatMessage, ChatMessage.Read);


//aus client chatmessage.cs:
/*
public static void Read(BitStream stream)
    {
        ChatMenu chat = ChatMenu.GetChat();
        ChatTextType chatType = (ChatTextType)stream.mReadByte();
        string message = (string)stream.mReadString();

        //TODO:
        //zERROR.GetZErr(Program.Process).Report(2, 'G', "type: " + chatType.ToString() + " message: " + message, 0, "hGame.cs", 0);

        switch (chatType)
        {
            // RP
            case ChatTextType.Say:
                chat.AddRPMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.Shout:
                chat.AddRPMessage(message, new Types.ColorRGBA(72, 118, 255));
                break;
            case ChatTextType.Whisper:
                chat.AddRPMessage(message, new Types.ColorRGBA(131, 111, 255));
                break;
            case ChatTextType.Ambient:
                chat.AddRPMessage(message, new Types.ColorRGBA(255, 127, 36));
                break;
            case ChatTextType.RPGlobal:
                chat.AddRPMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.RPEvent:
                chat.AddRPMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;

            // OOC
            case ChatTextType.OOC:
                chat.AddOOCMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.OOCGlobal:
                chat.AddOOCMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.PM:
                chat.AddOOCMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.OOCEvent:
                chat.AddOOCMessage(message, new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.PlayerSpawn:
                chat.Players.Add(message);
                chat.AddOOCMessage(message + " ist dem Spiel beigetreten", new Types.ColorRGBA(255, 255, 255));
                break;
            case ChatTextType.PlayerDespawn:
                chat.Players.Remove(message);
                chat.AddOOCMessage(message + " hat das Spiel verlassen", new Types.ColorRGBA(255, 255, 255));
                break;

            // Added to both
            case ChatTextType._Error:
                chat.AddToShown(message, new Types.ColorRGBA(255, 0, 0));
                break;
            case ChatTextType._Hint:
                chat.AddToShown(message, new Types.ColorRGBA(255, 0, 255));
                break;
        }
    }

    //siehe chatmenu.cs
    //void SendMsg()
    //{
    //    var msg = textBox.Input;
    //    if (string.IsNullOrWhiteSpace(msg))
    //        return;

    //    var stream = GUC.Network.GameClient.Client.GetMenuMsgStream();
    //    stream.Write((byte)(TeamChat ? MenuMsgID.TeamChat : MenuMsgID.AllChat));
    //    stream.Write(msg);
    //    GUC.Network.GameClient.Client.SendMenuMsg(stream, GUC.Network.PktPriority.LOW_PRIORITY, GUC.Network.PktReliability.RELIABLE);
    //}

    public static void SendMessage(string message)
    {
        //zERROR.GetZErr(Program.Process).Report(2, 'G', "sending " + message.ToString(), 0, "hGame.cs", 0);

        BitStream stream = Program.client.SetupSendStream(NetworkID.ChatMessage);
        stream.mWrite(message);
        Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
    }*/



//aus server chatmessage.cs:
/*
public static void Read(BitStream stream, Client client)
    {
        string message = stream.mReadString();
        Log.Logger.Log("received: " + message.ToString());
        Chat chat = Chat.GetChat();
        chat.MessageReceived(message, client.character);
    }

    public static void SendMessage(string message, NPC to, ChatTextType type)
    {
        Log.Logger.log("[SENDING TO " + to.CustomName + "]: " + message);
        BitStream stream = Program.server.SetupStream(NetworkID.ChatMessage);
        stream.mWrite((byte)type);
        stream.mWrite(message);
        Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', to.client.guid, false);
    }

    public static void SendPlayerSpawned(string playerName)
    {
        BitStream stream = Program.server.SetupStream(NetworkID.ChatMessage);
        stream.mWrite((byte)ChatTextType.PlayerSpawn);
        stream.mWrite(playerName);
        foreach (KeyValuePair<uint, NPC> pair in sWorld.PlayerDict)
        {
            if (pair.Value.client != null)
                Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pair.Value.client.guid, false);
        }
    }

    public static void SendPlayerDespawned(string playerName)
    {
        BitStream stream = Program.server.SetupStream(NetworkID.ChatMessage);
        stream.mWrite((byte)ChatTextType.PlayerDespawn);
        stream.mWrite(playerName);
        foreach (KeyValuePair<uint, NPC> pair in sWorld.PlayerDict)
        {
            Program.server.ServerInterface.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, 'I', pair.Value.client.guid, false);
        }
    }


*/

    //TODO TODO TODO
    /*
using GUC.Network;
namespace GUC.Server.Interface
{
    public class Chat
    {
        public delegate void CommandDelegate(NPC player, string[] parameters);

        Dictionary<uint, NPC> PlayerList;// = sWorld.PlayerDict; //TODO

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
}*/
