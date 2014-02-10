using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Net.Message
{
    public class TimeMessage : Message
    {
        public override void Write(RakNet.BitStream stream, Server server)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)Network.NetWorkIDS.TimeMessage);
            stream.Write(Program.gTime.GetDay());
            stream.Write(Program.gTime.getHourInDay());
            stream.Write(Program.gTime.GetMinuteInHour());

            server.server.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }
    }
}
