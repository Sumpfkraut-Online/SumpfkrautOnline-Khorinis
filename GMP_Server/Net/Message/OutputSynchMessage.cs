using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Net.Message
{
    public class OutputSynchMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, packet.guid, true);

        }
    }
}
