using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class AnimationOverlayMessage : Message
    {
        public override void  Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            byte type = 0;
            String aniID = "";
            float value = 0;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out aniID);
            stream.Read(out value);
            stream.Reset();

            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AnimationOverlayMessage);
            stream.Write(id);
            stream.Write(type);
            stream.Write(aniID);
            stream.Write(value);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
