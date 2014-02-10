using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class AttackSynchMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id1; int id2; byte modeDamage; byte modeWeapon;
            byte item;
            String weapon = "";

            stream.Read(out id1);
            stream.Read(out id2);
            stream.Read(out modeDamage);
            stream.Read(out modeWeapon);
            stream.Read(out item);
            stream.Read(out weapon);

            Program.scriptManager.OnDamage(ref id1, ref id2, ref modeDamage, ref modeWeapon, item, weapon);


            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AttackSynchMessage);
            stream.Write(id1);
            stream.Write(id2);
            stream.Write(modeDamage);
            stream.Write(modeWeapon);
            stream.Write(item);
            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);

        }
    }
}
