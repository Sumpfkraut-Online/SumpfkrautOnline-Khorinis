using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;
using WinApi;
using Gothic.zClasses;
using Injection;
using GMP.Logger;
using System.Windows.Forms;
using Gothic.zTypes;
using GMP.Modules;
using System.Threading;
using Network.Types;
using GMP.Helper;

namespace GMP.Net.Messages
{
    public class PlayerStatusMessage : Message
    {
        static int npcid;
        static long lastUpdate;

        static List<Player> updatePositionList = new List<Player>();
        static List<Player> updateDirList = new List<Player>();

        private static bool updatePlayerPosition(Player pl)
        {
            if (!pl.isSpawned || pl.NPCAddress == oCNpc.Player(Process.ThisProcess()).Address || (pl.isNPC && pl.NPC.controller == Program.Player))
                return true;

            
            oCNpc npc = new oCNpc(Process.ThisProcess(), pl.NPCAddress);
            Matrix4 trafo = npc.TrafoObjToWorld;

            if (new Vec3f(Program.Player.pos).getDistance((Vec3f)pl.pos) > 4000)//Wenn Spieler weiter als 40m Entfernt, braucht er nicht zu aktualisieren
            {
                trafo.set(3, pl.pos[0]);
                trafo.set(7, pl.pos[1]);
                trafo.set(11, pl.pos[2]);
                return true;
            }


            float[] pos = new float[] { trafo.get(3), trafo.get(7), trafo.get(11) };
            Vec3f ap = (Vec3f)pos;//Real position
            Vec3f rp = (Vec3f)pl.pos;//New position

            if (rp.getDistance(ap) > 5.0f)
            {
                Vec3f t = rp - ap;
                t = t.normalise();
                t.data[0] *= 0.001f;
                t.data[1] *= 0.001f;
                t.data[2] *= 0.001f;
                
                Vec3f np = ap + t;
                trafo.set(3, np.X);
                trafo.set(7, np.Y);
                trafo.set(11, np.Z);

                return false;
            }
            else
            {
                trafo.set(3, pl.pos[0]);
                trafo.set(7, pl.pos[1]);
                trafo.set(11, pl.pos[2]);

                return true;
            }
        }

        private static bool updatePlayerDir(Player pl)
        {
            if (!StaticVars.Ingame)
                return true;

            if (!pl.isSpawned || pl.NPCAddress == oCNpc.Player(Process.ThisProcess()).Address)
                return true;


            oCNpc npc = new oCNpc(Process.ThisProcess(), pl.NPCAddress);
            Matrix4 trafo = npc.TrafoObjToWorld;
            double angle = (Math.Atan2(pl.dir[0], pl.dir[2]) * (180.0 / Math.PI));

            npc.ResetRotationsWorld();
            npc.RotateWorldY((float)angle);

            return true;

            //TODO: Winkel langsam verändern:
            
            if (new Vec3f(Program.Player.pos).getDistance((Vec3f)pl.pos) > 4000)//Wenn Spieler weiter als 40m Entfernt, braucht er nicht zu aktualisieren
            {
                npc.ResetRotationsWorld();
                npc.RotateWorldY((float)angle);

                return true;
            }

            float[] dir2 = trafo.getDirection();
            double angle2 = (Math.Atan2(dir2[0], dir2[2]) * (180.0 / Math.PI));

            double angleAdd = 2.0;

            double cA = angle < 0 ? -1.0* angle : angle;
            double cA2 = angle2 < 0 ? -1.0 * angle2 : angle2;

            double diff = cA - cA2;
            diff = diff < 0 ? -1.0 * diff : diff;

            if (diff <= 10.0)
            {
                npc.ResetRotationsWorld();
                npc.RotateWorldY((float)angle);

                return true;
            }
            else
            {
                if (angle2 < 0)
                {
                    if (angle < angle2 || angle2 + 180.0 < angle)
                    {
                        angle = angle2 - angleAdd;
                    }
                    else
                    {
                        angle = angle2 + angleAdd;
                    }
                }
                else if (angle < 0)
                {
                    if (angle + 180.0 < angle2)
                    {
                        angle = angle2 + angleAdd;
                    }
                    else
                    {
                        angle = angle2 - angleAdd;
                    }
                }
                else
                {
                    if(angle > angle2)
                        angle = angle2 + angleAdd;
                    else
                        angle = angle2 - angleAdd;
                }
                npc.ResetRotationsWorld();
                npc.RotateWorldY((float)angle);
                
                return false;
            }
            
        }

