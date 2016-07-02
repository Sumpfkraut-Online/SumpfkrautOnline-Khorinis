using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RakNet;
using GUC.Network;
using GUC.Server.WorldObjects;
using GUC.Enumeration;
using GUC.Server.Interface;

namespace GUC.Server.Network.Messages
{
    static class ChatMessage
    {
        public static void Read(BitStream stream, Client client)
        {
            string message = stream.mReadString();
            Log.Logger.log("recieved: " + message.ToString());
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
    }
}
