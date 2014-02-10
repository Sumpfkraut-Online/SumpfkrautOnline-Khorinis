using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using RakNet;

namespace ListModuleServer
{
    public class SoundSynchMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server)
        {
            int id; String guid, sfx;
            stream.Read(out id); stream.Read(out guid); stream.Read(out sfx);

            Console.WriteLine(sfx);

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xFC);

            stream.Write(id);
            stream.Write(guid);

            stream.Write(sfx);
            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }
    }
}
