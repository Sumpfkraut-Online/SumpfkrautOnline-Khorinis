using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;
using RakNet;
using Network;
using Injection;
using System.Windows.Forms;
using GMP.Helper;
using GMP.Modules;

namespace GMP.Net.Messages
{
    //public class InventorySynch : Message
    //{
    //    class itemToitem
    //    {
    //        public oCItem now;
    //        public item last;
    //        public int function;
    //        public int amount;
    //    }

    //    static int npcid;
    //    public override void Write(RakNet.BitStream stream, Client client)
    //    {
    //        Process process = Process.ThisProcess();

    //        for (int x = npcid; x < npcid + 5; x++)
    //        {
    //            if (x >= Program.playerList.Count)
    //                break;
    //            Player pl = Program.playerList[x];

    //            if(!pl.isSpawned)
    //                continue;
    //            oCNpc player = new oCNpc(process, pl.NPCAddress);
    //            if (player.Address == 0)
    //                continue;

    //            if (InventoryHelper.isEqual(player, pl))
    //                continue;

                

    //            List<oCItem> activeItemList = InventoryHelper.GetListFromSortList(player.Inventory.ItemList);

    //            stream.Reset();
    //            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
    //            stream.Write((byte)NetWorkIDS.InventorySynch);
    //            stream.Write(Program.Player.id);
    //            stream.Write(pl.id);
    //            stream.Write(TimeManager.conv_Date2Timestam());
    //            stream.Write(activeItemList.Count);

    //            List<item> items = new List<item>();
    //            for (int i = 0; i < activeItemList.Count; i++)
    //            {
    //                stream.Write(activeItemList[i].ObjectName.Value);
    //                stream.Write(activeItemList[i].Amount);

    //                items.Add(new item() { code = activeItemList[i].ObjectName.Value.Trim().ToLower(), Amount = activeItemList[i].Amount });
    //            }
    //            pl.itemList = items;

    //            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
    //        }


    //        npcid += 5;
    //        if (StaticVars.npcControlList.Count <= npcid)
    //            npcid = 0;
    //    }


    //    public override void Read(BitStream stream, Packet packet, Client client)
    //    {
    //        int senderid;
    //        int invid;
    //        int count;
    //        long lastSendet;

    //        stream.Read(out senderid);
    //        stream.Read(out invid);
    //        stream.Read(out lastSendet);
    //        stream.Read(out count);


    //        Player pl = Player.getPlayerSort(invid, Program.playerList);
    //        Player sender = Player.getPlayerSort(senderid, Program.playerList);
    //        if (pl == null || sender == null )
    //            return;

    //        if (pl.lastSendet[(int)Player.SendTime.itemList] > lastSendet)
    //            return;

    //        pl.lastSendet[(int)Player.SendTime.itemList] = lastSendet;


            

    //        List<item> items = new List<item>();
    //        for (int i = 0; i < count; i++)
    //        {
    //            string code; int amount;
    //            stream.Read(out code);
    //            stream.Read(out amount);

    //            items.Add(new item() { code = code, Amount = amount });
    //        }

    //        pl.itemList = items;
            
    //        if (sender.id == Program.Player.id || !pl.isSpawned)
    //            return;

    //        Process process = Process.ThisProcess();
    //        if (InventoryHelper.isEqual(new oCNpc(process, pl.NPCAddress), pl))
    //            return;

    //        InventoryHelper.ChangeInventory(pl);
    //        //InventoryHelper.RemoveInventory(pl);
    //        //InventoryHelper.AddInventory(pl);

    //    }



        

    //}
}
