using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.Server.Network.Messages
{
    class ChatMessage : IMessage
    {
        public ChatMessage()
        {

        }

        //we received a chat text from a client
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0;
            string text;

            stream.Read(out playerID);
            Player pl = (Player)sWorld.VobDict[playerID];

            stream.Read(out text);

            ChatTextType type = ChatTextType.Say;
            int range = 300;

            int removeCount = 0;
            if (text.StartsWith("!/ooc")) //out-of-character global
            {
                removeCount = 5;
                type = ChatTextType.GlobalOOC;
            }
            else if (text.StartsWith("!//"))
            {
                removeCount = 3;
                type = ChatTextType.GlobalOOC;
            } 
            else if (text.StartsWith("!")) //shout
            {
                removeCount = 1;
                type = ChatTextType.Shout;
                range = 600;
            }
            else if (text.StartsWith(".")) //whisper
            {
                removeCount = 1;
                type = ChatTextType.Whisper;
                range = 50;
            }
            else if (text.StartsWith("/me")) //ambient "Diego macht irgendwas."
            {
                removeCount = 3;
                type = ChatTextType.Ambient;
            }
            else if (text.StartsWith("/ooc")) //out-of-character
            {
                removeCount = 4;
                type = ChatTextType.OOC;
            }
            else if (text.StartsWith("//"))
            {
                removeCount = 2;
                type = ChatTextType.OOC;
            }
            else if (text.StartsWith("/global")) //global for admins
            {
                removeCount = 7;
                type = ChatTextType.Global;
            }
            else if (text.StartsWith("/"))
            {
                return;
            }
            text = text.Remove(0, removeCount).Trim();

            if (type == ChatTextType.GlobalOOC || type == ChatTextType.Global)
            {
                SendText(type, pl, null, text); //send to all
            }
            else
            {
                foreach (Player client in sWorld.PlayerList)
                {
                    if (pl.Position.getDistance(client.Position) < range)
                    {
                        SendText(type, pl, client, text);
                    }
                }
            }


        }

        private void SendText(ChatTextType type, Player from, Player to, string text)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write((byte)type);
            if (type != ChatTextType.Global)
            {
                stream.Write(from.ID);
            }
            stream.Write(text);


            if (to == null /*type == ChatTextType.Global || type == ChatTextType.GlobalOOC*/) //send to everyone
            {
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                return;
            }

            using (RakNetGUID guid = to.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }
    }
}
