using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using RakNet;
using Network;
using GMP.Modules;
using Injection;
using GMP.Helper;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GMP.Network.Messages
{
    public class StaticNPCMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, String npcinstance, String world, float x, float y, float z)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.StaticNPCMessage);
            stream.Write(npcinstance);
            stream.Write(world);
            stream.Write(x);
            stream.Write(y);
            stream.Write(z);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.StaticNPCMessage))
                StaticVars.sStats[(int)NetWorkIDS.StaticNPCMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.StaticNPCMessage] += 1;
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            if (Program.Player == null)
                return;
            String instance, world; float x, y, z;
            int npcid;

            stream.Read(out npcid);
            stream.Read(out instance);
            stream.Read(out world);
            stream.Read(out x);
            stream.Read(out y);
            stream.Read(out z);

            if (StaticVars.AllPlayerDict.ContainsKey(npcid))
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "This NPC was already spawned:" + instance + " id:" + npcid + " World:" + world, 0, "Program.cs", 0);
                return;
            }



            Player player = new Player("npc");
            player.id = npcid;
            player.pos = new float[] { x, y, z };
            player.instance = instance;
            player.actualMap = Player.getMap(world);
            player.isNPC = true;


            NPC npc = new NPC();
            npc.npcPlayer = player;
            npc.spawn = new float[] { x, y, z };

            player.NPC = npc;

            StaticVars.npcList.Add(npc);
            npc.npcPlayer.NPCList.Add(npc);
            Program.playerList.Add(player);
            StaticVars.AllPlayerDict.Add(player.id, player);
            Program.playerList.Sort(new Player.PlayerComparer());

            if (!Player.isSameMap(player.actualMap, Program.Player.actualMap))
            {
                return;
            }


            Process process = Process.ThisProcess();

            NPCHelper.SpawnPlayer(player,false);
            zVec3 pos = zVec3.Create(process);
            pos.X = player.pos[0]; pos.Y = player.pos[1]; pos.Z = player.pos[2];
            new oCNpc(process, player.NPCAddress).NpcStates.InitAIStateDriven(pos);
            pos.Dispose();
            TimeMessage.firstTimeUpdate = false;
        }
    }
}
