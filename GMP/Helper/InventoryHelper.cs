using System;
using System.Collections.Generic;
using System.Text;
using Network;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GMP.Helper
{
    public class InventoryHelper
    {
        public static void RemoveItem(Player pl, String item, int Amount)
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            if (npc.Address == 0)
                return;

            oCItem itemPrev = null;
            oCItem itemSec = null;

            zCListSort<oCItem> itemListS = npc.Inventory.ItemList;
            do
            {
                if (itemListS.Data.ObjectName.Value.Trim().ToLower() == item.ToLower().Trim() && itemListS.Data.Amount < Amount)
                    itemSec = itemListS.Data;
                else if (itemListS.Data.ObjectName.Value.Trim().ToLower() == item.ToLower().Trim() && itemListS.Data.Amount >= Amount)
                    itemPrev = itemListS.Data;

            } while ((itemListS = itemListS.Next).Address != 0);

            if (itemPrev != null)
                npc.RemoveFromInv(itemPrev, Amount);
            else if (itemSec != null)
                npc.RemoveFromInv(itemSec, Amount);
        }

        public static void InsertItem(Player pl, String item, int Amount)
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            if (npc.Address == 0)
                return;

            zString str = zString.Create(process, item);
            int id = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            if (id == 0)
                return;
            oCItem itm = npc.PutInInv(id, Amount);
            npc.RemoveFromInv(itm, Amount);
            npc.PutInInv(itm);

        }


        public static void RemoveInventory(Player pl)
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            if (npc.Address == 0)
                return;


            List<oCItem> itemList = new List<oCItem>();
            zCListSort<oCItem> itemListS = npc.Inventory.ItemList;
            do
            {
                itemList.Add(itemListS.Data);
            } while ((itemListS = itemListS.Next).Address != 0);

            foreach (oCItem itm in itemList)
                npc.RemoveFromInv(itm, itm.Amount);

            pl.itemList.Clear();
        }

        public static void AddInventory(Player pl)
        {
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            if (npc.Address == 0)
                return;

            foreach (item itm in pl.itemList)
            {
                zString str = zString.Create(process, itm.code);
                int id = zCParser.getParser(process).GetIndex(str);
                str.Dispose();

                if (id == 0)
                    continue;
                oCItem item = npc.PutInInv(id, itm.Amount);
                npc.RemoveFromInv(item, item.Amount);
                npc.PutInInv(item);
            }
        }

        public static void InsertToInventory(Player pl, List<item> itemList)
        {
            Process process = Process.ThisProcess();

            oCNpc player = new oCNpc(process, pl.NPCAddress);

            foreach (item lItem in itemList)
            {
                zString str = zString.Create(process, lItem.code);
                if (zCParser.getParser(process).GetIndex(str) == 0)
                    continue;

                oCItem itm = player.PutInInv(str, lItem.Amount);
                player.RemoveFromInv(itm, itm.Amount);
                player.PutInInv(itm);
                str.Dispose();

                pl.itemList.Add(lItem);
            }

        }

        

        #region old

        //public static void ChangeInventory(Player pl)
        //{
        //    Process process = Process.ThisProcess();

        //    oCNpc player = new oCNpc(process, pl.NPCAddress);

        //    //Listen füllen
        //    List<oCItem> activeItemList = GetListFromSortList(player.Inventory.ItemList);
        //    List<item> lastItemList = new List<item>();

        //    foreach (item it in pl.itemList)
        //        lastItemList.Add(it);


        //    //Listen vergleichen und gleiche löschen, sind beide listen leer, waren sie identisch
        //    item[] lArr = lastItemList.ToArray();
        //    foreach (item lItem in lArr)
        //    {
        //        foreach (oCItem aItm in activeItemList)
        //        {
        //            if (lItem.code.Trim().ToLower() == aItm.ObjectName.Value.Trim().ToLower()
        //                && lItem.Amount == aItm.Amount)
        //            {

        //                activeItemList.Remove(aItm);
        //                lastItemList.Remove(lItem);
        //                break;
        //            }
        //        }
        //    }
        //    //Rest ist unterschiedlich:

        //    //Amount änderungen
        //    lArr = lastItemList.ToArray();
        //    foreach (item lItem in lArr)
        //    {
        //        foreach (oCItem aItm in activeItemList)
        //        {
        //            if (lItem.code.Trim().ToLower() == aItm.ObjectName.Value.Trim().ToLower())
        //            {
        //                aItm.Amount = lItem.Amount;
        //                activeItemList.Remove(aItm);
        //                lastItemList.Remove(lItem);
        //                break;
        //            }
        //        }
        //    }

        //    //Entfernte
        //    oCItem[] aArr = activeItemList.ToArray();
        //    foreach (oCItem aItm in aArr)
        //    {
        //        player.RemoveFromInv(aItm, aItm.Amount);
        //        activeItemList.Remove(aItm);
        //    }

        //    //Hinzugefügte
        //    lArr = lastItemList.ToArray();
        //    foreach (item lItem in lArr)
        //    {
        //        zString str = zString.Create(process, lItem.code);
        //        if (zCParser.getParser(process).GetIndex(str) != 0)
        //        {
        //            oCItem itm = player.PutInInv(str, lItem.Amount);
        //            player.RemoveFromInv(itm, itm.Amount);
        //            player.PutInInv(itm);
        //        }
        //        str.Dispose();
        //    }



        //}

        //public static bool isEqual(oCNpc player, Player pl)
        //{
        //    //Listen füllen
        //    List<oCItem> activeItemList = GetListFromSortList(player.Inventory.ItemList);
        //    List<item> lastItemList = new List<item>();
            
        //    foreach (item it in pl.itemList)
        //        lastItemList.Add(it);


        //    //Listen vergleichen und gleiche löschen, sind beide listen leer, waren sie identisch
        //    item[] lArr = lastItemList.ToArray();
        //    foreach (item lItem in lArr)
        //    {
        //        foreach (oCItem aItm in activeItemList)
        //        {
        //            if (lItem.code.Trim().ToLower() == aItm.ObjectName.Value.Trim().ToLower()
        //                && lItem.Amount == aItm.Amount)
        //            {

        //                activeItemList.Remove(aItm);
        //                lastItemList.Remove(lItem);
        //                break;
        //            }
        //        }
        //    }

        //    if (activeItemList.Count == 0 && lastItemList.Count == 0)
        //        return true;
        //    else
        //        return false;
        //}
        #endregion
        public static List<oCItem> GetListFromSortList(zCListSort<oCItem> itemlist)
        {
            List<oCItem> activeItemList = new List<oCItem>();
            do
            {
                if (itemlist.Data.Address == 0)
                    continue;
                activeItemList.Add(itemlist.Data);
            } while ((itemlist = itemlist.Next).Address != 0);

            return activeItemList;
        }


    }
}