        public static void update()
        {
            Player[] uPl = updatePositionList.ToArray();
            foreach (Player pl in uPl)
            {
                bool t = updatePlayerPosition(pl);
                if (t)
                    updatePositionList.Remove(pl);

            }
            uPl = updateDirList.ToArray();
            foreach (Player pl in uPl)
            {
                bool t = updatePlayerDir(pl);
                if (t)
                    updateDirList.Remove(pl);

            }
        }


        public override void Write(RakNet.BitStream stream, Client client)
        {
            if (!StaticVars.Ingame || lastUpdate + 10000*200 > DateTime.Now.Ticks)
                return;
            Process process = Process.ThisProcess();
            
            byte count = 1 + 5;
            if (npcid + 5 >= StaticVars.npcControlList.Count)
            {
                count = (byte)(count - ((npcid + 5)-StaticVars.npcControlList.Count));
            }

            WritePlayer(Program.Player, stream, client);
            for (int i = npcid; i < npcid + 5; i++)
            {
                if (i >= StaticVars.npcControlList.Count)
                    break;
                WritePlayer(StaticVars.npcControlList[i].npcPlayer, stream, client);
            }






            //type = 0;

            npcid += 5;
            if (StaticVars.npcControlList.Count <= npcid)
            {
                npcid = 0;
                lastUpdate = DateTime.Now.Ticks;
            }
        }

        private void WritePlayer(Player pl, RakNet.BitStream stream, Client client)
        {
            Process process = Process.ThisProcess();
            if (pl == null || pl.lastHP == 0)
                return;
            oCNpc player = new oCNpc(process, pl.NPCAddress);
            if (player.VobType != zCVob.VobTypes.Npc)
                return;
            Matrix4 trafo = player.TrafoObjToWorld;
            byte type = 1;

            float[] pos = new float[] { trafo.get(3), trafo.get(7), trafo.get(11), trafo.get(2), trafo.get(6), trafo.get(10) };

            if (new Vec3f(pos).getDistance((Vec3f)pl.pos) > 1000)
                type = 0;

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PlayerStatusMessage);
            stream.Write((byte)type);

            stream.Write(pl.id);

            if (player.FocusVob.Address == 0 || !StaticVars.spawnedPlayerDict.ContainsKey(player.FocusVob.Address))
            {
                stream.Write((int)0);
            }
            else
            {
                stream.Write(StaticVars.spawnedPlayerDict[player.FocusVob.Address].id);
            }

            
            if (type == 1)
            {
                stream.Write(pos[0]); stream.Write(pos[1]); stream.Write(pos[2]);
            }
            stream.Write(pos[3]); stream.Write(pos[4]); stream.Write(pos[5]);
            stream.Write(pl.lastAnimation);
            pl.lastAnimation = 30000;

            pl.pos = new float[] { pos[0], pos[1], pos[2] };

            //Senden
            client.client.Send(stream, RakNet.PacketPriority.IMMEDIATE_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }

        public override void Read(BitStream stream, Packet packet, Client client)
        {
            byte type ;
            int id = 0;
            int focusID = 0;

            stream.Read(out type);
            


            int posLen = 3;
            if (type == 1)
                posLen = 6;

            
            
            float[] pos = new float[posLen];
            short animation = 0;

            stream.Read(out id);
            stream.Read(out focusID);
            for (int i = 0; i < posLen; i++)
                stream.Read(out pos[i]);


            stream.Read(out animation);

            if (Program.Player == null || id == Program.Player.id)
                return;
            if (!StaticVars.AllPlayerDict.ContainsKey(id))
                return;
            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null)
                return;

            

