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
            Log.Logger.log(Log.Logger.LOG_INFO, text);

            foreach (Player client in sWorld.PlayerList)
            {
                SendText(client, pl.Name + ": " + text);
            }
        }

        private void SendText(Player to, string text)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);

            stream.Write(text);

            using (RakNetGUID guid = to.GUID)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }
    }
}
