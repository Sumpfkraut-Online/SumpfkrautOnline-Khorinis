using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using Injection;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;
using GMP.Modules;
using GMP.Logger;
using GMP.Helper;
using System.Windows.Forms;
using System.Threading;

namespace GMP.Net.Messages
{
    public class AllPlayerSynchMessage : Message
    {
        static int npcid;
        static long lastUpdate;
        public void Write(RakNet.BitStream stream, Client client, bool mustNew)
        {
            if (!StaticVars.Ingame || lastUpdate + 10000 * 30 > DateTime.Now.Ticks)
                return;
            Process process = Process.ThisProcess();


            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AllPlayerSynchMessage);
            stream.Write(Program.Player.id);

            
            List<Player> nPL = new List<Player>();
            for (int i = npcid; i < npcid+5; i++)
            {
                if (Program.playerList.Count <= i)
                    break;
                Player pl = Program.playerList[i];
                oCNpc npc = new oCNpc(process, pl.NPCAddress);

                if (!pl.isSpawned || pl.NPCAddress == 0)
                {
                    continue;
                }



                if (!pl.isPlayer)//TODO: Und Kein npc der vom player kontrolliert wird
                {
                    npc.MPMax = 100000;
                    npc.MP = 100000;
                    npc.DiveCTR =  99999.0f;
                    npc.DiveTime = 99999.0f;
                    npc.FallDownDamage = 0;
                }

                

                if (mustNew && npc.HP == pl.lastHP)
                    continue;
                nPL.Add(pl);
            }

            stream.Write(nPL.Count);
            for (int i = 0; i < nPL.Count; i++)
            {
                Player pl = nPL[i];
                oCNpc npc = new oCNpc(process, pl.NPCAddress);

                
                stream.Write(pl.id);

                if (npc.HP != pl.lastHP)
                {
                    stream.Write((byte)AllPlayerSynchMessageTypes.HP);
                    stream.Write(npc.HP);

                    pl.lastHP = npc.HP;
                }
            }
            if (nPL.Count != 0)
            {
                if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AllPlayerSynchMessage))
                    StaticVars.sStats[(int)NetWorkIDS.AllPlayerSynchMessage] = 0;
                StaticVars.sStats[(int)NetWorkIDS.AllPlayerSynchMessage] += 1;
                client.client.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }

            npcid += 5;
            if (Program.playerList.Count <= npcid)
            {
                npcid = 0;
                lastUpdate = DateTime.Now.Ticks;
            }
        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            if (!StaticVars.Ingame)
                return;
            int id = 0;
            int size = 0;

            stream.Read(out id);
            stream.Read(out size);

            if (Program.Player == null || size == 0 || Program.Player.id == id)
            {
                return;
            }


            Process process = Process.ThisProcess();

            
            for (int i = 0; i < size; i++)
            {
                int idnpc;
                byte type = 0;
                int value;
                

                stream.Read(out idnpc);
                stream.Read(out type);
                stream.Read(out value);

                Player pl = StaticVars.AllPlayerDict[idnpc];
                if (pl == null)
                    continue;


                if (type == (byte)AllPlayerSynchMessageTypes.HP)
                {
                    pl.lastHP = value;
                }
                else if (type == (byte)AllPlayerSynchMessageTypes.HP_Max)
                {
                    pl.lastHP_Max = value;
                }
                else if (type == (byte)AllPlayerSynchMessageTypes.MP_Max)
                {
                    pl.lastMP_Max = value;
                }
                else if (type == (byte)AllPlayerSynchMessageTypes.MP)
                {
                    pl.lastMP = value;
                }
                else if (type == (byte)AllPlayerSynchMessageTypes.Str)
                {
                    pl.lastStr = value;
                }
                else if (type == (byte)AllPlayerSynchMessageTypes.Dex)
                {
                    pl.lastDex = value;
                }
                else if (type >= (byte)AllPlayerSynchMessageTypes.last)
                {
                    if (type >= (byte)AllPlayerSynchMessageTypes.last + 4 + 22)
                    {
                        //talents
                        int type2 = type - ((int)AllPlayerSynchMessageTypes.last + 4+22);
                        pl.lastTalentSkills[type2] = value;
                    }
                    else if (type >= (byte)AllPlayerSynchMessageTypes.last + 4)
                    {
                        //talents
                        int type2 = type - ((int)AllPlayerSynchMessageTypes.last + 4);
                        pl.lastTalentValues[type2] = value;
                    }
                    else
                    {
                        //hitchances
                        int type2 = type - (byte)AllPlayerSynchMessageTypes.last;
                        pl.lastHitChances[type2] = value;
                    }
                }


                if (!pl.isSpawned || pl.NPCAddress == 0)
                    continue;

                oCNpc npc = new oCNpc(process, pl.NPCAddress);
                

                if (type == (byte)AllPlayerSynchMessageTypes.HP)
                {
                    if (npc.HP == 0 && value != 0 && npc.Address != 0)
                    {
                        try
                        {
                            zVec3 pos = npc.GetPosition();
                            npc.ResetPos(pos);
                            pos.Dispose();
                        }
                        catch (Exception ex) { }//TODO: kA warum das bei Seq bugged :P
                    }
                    npc.HP = value;
                    if (value == 1)
                    {
                        npc.DropUnconscious(0, new oCNpc(process, 0));
                        npc.HP = 2;
                    }
                }
                else if (type == (byte)AllPlayerSynchMessageTypes.HP_Max)
                    npc.HPMax = value;
                else if (type == (byte)AllPlayerSynchMessageTypes.MP)
                    npc.MP = value;
                else if (type == (byte)AllPlayerSynchMessageTypes.MP_Max)
                    npc.MPMax = value;
                else if (type == (byte)AllPlayerSynchMessageTypes.Str)
                    npc.Strength = value;
                else if (type == (byte)AllPlayerSynchMessageTypes.Dex)
                    npc.Dexterity = value;
                else if (type >= (byte)AllPlayerSynchMessageTypes.last)
                {
                    if (type >= (byte)AllPlayerSynchMessageTypes.last + 4 + 22)
                    {
                        //talents
                        int type2 = type - ((int)AllPlayerSynchMessageTypes.last + 4 + 22);
                        npc.SetTalentSkill(type2, value);

                    }
                    else if (type >= (byte)AllPlayerSynchMessageTypes.last + 4)
                    {
                        //talents
                        int type2 = type - ((int)AllPlayerSynchMessageTypes.last + 4);
                        npc.SetTalentValue(type2, value);

                    }
                    else
                    {
                        //hitchances
                        int type2 = type - (byte)AllPlayerSynchMessageTypes.last;
                        npc.SetHitChances(type2 + 1, value);
                    }
                }
            }
        }
    }
}