            if(type == 1)
            {
                pl.pos = new float[] { pos[0], pos[1], pos[2] };
                if (!updatePlayerPosition(pl) && !updatePositionList.Contains(pl))
                    updatePositionList.Add(pl);
                
            }

            float[] dir = null;
            if (type == 1)
            {
                dir = new float[] { pos[3], pos[4], pos[5] };
            }
            else
            {
                dir = new float[] { pos[0], pos[1], pos[2] };
            }

            pl.dir = dir;

            if (!pl.isSpawned || pl.NPCAddress == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc player = new oCNpc(process, pl.NPCAddress);

            if (animation != 30000)
            {
                player.GetModel().StartAni(animation, 0);
            }

            SetDirection(pl);
            

            

            
        }


        public static void SetAngle(Player pl, float angle)
        {
            if (pl == null || pl.NPCAddress == 0 || !StaticVars.Ingame)
                return;

            Process process = Process.ThisProcess();
            oCNpc player = new oCNpc(process, pl.NPCAddress);

            player.ResetRotationsWorld();
            player.RotateWorldY((float)angle);

            Matrix4 trafo = player.TrafoObjToWorld;
            pl.dir = new float[] { trafo.get(2), trafo.get(6), trafo.get(10) };
        }


        /// <summary>
        /// Set the direction of the player. The Direction of the player has to be set in pl.dir = float(3)
        /// </summary>
        /// <param name="pl"></param>
        public static void SetDirection(Player pl)
        {
            if (pl == null || pl.NPCAddress == 0 || !StaticVars.Ingame)
                return;


            if (!updatePlayerDir(pl))
                updateDirList.Add(pl);


            //Process process = Process.ThisProcess();
            //oCNpc player = new oCNpc(process, pl.NPCAddress);

            //double angle = (Math.Atan2(pl.dir[0], pl.dir[2]) * (180.0 / Math.PI));

            //player.ResetRotationsWorld();
            //player.RotateWorldY((float)angle);
        }

        public static void SetPosition(Player pl, float[] pos)
        {
            pl.pos = new float[] { pos[0], pos[1], pos[2] };
        }

