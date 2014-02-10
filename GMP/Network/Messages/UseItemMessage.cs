using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;
using Injection;
using GMP.Helper;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class UseItemMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, String instance)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.UseItemMessage);
            stream.Write(Program.Player.id);
            stream.Write(instance);

            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            int id;
            String instance;

            stream.Read(out id);
            stream.Read(out instance);

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null || !pl.isSpawned || pl.isPlayer)
                return;

            
        }
    }
}
