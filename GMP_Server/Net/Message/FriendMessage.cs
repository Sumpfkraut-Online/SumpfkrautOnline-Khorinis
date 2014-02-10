using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class FriendMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int senderID; int receiverID; byte type;
            stream.Read(out senderID);
            stream.Read(out receiverID);
            stream.Read(out type);


            Player pl = Player.getPlayer(receiverID, Program.playList);
            if (pl == null)
                return;

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.FriendMessage);
            stream.Write(senderID);
            stream.Write(type);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, pl.guid, false);
        }
    }
}
