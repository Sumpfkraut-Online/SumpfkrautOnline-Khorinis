using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using RakNet;
using Network;
using Injection;
using Gothic.zTypes;
using WinApi;
using System.Windows.Forms;
using GMP.Logger;
using GMP.Injection.Synch;
using Network.Savings;
using GMP.Helper;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class MobSynch : Message
    {
        static List<oCMobInter> lastInterMobUsed = new List<oCMobInter>();
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id = 0;

            byte type = 0;
            int vobtype = 0;
            String name = "";
            float[] pos = new float[3];
            String x; String y;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out vobtype);
            stream.Read(out name);
            stream.Read(out x);
            stream.Read(out y);
     
            for (int i = 0; i < 3; i++)
                stream.Read(out pos[i]);

            if (Program.Player == null || id == Program.Player.id)
                return;

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null || !pl.isSpawned)
                return;

            //Suchen und StartInteract
            Process process = Process.ThisProcess();


            //List<zCVob> namedList = new List<zCVob>();

            oCMobInter currMobInter = null;
            zCVob vob = ObjectHelper.findVob(name, pos, (zCVob.VobTypes)vobtype, World.MobInterMinDistance);
            currMobInter = new oCMobInter(process, vob.Address);

            if (currMobInter != null && currMobInter.Address != 0)
            {
                lastInterMobUsed.Add(currMobInter);
                if (lastInterMobUsed.Count > 10)
                    lastInterMobUsed.RemoveAt(0);
                if (type == 1)
                    currMobInter.StartInteraction(new oCNpc(process, pl.NPCAddress));
                else if (type == 2)
                    currMobInter.StopInteraction(new oCNpc(process, pl.NPCAddress));
                else if (type == 3)
                    currMobInter.StartStateChange(new oCNpc(process, pl.NPCAddress), Convert.ToInt32(x), Convert.ToInt32(y));
                else if (type == 4)
                    new oCMobLockable(process, currMobInter.Address).PickLock(new oCNpc(process, pl.NPCAddress), (char)Convert.ToInt32(x));
                else if (type == 5)
                    new oCMobLockable(process, currMobInter.Address).SetLocked(0);
                else if (type == 6)
                {
                    if (Convert.ToInt32(x) == 1)
                    {
                        new oCMobLockable(process, currMobInter.Address).SetLocked(1);
                    }
                }
                else if (type == 7)
                    currMobInter.SetMobBodyState(new oCNpc(process, pl.NPCAddress));
                else if (type == 8)
                {
                    Animation.startAnimEnabled = true;
                    currMobInter.GetModel().StartAni(Convert.ToInt32(x), Convert.ToInt32(y));
                }
                else if (type == 9)
                {
                    currMobInter.GetModel().StopAni(Convert.ToInt32(x));
                }
                else if (type == 10)
                {
                    zString str = zString.Create(process, x);
                    currMobInter.CallOnStateFunc(new oCNpc(process, pl.NPCAddress), zCParser.getParser(process).GetIndex(str));
                    str.Dispose();
                }
                else if (type == 11)
                {
                    zCTree<zCVob> vobtree = oCGame.Game(process).World.GlobalVobTree.FirstChild;
                    do
                    {
                        if (vobtree.Data.ObjectName.Value.Trim().ToLower() == x.Trim().ToLower())
                        {
                            currMobInter.OnTrigger(vobtree.Data, new zCVob(process, pl.NPCAddress));
                            break;
                        }
                    } while ((vobtree = vobtree.Next).Address != 0);
                }
                else if (type == 12)
                {
                    zCTree<zCVob> vobtree = oCGame.Game(process).World.GlobalVobTree.FirstChild;
                    do
                    {
                        if (vobtree.Data.ObjectName.Value.Trim().ToLower() == x.Trim().ToLower())
                        {
                            currMobInter.OnUnTrigger(vobtree.Data, new zCVob(process, pl.NPCAddress));
                            break;
                        }
                    } while ((vobtree = vobtree.Next).Address != 0);
                }
                else if (type == 13)
                {

                    oCMobContainer mobCont = new oCMobContainer(process, currMobInter.Address);
                    int amount = Convert.ToInt32(y);
                    oCItem item = null;
                    zCListSort<oCItem> itemList = mobCont.ItemList;

                    pl.InsertItem(new item() { code = x, Amount = amount });
                    InventoryHelper.InsertItem(pl, x, amount);

                    do
                    {
                        if (itemList.Data.ObjectName.Value.Trim().ToLower() == x.ToLower().Trim())
                            item = itemList.Data;
                    } while ((itemList = itemList.Next).Address != 0);

                    if (item == null)
                        return;

                    if (item.Amount - amount <= 0)
                        mobCont.Remove(item, amount);
                    else
                        item.Amount = item.Amount - amount;
                }
                else if (type == 14)
                {
                    oCMobContainer mobCont = new oCMobContainer(process, currMobInter.Address);
                    zString iteminstance = zString.Create(process, x);
                    int itemid = zCParser.getParser(process).GetIndex(iteminstance);
                    iteminstance.Dispose();
                    int amount = Convert.ToInt32(y);

                    pl.RemoveItem(new item() { code = x, Amount = amount });
                    InventoryHelper.RemoveItem(pl, x, amount);

                    //Altes Item suchen
                    oCItem item = null;
                    zCListSort<oCItem> itemList = mobCont.ItemList;
                    do
                    {
                        if (itemList.Data.ObjectName.Value.Trim().ToLower() == x.ToLower().Trim())
                            item = itemList.Data;
                    } while ((itemList = itemList.Next).Address != 0);


                    if (item == null || item.MultiSlot() == 0)
                    {
                        item = oCObjectFactory.GetFactory(process).CreateItem(itemid);
                        mobCont.Insert(item);
                        item.Amount = amount;
                    }
                    else
                        item.Amount = item.Amount + amount;
                }
            }
            else
            {
                //MessageBox.Show("MobNotFound!");
            }

        }

        public void Write(RakNet.BitStream stream, Client client, byte type, oCMobInter mobInter, String x, String y)
        {
            if (mobInter == null || mobInter.Address == 0 )
                return;
            if (Program.Player == null)
                return;
            
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.MobSynch);
            stream.Write(Program.Player.id);
            stream.Write(type);
            stream.Write((int)mobInter.VobType);
            stream.Write(ObjectHelper.getName(mobInter));
            stream.Write(x);
            stream.Write(y);
            //stream.Write(mobInter.GetModel().GetVisualName().Value);
            stream.Write(mobInter.TrafoObjToWorld.get(3));
            stream.Write(mobInter.TrafoObjToWorld.get(7));
            stream.Write(mobInter.TrafoObjToWorld.get(11));

            client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.MobSynch))
                StaticVars.sStats[(int)NetWorkIDS.MobSynch] = 0;
            StaticVars.sStats[(int)NetWorkIDS.MobSynch] += 1;
        }
    }
}
