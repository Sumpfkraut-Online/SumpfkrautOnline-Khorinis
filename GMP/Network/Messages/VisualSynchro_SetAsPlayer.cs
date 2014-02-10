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
    public class VisualSynchro_SetAsPlayer : Message
    {
        public void Write(RakNet.BitStream stream, Client client, String instance)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.VisualSynchro_SetAsPlayer);
            stream.Write(Program.Player.id);
            stream.Write(instance);

            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.VisualSynchro_SetAsPlayer))
                StaticVars.sStats[(int)NetWorkIDS.VisualSynchro_SetAsPlayer] = 0;
            StaticVars.sStats[(int)NetWorkIDS.VisualSynchro_SetAsPlayer] += 1;
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            int id;
            String instance;

            stream.Read(out id);
            stream.Read(out instance);

            if (!StaticVars.Ingame)
                return;
            if (!StaticVars.AllPlayerDict.ContainsKey(id))
                return;

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null || !pl.isSpawned || pl.isPlayer)
                return;

            pl.instance = instance;

            NPCHelper.RemovePlayer(pl, false);
            NPCHelper.SpawnPlayer(pl,true);
            NPCHelper.SetStandards(pl);
        }
    }
}
