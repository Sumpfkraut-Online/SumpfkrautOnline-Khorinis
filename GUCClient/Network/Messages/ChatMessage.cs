using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RakNet;
using GUC.Network;
using GUC.Client.WorldObjects;
using GUC.Enumeration;
using Gothic.zClasses;

using GUC.Client.Menus;

namespace GUC.Client.Network.Messages
{
    static class ChatMessage
    {
        public static void Read(BitStream stream)
        {
            ChatMenu chat = ChatMenu.GetChat();
            ChatTextType chatType = (ChatTextType)stream.mReadByte();
            string message = (string)stream.mReadString();
            zERROR.GetZErr(Program.Process).Report(2, 'G', "type: " + chatType.ToString() + " message: " + message, 0, "hGame.cs", 0);
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


        public static void SendMessage(string message)
        {
            zERROR.GetZErr(Program.Process).Report(2, 'G', "sending " + message.ToString(), 0, "hGame.cs", 0);
            BitStream stream = Program.client.SetupSendStream(NetworkID.ChatMessage);
            stream.mWrite(message);
            Program.client.SendStream(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.UNRELIABLE);
        }
    }
}
