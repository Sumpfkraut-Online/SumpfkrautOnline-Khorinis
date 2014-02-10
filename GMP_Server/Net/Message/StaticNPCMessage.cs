using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class StaticNPCMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            String instance, world; float x, y, z;

            stream.Read(out instance);
            stream.Read(out world);
            stream.Read(out x);
            stream.Read(out y);
            stream.Read(out z);


            Program.idCount += 1;
            int npcid = Program.idCount;

            
            Player pl = new Player("NPC");

            pl.id = npcid;
            pl.actualMap = world.Trim().ToUpper();
            pl.instance = instance;
            pl.isSpawned = true;
            pl.pos = new float[] { x, y, z };
            pl.isNPC = true;

            NPC npc = new NPC();
            npc.isStatic = true;
            npc.npcPlayer = pl;
            npc.controller = null;
            npc.spawn = new float[] { x, y, z };

            pl.NPC = npc;

            Console.WriteLine("Hey ho npc spawn O.o");


            Write(stream, server, npc);
            //new NPCControllerMessage().Write(stream, server, npc, true);
        }

        public void Write(RakNet.BitStream stream, Server server, NPC npc)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.StaticNPCMessage);
            stream.Write(npc.npcPlayer.id);
            stream.Write(npc.npcPlayer.instance);
            stream.Write(npc.npcPlayer.actualMap);
            stream.Write(npc.npcPlayer.pos[0]);
            stream.Write(npc.npcPlayer.pos[1]);
            stream.Write(npc.npcPlayer.pos[2]);

            Program.npcList.Add(npc);
            Program.playerDict.Add(npc.npcPlayer.id, npc.npcPlayer);
            Program.playerList.Add(npc.npcPlayer);
            Program.playerList.Sort(new Network.Player.PlayerComparer());

            server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }
    }
}
