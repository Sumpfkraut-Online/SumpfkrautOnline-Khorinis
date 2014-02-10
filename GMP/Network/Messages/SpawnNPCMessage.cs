using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Gothic.zTypes;
using GMP.Modules;
using Injection;
using GMP.Helper;
using WinApi;
using Gothic.zClasses;
using GMP.Injection.Hooks;

namespace GMP.Network.Messages
{
    public class SpawnNPCMessage : Message
    {
        public void Write(RakNet.BitStream stream, Client client, bool isSummoned,
            String instance, float[] pos, String wp, String world, int[] ids)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.SpawnNPCMessage);
            //stream.Write(Program.Player.id);
            stream.Write(Program.Player.id);
            stream.Write(instance);
            stream.Write(world);
            stream.Write(ids.Length);

            for (int i = 0; i < ids.Length; i++)
                stream.Write(ids[i]);
            
            stream.Write(isSummoned);
            if (isSummoned)
            {
                stream.Write(pos[0]);
                stream.Write(pos[1]);
                stream.Write(pos[2]);
            }
            else
            {
                stream.Write(wp);
                stream.Write(pos[0]);
                stream.Write(pos[1]);
                stream.Write(pos[2]);
            }
            

            client.client.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.SpawnNPCMessage))
                StaticVars.sStats[(int)NetWorkIDS.SpawnNPCMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.SpawnNPCMessage] += 1;
        }

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            String instance, world;
            bool isSummoned;
            int npcid;

            if (!StaticVars.Ingame)
                return;

            stream.Read(out npcid);
            stream.Read(out instance);
            stream.Read(out world);
            stream.Read(out isSummoned);



            if (isSummoned)
            {
                float[] pos = new float[3];
                stream.Read(out pos[0]);
                stream.Read(out pos[1]);
                stream.Read(out pos[2]);

                NPC npc = null;

                
                if (npc == null)
                {
                    Player player = new Player("npc");
                    player.id = npcid;
                    player.pos = pos;
                    player.instance = instance;
                    player.actualMap = Player.getMap(world);
                    player.isNPC = true;


                    npc = new NPC();
                    npc.npcPlayer = player;
                    npc.spawn = pos;
                    npc.isSummond = true;

                    player.NPC = npc;

                    //System.Windows.Forms.MessageBox.Show("Null NPc: " + player.instance);

                    //Spawn
                    if (Player.isSameMap(npc.npcPlayer.actualMap, Program.Player.actualMap))
                    {
                        NPCHelper.SpawnPlayer(npc.npcPlayer, false);
                    }
                }
                else
                {
                    npc.npcPlayer.id = npcid;
                }
                StaticVars.npcList.Add(npc);
                npc.npcPlayer.NPCList.Add(npc);

                StaticVars.AllPlayerDict.Add(npc.npcPlayer.id, npc.npcPlayer);
                Program.playerList.Add(npc.npcPlayer);
                Program.playerList.Sort(new Player.PlayerComparer());
                
            }
            else
            {
                String wp = "";
                stream.Read(out wp);
                

                NPC npc = null;

                if (npcid != -1)
                {
                    if (npc == null)
                    {
                        Player player = new Player("npc");
                        player.id = npcid;
                        //player.pos = pos;
                        player.instance = instance;
                        player.actualMap = Player.getMap(world);
                        player.isNPC = true;


                        npc = new NPC();
                        npc.npcPlayer = player;
                        npc.wp = wp.Trim().ToUpper();
                        npc.isSpawned = true;

                        player.NPC = npc;


                        

                        //System.Windows.Forms.MessageBox.Show("Null NPc: " + player.instance);

                        //Spawn
                        if (Player.isSameMap(npc.npcPlayer.actualMap, Program.Player.actualMap))
                        {
                            //wp suchen und position eintragen...
                            Process process = Process.ThisProcess();
                            float[] pos = oCGame.Game(process).World.WayNet.getWaypointPosition(npc.wp);
                            if (pos == null)
                                return;


                            npc.spawn = pos;
                            npc.npcPlayer.pos = pos;
                            NPCHelper.SpawnPlayer(npc.npcPlayer, false);
                        }
                    }
                    else
                    {
                        npc.npcPlayer.id = npcid;
                    }
                    StaticVars.npcList.Add(npc);
                    npc.npcPlayer.NPCList.Add(npc);

                    StaticVars.AllPlayerDict.Add(npc.npcPlayer.id, npc.npcPlayer);
                    Program.playerList.Add(npc.npcPlayer);
                    Program.playerList.Sort(new Player.PlayerComparer());
                }
                else
                {
                    if (npc != null)
                    {
                        Process process = Process.ThisProcess();
                        oCGame.Game(process).GetSpawnManager().DeleteNPC(new oCNpc(process, npc.npcPlayer.NPCAddress));
                    }
                }
            }





        }
    }
}
