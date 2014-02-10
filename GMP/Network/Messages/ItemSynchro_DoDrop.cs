using System;
using System.Collections.Generic;
using System.Text;
using Injection;
using Network;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using RakNet;
using System.Windows.Forms;
using Network.Types;
using GMP.Helper;
using Network.Savings;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class ItemSynchro_DoDrop : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id = 0;
            String mode;
            byte type = 0;
            int amount = 0;

            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out mode);
            stream.Read(out amount);

            if (Program.Player == null || id == Program.Player.id)
                return;

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null || !pl.isSpawned)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            if (type == 1)
            {
                zString str = zString.Create(process, mode.Trim());
                oCItem item = npc.IsInInv(str, amount);

                if (item == null || item.Address == 0)
                    item = npc.PutInInv(str, amount);
                str.Dispose();

                npc.DoDropVob(item);
                npc.RemoveFromInv(item, item.Amount);
            }
            else if (type == 2)
            {
                zCVob currItem = null;

                if (npc.FocusVob.Address != 00 && npc.FocusVob.ObjectName.Value.Trim() == mode.Trim())
                    currItem = npc.FocusVob;

                if (currItem == null)
                {
                    #region oldItemSearch
                    //zCListSort<zCVob> itemList = oCGame.Game(process).World.VobList;
                    //int size = itemList.size();
                    //List<zCVob> namedList = new List<zCVob>();
                    //zCListSort<zCVob> listTemp = itemList;

                    //do 
                    //{
                    //    zCVob vob = listTemp.Data;

                    //    if (vob == null || vob.VobType != zCVob.VobTypes.Item)
                    //        continue;
                    //    if (vob.ObjectName.Value.Trim() == mode.Trim())
                    //        namedList.Add(vob);
                    //} while ((listTemp = listTemp.Next).Address != 0);

                    
                    //float minDistance = 0;
                    //float[] npcPos = new float[] { npc.TrafoObjToWorld.get(3), npc.TrafoObjToWorld.get(7), npc.TrafoObjToWorld.get(11) };
                    //foreach (zCVob item in namedList)
                    //{
                    //    float[] itemPos = new float[] { item.TrafoObjToWorld.get(3), item.TrafoObjToWorld.get(7), item.TrafoObjToWorld.get(11) };
                    //    float[] relPos = new float[] { itemPos[0] - npcPos[0], itemPos[1] - npcPos[1], itemPos[2] - npcPos[2] };
                    //    float distance = (float)Math.Sqrt((double)relPos[0] * (double)relPos[0] + (double)relPos[1] * (double)relPos[1] + (double)relPos[2] * (double)relPos[2]);
                    //    if (currItem == null || minDistance > distance)
                    //    {
                    //        currItem = item;
                    //        minDistance = distance;
                    //    }
                    //}
                    #endregion
                    //zCListSort<zCVob> itemList = oCGame.Game(process).World.VobList;
                    //int size = itemList.size();
                    //List<zCVob> namedList = new List<zCVob>();
                    //zCListSort<zCVob> listTemp = itemList;

                    //do
                    //{
                    //    zCVob vob = listTemp.Data;

                    //    if (vob == null || vob.Address == 0 || vob.VobType != zCVob.VobTypes.Item)
                    //        continue;
                    //    if (vob.ObjectName.Value.Trim() == mode.Trim() && new oCItem(process, vob.Address).Amount == amount)
                    //        namedList.Add(vob);
                    //} while ((listTemp = listTemp.Next).Address != 0);


                    //float minDistance = 0;
                    //Vec3f npcPos = new Vec3f(new float[] { npc.TrafoObjToWorld.get(3), npc.TrafoObjToWorld.get(7), npc.TrafoObjToWorld.get(11) });
                    //foreach (zCVob item in namedList)
                    //{
                    //    Vec3f itemPos = new Vec3f(new float[] { item.TrafoObjToWorld.get(3), item.TrafoObjToWorld.get(7), item.TrafoObjToWorld.get(11) });
                    //    float distance = npcPos.getDistance(itemPos);
                    //    if (currItem == null || minDistance > distance)
                    //    {
                    //        currItem = item;
                    //        minDistance = distance;
                    //    }
                    //}
                    currItem = ObjectHelper.getNearestItem(mode, new float[] { npc.TrafoObjToWorld.get(3), npc.TrafoObjToWorld.get(7), npc.TrafoObjToWorld.get(11) }, 
                        zCVob.VobTypes.Item, World.ItemsMinDistance, amount);
                }


                if (currItem == null || currItem.Address == 0)
                {
                    zERROR.GetZErr(process).Report(2, 'G', "Couldn't find the Item! Items are asynchron now!", 0, "ItemSyncrho_DoDrop.cs", 0);
                    return;
                }
                npc.DoTakeVob(currItem);
                //npc.PutInInv(new oCItem(process, currItem.Address));
                //npc.RemoveVobFromWorld();
                //world.RemoveVob(currItem);
            }
            
        }


        public void Write(RakNet.BitStream stream, Client client, String mode, int amount, byte type)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ItemSynchro_DoDrop);
            stream.Write(Program.Player.id);
            stream.Write(type);
            stream.Write(mode);
            stream.Write(amount);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.ItemSynchro_DoDrop))
                StaticVars.sStats[(int)NetWorkIDS.ItemSynchro_DoDrop] = 0;
            StaticVars.sStats[(int)NetWorkIDS.ItemSynchro_DoDrop] += 1;
        }
    }
}
