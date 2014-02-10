using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class PassivePerceptionMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int type, npc1_ID, npc2_ID, sNPC_ID;

            stream.Read(out type);
            stream.Read(out sNPC_ID);
            stream.Read(out npc1_ID);
            stream.Read(out npc2_ID);

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PassivePerceptionMessage);
            stream.Write(type);
            stream.Write(sNPC_ID);
            stream.Write(npc1_ID);
            stream.Write(npc2_ID);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