        public static oCItem GetItemMag(oCNpc player, String item)
        {
            zCArray<oCItem> items = player.MagBook.SpellItems;

            item = item.Trim().ToLower();
            for (int i = 0; i < items.Size; i++)
            {
                if (items.get(i).ObjectName.Value.Trim().ToLower() == item)
                {
                    return items.get(i);
                }
            }
            return null;
        }
        public static int SetActiveSpell(oCNpc player, String spellname)
        {
            Process process = Process.ThisProcess();
            zCArray<oCItem> items = player.MagBook.SpellItems;
            
            spellname = spellname.Trim().ToLower();
            for (int i = 0; i < items.Size; i++)
            {
                if (items.get(i).ObjectName.Value.Trim().ToLower() == spellname)
                {
                    process.Write(i, player.MagBook.Address + 36);
                    return 0;
                }
            }
            return 1;
        }
        public static void Equip(oCNpc player, String itemName)
        {
            zCListSort<oCItem> items = player.Inventory.ItemList;
            int size = items.size();
            itemName = itemName.Trim().ToLower();
            bool found = false;

            for (int i = 0; i < size; i++)
            {
                if (items.get(i).ObjectName.Value.Trim().ToLower() == itemName)
                {
                    player.Equip(items.get(i));
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Process process = Process.ThisProcess();

                zString str= zString.Create(process, itemName);
                oCItem item = player.PutInInv(str, 1);
                str.Dispose();

                player.Equip(item);
            }

        }


        public static void EquipArmor(oCNpc player, String armor)
        {
            Process process = Process.ThisProcess();
            armor = armor.Trim();
            if (armor == "")
            {
                if (player.GetEquippedArmor().Address != 0)
                {
                    player.UnequipItem(player.GetEquippedArmor());
                }
            }
            else
            {
                if (player.GetEquippedArmor().ObjectName.Value.Trim() != armor)
                {
                    zCListSort<oCItem> items = player.Inventory.ItemList;
                    int size = items.size();
                    oCItem item = null;
                    for (int i = 0; i < size; i++)
                    {
                        if (items.get(i).ObjectName.Value.Trim() == armor)
                        {
                            item = items.get(i);
                            break;
                        }
                    }

                    zString str= zString.Create(process, armor.Trim());
                    if (item == null && zCParser.getParser(process).GetIndex(str) != 0)
                    {
                        item = player.PutInInv(str, 1);
                        player.RemoveFromInv(item, item.Amount);
                        player.PutInInv(item);
                    }
                    str.Dispose();
                    if(item != null)
                        player.EquipArmor(item);
                }
            }
        }

        public static void PutInSlot(Process process, oCNpc player, zString slot, String name)
        {
            name = name.Trim().ToLower();
            if (player.GetSlotItem(slot).ObjectName.Value.Trim().ToLower()
                != name)
            {
                if (name.Length == 0 ||
                    player.GetSlotItem(slot).ObjectName.Value.Trim().ToLower().Length != 0)//Kein Item, oder altes Item falsch
                {
                    player.RemoveFromSlot(slot, player.GetSlotItem(slot).Instanz, player.GetSlotItem(slot).Amount);
                }
                if (name.Length > 0)
                {
                    zString str = zString.Create(process, name);
                    int itemid = zCParser.getParser(process).GetIndex(str);
                    str.Dispose();
                    if (itemid != 0)
                    {
                        oCItem itm = oCObjectFactory.GetFactory(process).CreateItem(itemid);
                        zCVob vob = new zCVob(process, itm.Address);
                        player.Equip(itm);
                        player.PutInSlot(slot, vob, 1);
                    }
                }

            }
        }

        public static void EquipWeapon(oCNpc player, String armor)
        {
            Process process = Process.ThisProcess();
            armor = armor.Trim();
            if (armor == "")
            {
                if (player.GetEquippedMeleeWeapon().Address != 0)
                {
                    player.UnequipItem(player.GetEquippedMeleeWeapon());
                }
            }
            else
            {
                if (player.GetEquippedMeleeWeapon().ObjectName.Value.Trim() != armor)
                {
                    zCListSort<oCItem> items = player.Inventory.ItemList;
                    int size = items.size();
                    oCItem item = null;
                    for (int i = 0; i < size; i++)
                    {
                        if (items.get(i).ObjectName.Value.Trim() == armor)
                        {
                            item = items.get(i);
                            break;
                        }
                    }

                    zString str = zString.Create(process, armor.Trim());
                    if (item == null && zCParser.getParser(process).GetIndex(str) != 0)
                        item = player.PutInInv(str, 1);
                    str.Dispose();
                    if(item != null)
                        player.EquipWeapon(item);
                }
            }
        }

        public static void EquipRangeWeapon(oCNpc player, String armor)
        {
            Process process = Process.ThisProcess();
            armor = armor.Trim();
            if (armor == "")
            {
                if (player.GetEquippedRangedWeapon().Address != 0)
                {
                    player.UnequipItem(player.GetEquippedRangedWeapon());
                }
            }
            else
            {
                if (player.GetEquippedRangedWeapon().ObjectName.Value.Trim() != armor)
                {
                    zCListSort<oCItem> items = player.Inventory.ItemList;
                    int size = items.size();
                    oCItem item = null;
                    for (int i = 0; i < size; i++)
                    {
                        if (items.get(i).ObjectName.Value.Trim() == armor)
                        {
                            item = items.get(i);
                            break;
                        }
                    }

                    zString str = zString.Create(process, armor.Trim());
                    if (item == null && zCParser.getParser(process).GetIndex(str) != 0)
                        item = player.PutInInv(str, 1);
                    str.Dispose();
                    if(item != null)
                        player.EquipFarWeapon(item);
                }
            }
        }


    }
}
