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
    public class AllPlayerSynchMessage2 : Message
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
            //stream.Write(TimeManager.conv_Date2Timestam());





            
            List<Player> nPL = new List<Player>();
            if (findDifferent(Program.Player))
                nPL.Add(Program.Player);
            //Wozu bei anderen als dem Spieler?
            //for (int i = npcid; i < npcid+5; i++)
            //{
            //    if (StaticVars.npcControlList.Count <= i)
            //        break;
            //    Player pl = StaticVars.npcControlList[i].npcPlayer;
            //    if(findDifferent(pl))
            //        nPL.Add(pl);
            //}

            stream.Write(nPL.Count);
            for (int i = 0; i < nPL.Count; i++)
            {
                Player pl = nPL[i];
                WriteDiffrent(pl, stream);
            }
            if (nPL.Count != 0)
            {
                client.client.Send(stream, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AllPlayerSynchMessage))
                    StaticVars.sStats[(int)NetWorkIDS.AllPlayerSynchMessage] = 0;
                StaticVars.sStats[(int)NetWorkIDS.AllPlayerSynchMessage] += 1;
            }
            npcid += 5;
            if (StaticVars.npcControlList.Count <= npcid)
            {
                npcid = 0;
                lastUpdate = DateTime.Now.Ticks;
            }
        }



        public bool findDifferent(Player pl)
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            if (!pl.isSpawned || pl.NPCAddress == 0)
                return false;
            

            int talentsize = 22;
            bool sameTalent = true;
            for (int talentIndex = 0; talentIndex < talentsize; talentIndex++)
            {
                if (npc.GetTalentSkill(talentIndex) != pl.lastTalentSkills[talentIndex])
                {
                    sameTalent = false;
                    break;
                }
            }

            bool sameHitChances = true;
            for (int hcIndex = 1; hcIndex < 5; hcIndex++)
            {
                if (npc.GetHitChances(hcIndex) != pl.lastHitChances[hcIndex - 1])
                {
                    sameHitChances = false;
                    break;
                }
            }

            


            if (npc.HPMax == pl.lastHP_Max && npc.MP == pl.lastMP && npc.MPMax == pl.lastMP_Max
                && npc.Strength == pl.lastStr && npc.Dexterity == pl.lastDex && sameTalent && sameHitChances)
                return false;

            return true;
        }

        public void WriteDiffrent(Player pl, BitStream stream)
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);


            stream.Write(pl.id);

            if (npc.HPMax != pl.lastHP_Max)
            {
                stream.Write((byte)AllPlayerSynchMessageTypes.HP_Max);
                stream.Write(npc.HPMax);

                pl.lastHP_Max = npc.HPMax;
            }
            else if (npc.MP != pl.lastMP)
            {
                stream.Write((byte)AllPlayerSynchMessageTypes.MP);
                stream.Write(npc.MP);

                pl.lastMP = npc.MP;
            }
            else if (npc.MPMax != pl.lastMP_Max)
            {
                stream.Write((byte)AllPlayerSynchMessageTypes.MP_Max);
                stream.Write(npc.MPMax);

                pl.lastMP_Max = npc.MPMax;
            }
            else if (npc.Strength != pl.lastStr)
            {
                stream.Write((byte)AllPlayerSynchMessageTypes.Str);
                stream.Write(npc.Strength);
                pl.lastStr = npc.Strength;

                //pl.lastStr = npc.Strength;
            }
            else if (npc.Dexterity != pl.lastDex)
            {
                stream.Write((byte)AllPlayerSynchMessageTypes.Dex);
                stream.Write(npc.Dexterity);

                pl.lastDex = npc.Dexterity;
                //pl.lastDex = npc.Dexterity;
            }
            else
            {
                bool sameHitChances = true;
                for (int hcIndex = 1; hcIndex < 5; hcIndex++)
                {
                    if (npc.GetHitChances(hcIndex) != pl.lastHitChances[hcIndex - 1])
                    {
                        byte type = (byte)((int)AllPlayerSynchMessageTypes.last + hcIndex - 1);
                        stream.Write(type);
                        stream.Write(npc.GetHitChances(hcIndex));

                        pl.lastHitChances[hcIndex - 1] = npc.GetHitChances(hcIndex);
                        sameHitChances = false;
                        break;
                    }
                }


                if (sameHitChances)
                {
                    for (int talentIndex = 0; talentIndex < pl.lastTalentSkills.Length; talentIndex++)
                    {
                        if (npc.GetTalentSkill(talentIndex) != pl.lastTalentSkills[talentIndex])
                        {
                            byte type = (byte)((int)AllPlayerSynchMessageTypes.last + 4 + talentIndex);
                            stream.Write(type);
                            stream.Write(npc.GetTalentSkill(talentIndex));

                            pl.lastTalentSkills[talentIndex] = npc.GetTalentSkill(talentIndex);
                            break;
                        }
                    }
                }

            }

        }
    }
}
