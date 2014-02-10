using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Injection;
using RakNet;
using GMP.Helper;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class LevelChangeMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id;
            string levelname;

            stream.Read(out id);
            stream.Read(out levelname);

            Player pl = Player.getPlayer(id, StaticVars.playerlist);
            if (pl == null || pl.isPlayer)
                return;

            pl.actualMap = Player.getMap(levelname);
            if (Player.isSameMap(pl.actualMap, Program.Player.actualMap))
            {
                NPCHelper.SpawnPlayer(pl,true);
                NPCHelper.SetStandards(pl);
            }
            else if (!Player.isSameMap(pl.actualMap, Program.Player.actualMap))
            {
                NPCHelper.RemovePlayer(pl, false);
            }
        }

        public void Write(RakNet.BitStream stream, Client client, String levelname)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.LevelChangeMessage);
            stream.Write(Program.Player.id);
            stream.Write(levelname);

            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.LevelChangeMessage))
                StaticVars.sStats[(int)NetWorkIDS.LevelChangeMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.LevelChangeMessage] += 1;
        }
    }
}
