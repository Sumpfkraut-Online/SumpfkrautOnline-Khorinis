using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class SpawnNPCMessage : Message
    {
        public static int CountNPCSummoned(String instance, String world, float[] pos)
        {
            int count = 0;
            int distance = 800;
            foreach (NPC npc in Program.npcList)
            {
                if (npc.isSummond &&
                    (npc.npcPlayer.instance == instance || "SUMMONED_" + npc.npcPlayer.instance == instance
                        || npc.npcPlayer.instance == "SUMMONED_" + instance)
                    && Player.isSameMap(npc.npcPlayer.actualMap, world))
                {
                    if (pos[0] - distance < npc.spawn[0] && pos[0] + distance > npc.spawn[0]
                        && pos[1] - distance < npc.spawn[1] && pos[1] + distance > npc.spawn[1]
                            && pos[2] - distance < npc.spawn[2] && pos[2] + distance > npc.spawn[2])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public static int CountNPCSummoned(String instance, String world, String wp)
        {
            int count = 0;
            foreach (NPC npc in Program.npcList)
            {
                if (npc.isSpawned &&
                    npc.npcPlayer.instance.Trim().ToUpper() == instance.Trim().ToUpper()
                    && Player.isSameMap(npc.npcPlayer.actualMap, world))
                {
                    if (wp.Trim().ToUpper() == npc.wp.Trim().ToUpper())
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            String instance,world;
            int count;
            bool isSummoned;
            int id;

            stream.Read(out id);
            stream.Read(out instance);
            stream.Read(out world);
            stream.Read(out count);

            instance = instance.Trim().ToUpper();

            int[] ids = new int[count];
            for (int i = 0; i < ids.Length; i++)
                stream.Read(out ids[i]);

            stream.Read(out isSummoned);

            if (isSummoned)
            {
                float[] pos = new float[3];
                stream.Read(out pos[0]);
                stream.Read(out pos[1]);
                stream.Read(out pos[2]);

                int servercount = CountNPCSummoned(instance, world, pos);
                if (servercount == count)
                {
                    //Komplett neuer NPC, also zu allen eine Nachricht und neuen NPC anlegen

                    Program.idCount += 1;

                    Player pl = new Player("NPC");

                    pl.id = Program.idCount;
                    pl.actualMap = world.Trim().ToUpper();
                    pl.instance = instance;
                    pl.isSpawned = true;
                    pl.pos = pos;
                    pl.isNPC = true;

                    NPC npc = new NPC();
                    npc.isSummond = true;
                    npc.npcPlayer = pl;
                    npc.controller = null;
                    npc.spawn = pos;

                    pl.NPC = npc;

                    Program.npcList.Add(npc);
                    
                    Program.playerDict.Add(pl.id, pl);
                    Program.playerList.Add(pl);
                    Program.playerList.Sort(new Network.Player.PlayerComparer());

                    Write(stream, server, true, npc, null);

                    if (instance.StartsWith("SUMMONED_"))
                    {
                        if (npc.controller != null)
                        {
                            new NPCControllerMessage().Write(stream, server, npc, false);
                            npc.controller = null;
                        }
                        npc.controller = Player.getPlayer(id, Program.playList);
                        new NPCControllerMessage().Write(stream, server, npc, true);
                    }
                }
                else
                {
                    //NPC ist alt, also id raussuchen und spawnmessage zurück schicken
                    //Write(stream, server, false, instance, world, isSummoned, pos, null, packet.systemAddress);
                }
            }
            else
            {
                String wp;
                stream.Read(out wp);
                float[] pos = new float[3];
                stream.Read(out pos[0]);
                stream.Read(out pos[1]);
                stream.Read(out pos[2]);

                int servercount = CountNPCSummoned(instance, world, wp);
                Console.WriteLine("Pos: " + pos[0] + " " + pos[1] + " " + pos[2] + " " + wp + " " + instance);
                if (servercount == count)
                {
                    Console.WriteLine("Neuer NPC: "+instance );
                    //Komplett neuer NPC, also zu allen eine Nachricht und neuen NPC anlegen
                    createNPC(instance, pos, world, wp, stream, server);
                    //Program.idCount += 1;

                    //Player pl = new Player("NPC");

                    //pl.id = Program.idCount;
                    //pl.actualMap = Player.getMap(world);
                    //pl.instance = instance.Trim().ToUpper();
                    //pl.isSpawned = true;
                    //pl.pos = pos;

                    //pl.isNPC = true;

                    //NPC npc = new NPC();
                    //npc.isSpawned = true;
                    //npc.npcPlayer = pl;
                    //npc.controller = null;
                    //npc.wp = wp;
                    //npc.spawn = pos;

                    //pl.NPC = npc;

                    //Program.npcList.Add(npc);
                    //Program.playerList.Add(pl);
                    //Program.playerList.Sort(new Network.Player.PlayerComparer());

                    //Write(stream, server, true, npc, null);
                }
                else
                {

                    Console.WriteLine("Doppelt Spawn NPC: " + instance + "C:" + count+"|"+servercount);
                    //Löschen....
                    Player pl = new Player("NPC");

                    pl.id = -1;
                    pl.actualMap = Player.getMap(world);
                    pl.instance = instance.Trim().ToUpper();
                    pl.isSpawned = true;

                    pl.isNPC = true;

                    NPC npc = new NPC();
                    npc.isSpawned = true;
                    npc.npcPlayer = pl;
                    npc.controller = null;
                    npc.wp = wp;

                    pl.NPC = npc;

                    Write(stream, server, true, npc, packet.guid);



                    //Freie ID suchen
                    //NPC _npc = null;
                    //foreach (NPC npc in Program.npcList)
                    //{
                    //    if (npc.isSpawned && npc.wp.Trim().ToUpper() == wp.Trim().ToUpper()
                    //        && npc.npcPlayer.actualMap.Trim().ToUpper() == world.Trim().ToUpper())
                    //    {
                    //        bool isIn = false;
                    //        foreach (int currId in ids)
                    //        {
                    //            if (currId == npc.npcPlayer.id)
                    //            {
                    //                isIn = true;
                    //                break;
                    //            }
                    //        }

                    //        if (!isIn)//ID ist iO.
                    //        {
                    //            _npc = npc;
                    //            break;
                    //        }
                    //    }
                    //}

                    //if (_npc != null)//NPC id schicken . . . 
                    //{

                    //}

                }
            }
        }

        public static NPC createNPC(String instance, float[] pos, String world, String wp, RakNet.BitStream stream, Server server)
        {
            Program.idCount += 1;

            Player pl = new Player("NPC");

            pl.id = Program.idCount;
            pl.actualMap = Player.getMap(world);
            pl.instance = instance.Trim().ToUpper();
            pl.isSpawned = true;
            pl.pos = pos;

            pl.isNPC = true;

            NPC npc = new NPC();
            npc.isSpawned = true;
            npc.npcPlayer = pl;
            npc.controller = null;
            npc.wp = wp;
            npc.spawn = pos;
            npc.isSummond = true;
            npc.isStatic = true;

            pl.NPC = npc;


            Program.playerDict.Add(pl.id, pl);
            Program.npcList.Add(npc);
            Program.playerList.Add(pl);
            Program.playerList.Sort(new Network.Player.PlayerComparer());

            new SpawnNPCMessage().Write(stream, server, true, npc, null);

            return npc;
        }

        public void Write(RakNet.BitStream stream, Server server, bool newNPC, NPC max, RakNet.AddressOrGUID addrorGuid)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.SpawnNPCMessage);
            stream.Write(max.npcPlayer.id);
            stream.Write(max.npcPlayer.instance);
            stream.Write(max.npcPlayer.actualMap);
            stream.Write(max.isSummond);

            if (max.isSummond)
            {
                stream.Write(max.spawn[0]);
                stream.Write(max.spawn[1]);
                stream.Write(max.spawn[2]);
            }
            else
            {
                stream.Write(max.wp);
            }

            if (newNPC)
            {
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }
            else
            {
                server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, addrorGuid, false);
            }
        }
    }
}
