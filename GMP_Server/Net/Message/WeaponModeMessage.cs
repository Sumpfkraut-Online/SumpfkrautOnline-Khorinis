using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class WeaponModeMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            byte type;
            String mode = "";

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out mode);

            Player pl = Player.getPlayerSort(id, Program.playerList);
            if (pl != null)
            {
                pl.lastWeaponModeType = type;
                pl.lastWeaponMode = mode;
            }

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.WeaponModeMessage);
            stream.Write(id);
            stream.Write(type);
            stream.Write(mode);

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
