using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class VisualSynchro_SetAsPlayer : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id;
            String instance;

            stream.Read(out id);
            stream.Read(out instance);

            Player pl = Player.getPlayerSort(id, Program.playerList);
            if (pl == null)
                return;
            pl.instance = instance;

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.VisualSynchro_SetAsPlayer);
            stream.Write(id);
            stream.Write(instance);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
