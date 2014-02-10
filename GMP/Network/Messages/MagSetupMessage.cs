using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using RakNet;
using Network;
using Injection;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class MagSetupMessage : Message
    {
        static List<zCVob> lastVob = new List<zCVob>();
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id = 0;

            byte type = 0;
            int vobtype = 0;
            String name = "";
            float[] pos = new float[16];
            int x=0,y=0,z=0;
            int plid = 0;


            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out vobtype);
            stream.Read(out x);
            stream.Read(out y);
            stream.Read(out z);
            if (vobtype != (int)Gothic.zClasses.zCVob.VobTypes.Npc)
            {
                stream.Read(out name);


                for (int i = 0; i < 16; i++)
                    stream.Read(out pos[i]);
            }
            else
            {
                stream.Read(out plid);
            }

            

            if (Program.Player == null || id == Program.Player.id)
                return;

            

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null || !pl.isSpawned || pl.NPCAddress == 0)
                return;
            Process process = Process.ThisProcess();

            if (pl.NPCAddress == 0)
                zERROR.GetZErr(process).Report(3, 'G', "Player NPC Address was 0", 0, "Program.cs", 0);

            
            zCVob currMobInter = null;

            if (name.Trim().Length == 0 && vobtype != (int)Gothic.zClasses.zCVob.VobTypes.Npc)
            {
                if (type == 0)
                {
                    oCNpc magNPC = new oCNpc(process, pl.NPCAddress);
                    magNPC.MagBook.Spell_Setup(magNPC, magNPC, x);
                }
                else if (type == 1)
                {
                    float x2, y2, z2;
                    byte[] arr = BitConverter.GetBytes(z);
                    x2 = BitConverter.ToSingle(arr, 0);
                    arr = BitConverter.GetBytes(y);
                    y2 = BitConverter.ToSingle(arr, 0);
                    arr = BitConverter.GetBytes(x);
                    z2 = BitConverter.ToSingle(arr, 0);
                    zVec3 posV = zVec3.Create(process);
                    posV.X = x2;
                    posV.Y = y2;
                    posV.Z = z2;
                    new oCNpc(process, pl.NPCAddress).MagBook.StartCastEffect(new zCVob(process, 0), posV);
                }
                return;
            }



            if (vobtype == (int)Gothic.zClasses.zCVob.VobTypes.Npc)
            {
                //Spieler... Bewegen sich usw. darum stimmt position nicht mehr....
                Player play = StaticVars.AllPlayerDict[plid];
                if(play != null && play.NPCAddress != 0)
                    currMobInter = new zCVob(process, play.NPCAddress);
            }

            if (currMobInter == null)
            {
                foreach (zCVob mob in lastVob)
                {
                    if (mob == null || (int)mob.VobType != vobtype)
                    {
                        continue;
                    }

                    if (mob.ObjectName.Value.Trim() != name.Trim())
                    {
                        continue;
                    }

                    bool breaked = false;
                    for (int i = 0; i < 16; i++)
                    {
                        if (pos[i] < (int)mob.TrafoObjToWorld.get(i) - 5
                            || pos[i] > (int)mob.TrafoObjToWorld.get(i) + 5)
                        {
                            breaked = true;
                            break;
                        }
                    }

                    if (!breaked)
                    {
                        currMobInter = mob;
                        break;
                    }
                }
            }
            
            if (currMobInter == null)
            {
                zCTree<zCVob> vobtree = oCGame.Game(process).World.GlobalVobTree.FirstChild;
                while (vobtree.Address != 0)
                {
                    zCVob vob = vobtree.Data;
                    if (vob == null || (int)vob.VobType != vobtype)
                    {
                        vobtree = vobtree.Next;
                        continue;
                    }

                    if (vob.ObjectName.Value.Trim() != name.Trim())
                    {
                        vobtree = vobtree.Next;
                        continue;
                    }

                    bool breaked = false;
                    for (int i = 0; i < 16; i++)
                    {
                        if (pos[i] < (int)vob.TrafoObjToWorld.get(i) - 5
                            || pos[i] > (int)vob.TrafoObjToWorld.get(i) + 5)
                        {
                            breaked = true;
                            break;
                        }
                    }

                    if (!breaked)
                    {
                        currMobInter = vob;
                        break;
                    }

                    vobtree = vobtree.Next;
                }
            }


            //Vob gefunden


            if (currMobInter != null && currMobInter.Address != 0)
            {
                lastVob.Add(currMobInter);
                if (lastVob.Count > 100)
                    lastVob.RemoveAt(0);
                if (type == 0)
                {
                    new oCNpc(process, pl.NPCAddress).MagBook.Spell_Setup(new oCNpc(process, pl.NPCAddress), currMobInter, x);
                }
                else if (type == 1)
                {
                    float x2, y2, z2;
                    byte[] arr = BitConverter.GetBytes(z);
                    x2 = BitConverter.ToSingle(arr, 0);
                    arr = BitConverter.GetBytes(y);
                    y2 = BitConverter.ToSingle(arr, 0);
                    arr = BitConverter.GetBytes(x);
                    z2 = BitConverter.ToSingle(arr, 0);
                    zVec3 posV = zVec3.Create(process);
                    posV.X = x2;
                    posV.Y = y2;
                    posV.Z = z2;
                    new oCNpc(process, pl.NPCAddress).MagBook.StartCastEffect(currMobInter, posV);
                }
                else if (type == 2)
                {
                    new oCNpc(process, pl.NPCAddress).MagBook.StartInvestEffect(currMobInter, x,y,z);
                }
            }
            else
            {
            }




        }

        public void Write(RakNet.BitStream stream, Client client, zCVob vob, byte type, int x, int y, int z)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.MagSetupSynch);
            stream.Write(Program.Player.id);
            stream.Write(type);
            stream.Write((int)vob.VobType);
            stream.Write(x);
            stream.Write(y);
            stream.Write(z);

            if ((int)vob.VobType != (int)Gothic.zClasses.zCVob.VobTypes.Npc)
            {
                stream.Write(vob.ObjectName.Value);

                for (int i = 0; i < 16; i++)
                    stream.Write(vob.TrafoObjToWorld.get(i));
            }
            else
            {
                int strid = 0;
                foreach (Player pl in Program.playerList)
                {
                    if (pl.NPCAddress == vob.Address)
                    {
                        strid = pl.id;
                        break;
                    }
                }
                stream.Write(strid);
            }
            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.MagSetupSynch))
                StaticVars.sStats[(int)NetWorkIDS.MagSetupSynch] = 0;
            StaticVars.sStats[(int)NetWorkIDS.MagSetupSynch] += 1;
        }
    }
}
