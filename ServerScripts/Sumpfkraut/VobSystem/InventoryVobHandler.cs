using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Server.Scripts.Sumpfkraut.Database;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{

    /**
     *   Class which is used for inventory-manipulation (e.g. for transfering items in a trade).
     */
    public class InventoryVobHandler : GUC.Utilities.ExtendedObject
    {

        new public static readonly String _staticName = "InventoryVobHandler (static)";

        //private static Dictionary<String, Database.SQLiteGetTypeEnum> itemInInvTableDict = 
        //    Database.DBTables.InstTableDict[Database.InstTableEnum.ItemInInventory_inst];

        //private static Dictionary<String, Database.SQLiteGetTypeEnum> itemInContTableDict = 
        //    Database.DBTables.InstTableDict[Database.InstTableEnum.ItemInContainer_inst];

        //// referencing can be done without worrying about consistancy because 
        //// InstTableDict won't be changed by any part of the program
        //private static Dictionary<String, Database.SQLiteGetTypeEnum> itemInstTableDict = 
        //    Database.DBTables.InstTableDict[Database.InstTableEnum.Item_inst];

        //// stores ItemInst-objects with their ID-attribute as key
        //// used for faster and cleaner access to these items
        //private static Dictionary<int, ItemInst> itemInInvDict = new Dictionary<int, ItemInst>();


        //new public static readonly String _staticName = "InventoryVobHandler (static)";
        //new protected String _objName = "InventoryVobHandler (default)";

        //private static object playerInvLock = new object();
        //private static object containerInvLock = new object();



        public InventoryVobHandler ()
        {
            SetObjName("InventoryVobHandler (default)");
        }

        
        
        ///**
        // *   Try to transfer lists of items given by their ItemDef-objects as types and with respective amounts
        // *   between 2 npcs.
        // *   The trade will take place in multiple steps, where items transfer from one player to the other is
        // *   done by applying the inventory changes to the donator and latter to the acceptor. This process is
        // *   done for each player. If one of these transfer steps fails (e.g. items are missing in inventories),
        // *   the whole trade will be rolled back and negated. The order of npcs in the transfer does not matter.
        // *   Negative amounts will result in an attempt to pull those items from the other trade partner instead
        // *   of giving them to him/her.
        // *   @param playerID_1 is the ID of the NPCInst-object which represents the first npc in transfer
        // *   @param playerID_2 is the ID of the NPCInst-object which represents the first npc in transfer
        // *   @param itemDefIDs_1 are the IDs of ItemDef-objects which should be traded from npc 1 to npc 2
        // *   @param itemDefIDs_2 are the IDs of ItemDef-objects which should be traded from npc 2 to npc 1
        // *   @param amounts_1 are the respective amounts used in conjunction with itemDefIDs_1
        // *   @param amounts_2 are the respective amounts used in conjunction with itemDefIDs_2
        // */
        //public static bool TransferItems (int playerID_1, int playerID_2, 
        //    int[] itemDefIDs_1, int[] itemDefIDs_2, int[] amounts_1, int[] amounts_2)
        //{
        //    bool transP1ToP2 = TransferItems(playerID_1, playerID_2, itemDefIDs_1, amounts_1);
        //    if (!transP1ToP2)
        //    {
        //        Log.Logger.logWarning("TransferItems: Transfer from player 1 to player 2 failed!"
        //            + " No items will be traded and traded items will be transfered back to their original owners: "
        //            + "playerID_1=" + playerID_1 + ", " + "playerID_2=" + playerID_2);
        //        return false;
        //    }
        //    bool transP2ToP1 = TransferItems(playerID_2, playerID_1, itemDefIDs_2, amounts_2);
        //    if (!transP2ToP1)
        //    {
        //        Log.Logger.logWarning("TransferItems: Transfer from player 2 to player 1 failed!"
        //            + " No items will be traded and traded items will be transfered back to their original owners: "
        //            + "playerID_1=" + playerID_1 + ", " + "playerID_2=" + playerID_2);
        //        transP1ToP2 = TransferItems(playerID_1, playerID_2, itemDefIDs_1, amounts_1);
        //        if (!transP1ToP2)
        //        {
        //            string[] failedItems = new string[itemDefIDs_1.Length];
        //            for (int i = 0; i < itemDefIDs_1.Length; i++)
        //            {
        //                failedItems[i] = itemDefIDs_1[i].ToString() + "=" + amounts_1[i].ToString();
        //            }
        //            Log.Logger.logWarning("TransferItems: Retransferring itmes from player 2 to player 1 after "
        //                + "error in trade failed, too. The following items (by id) with their respective amounts "
        //                + "must be transferred back manually:"
        //                + string.Join(", ", failedItems));
        //        }
        //        return false;
        //    }
        //    return true;
        //}

        ///**
        // *   Transfer a list of items, according to their ItemDef with given amounts from the 
        // *   donator- to acceptor-npcs inventory.
        // *   This is a transfer in one direction. For exchanging items between both inventories,
        // *   use the higher-level-TransferItems-method instead. The transfer will be negated if one side
        // *   of the transfer does not have the necessary items or something else went wrong.
        // *   @param donatorID is the ID of the NPCInst-object which represents the donator of this transfer
        // *   @param acceptorID is the ID of the NPCInst-object which represents the acceptor of this transfer
        // *   @param itemDefIDs are the IDs of ItemDef-objects which should be translated into a trade of actual Items
        // *   @param amounts are the respective amounts of items, that should be transferred (if possible)
        // */
        //public static bool TransferItems (int donatorID, int acceptorID, int[] itemDefIDs, int[] amounts)
        //{
        //    bool giveItemsDon = RemoveItems(donatorID, itemDefIDs, amounts);
        //    if (!giveItemsDon)
        //    {
        //        Log.Logger.logWarning("TransferItems: Couldn't remove items from donators inventory: "
        //            + "donatorID=" + donatorID);
        //        return false;
        //    }
        //    bool giveItemsAccept = GiveItems(acceptorID, itemDefIDs, amounts);
        //    if (!giveItemsAccept)
        //    {
        //        // give back the removed items to the original donator
        //        giveItemsDon = GiveItems(donatorID, itemDefIDs, amounts);
        //        if (!giveItemsDon)
        //        {
        //            Log.Logger.logWarning("TransferItems: Failed to give back donators inventory-items after failed "
        //                + "attempt to give these items to the acceptor-inventory: " 
        //                + "donatorID=" + donatorID + ", acceptorID=" + acceptorID);
        //        }
        //        return false;
        //    }
        //    return true;
        //}

        ///**
        // *   Revert verson of GiveItems-method which applies the negative amounts of items, given by ItemDef-ids, on
        // *   a npc-inventory.
        // *   The sign of negative amounts will be reverted, too, which results in a donation of items to the npc-
        // *   inventory. The method is simply for convenience but can be easily replaced by GiveItems-method because
        // *   this method is called in the end anyway, provided negative amoutns are used to remove items.
        // *   @param npcInstID is the ID of the NPCInst-object for which the inventory will be manipulated
        // *   @param itemDefIDs are the IDs of ItemDef-objects which should be removed or given to the 
        // *   @param amounts are the respective amounts used in conjunction with itemDefIDs
        // */
        //public static bool RemoveItems (int npcInstID, int[] itemDefIDs, int[] amounts)
        //{
        //    for (int i = 0; i < amounts.Length; i++)
        //    {
        //        amounts[i] = -amounts[i];
        //    }
        //    return GiveItems(npcInstID, itemDefIDs, amounts);
        //}

        ///**
        // *   Give or remove items, defined by ItemDef-object-ids, with respective amounts to/from a npcs inventory.
        // *   Negative amounts will result in item-removal, positive ones in a donation of items to the inventory.
        // *   In case of item-removal, the inventory manipulation will only take place if enough items of the given
        // *   type are in posession of the npc beforehand.
        // *   @param npcInstID is the ID of the NPCInst-object for which the inventory will be manipulated
        // *   @param itemDefIDs are the IDs of ItemDef-objects which should be removed or given to the 
        // *   @param amounts are the respective amounts used in conjunction with itemDefIDs
        // */
        //public static bool GiveItems (int npcInstID, int[] itemDefIDs, int[] amounts)
        //{
        //    int[] errRealAmounts;
        //    if (!HasItems(npcInstID, itemDefIDs, amounts, out errRealAmounts))
        //    {
        //        StringBuilder warningSB = new StringBuilder();
        //        warningSB.Append("GiveItems: NPC with npcInstID=" + npcInstID);
        //        warningSB.Append(" has insufficient amounts of specified itemDefIDs: ");

        //        bool isFirst = true;
        //        for (int i = 0; i < itemDefIDs.Length; i++)
        //        {
        //            if (!isFirst)
        //            {
        //                warningSB.Append(", ");
        //            }
        //            else
        //            {
        //                isFirst = false;
        //            }
        //            warningSB.Append(itemDefIDs[i]);
        //            warningSB.Append("=");
        //            warningSB.Append(errRealAmounts[i]);
        //            warningSB.Append("|");
        //            warningSB.Append(amounts[i]);
        //        }

        //        Log.Logger.logWarning(warningSB.ToString());
        //        return false;
        //    }

        //    DateTime now;
        //    string nowString;

        //    // for creating new item in general Item_inst-table
        //    StringBuilder itemInst_NewItemsSB = new StringBuilder();
        //    itemInst_NewItemsSB.Append("INSERT INTO `Item_inst` (`ItemDefID`,`Amount`,`ChangeDate`,`CreationDate`)");
        //    itemInst_NewItemsSB.Append(" VALUES ");

        //    // for referencing a newly created item in the npcs inventory
        //    StringBuilder inv_NewItemsSB = new StringBuilder();
        //    inv_NewItemsSB.Append("INSERT INTO `ItemInInventory_inst` (`NPCInstID`,`ItemInstID`,`ChangeDate`,`CreationDate`)");
        //    inv_NewItemsSB.Append(" VALUES ");

        //    bool itemInst_NewItemsIsFirst = true;

        //    // for updating the Amount and the ChangeDate (CD) if item-entry is already existing
        //    StringBuilder itemInst_UpdateItemsSB = new StringBuilder();
        //    itemInst_UpdateItemsSB.Append("UPDATE `Item_inst` SET `Amount`=CASE ");
        //    StringBuilder itemInst_UpdateItemsCDSB = new StringBuilder();
        //    itemInst_UpdateItemsCDSB.Append("`ChangeDate`=CASE ");

        //    bool itemInst_UpdateItemsIsFirst = true;

        //    for (int i = 0; i < itemDefIDs.Length; i++)
        //    {
        //        now = DateTime.Now;
        //        nowString = Sumpfkraut.Utilities.DateTimeUtil.DateTimeToString(now);

        //        if (errRealAmounts[i] == 0)
        //        {
        //            if (amounts[i] > 0)
        //            {
        //                // only add a new row in Item_inst if the amount makes sense (0 or <0 adds nothing)
        //                if (!itemInst_NewItemsIsFirst)
        //                {
        //                    // use the comma to seperate from preveous entries
        //                    itemInst_NewItemsSB.Append(",");
        //                    inv_NewItemsSB.Append(",");
        //                }
        //                else
        //                {
        //                    itemInst_NewItemsIsFirst = false;
        //                }

        //                itemInst_NewItemsSB.Append("(");
        //                itemInst_NewItemsSB.Append(itemDefIDs[i]);
        //                itemInst_NewItemsSB.Append(",");
        //                itemInst_NewItemsSB.Append(amounts[i]);
        //                itemInst_NewItemsSB.Append(",");
        //                itemInst_NewItemsSB.Append("\"");
        //                itemInst_NewItemsSB.Append(nowString);
        //                itemInst_NewItemsSB.Append("\"");
        //                itemInst_NewItemsSB.Append(",");
        //                itemInst_NewItemsSB.Append("\"");
        //                itemInst_NewItemsSB.Append(nowString);
        //                itemInst_NewItemsSB.Append("\"");
        //                itemInst_NewItemsSB.Append(")");

        //                inv_NewItemsSB.Append("(");
        //                //inv_NewItemsSB.Append();
        //                // ...
        //                inv_NewItemsSB.Append("\"");
        //                inv_NewItemsSB.Append(nowString);
        //                inv_NewItemsSB.Append("\"");
        //                inv_NewItemsSB.Append(",");
        //                inv_NewItemsSB.Append("\"");
        //                inv_NewItemsSB.Append(nowString);
        //                inv_NewItemsSB.Append("\"");
        //                inv_NewItemsSB.Append(")");
        //            }
        //        }
        //        else if (errRealAmounts[i] > 0)
        //        {
        //            // the item(-stack) is aready represented by an existing row and needs to be updated accordingly
        //            if (!itemInst_UpdateItemsIsFirst)
        //            {
        //                // use the comma to seperate from preveous entries
        //                itemInst_UpdateItemsSB.Append(",");
        //                itemInst_UpdateItemsCDSB.Append(",");
        //            }
        //            else
        //            {
        //                itemInst_UpdateItemsIsFirst = false;
        //            }

        //            itemInst_UpdateItemsSB.Append("WHEN `ID`=");
        //            itemInst_UpdateItemsSB.Append(itemDefIDs[i]);
        //            itemInst_UpdateItemsSB.Append(" THEN `Amount`");
        //            if (amounts[i] >= 0)
        //            {
        //                itemInst_UpdateItemsSB.Append("+");
        //            }
        //            itemInst_UpdateItemsSB.Append(amounts[i]);

        //            itemInst_UpdateItemsCDSB.Append("WHEN `ID`=");
        //            itemInst_UpdateItemsCDSB.Append(itemDefIDs[i]);
        //            itemInst_UpdateItemsSB.Append(" THEN `ChangeDate`=`ChangeDate`");
        //        }
        //        else
        //        {
        //            // shouldn't happen because HasItems returns false beforehand 
        //            // if there are not enough items on substraction
        //            Log.Logger.logError("GiveItems: Negative original amount-value in row with ID=" + itemDefIDs[i]);
        //            return false;
        //        }
        //    }

        //    itemInst_NewItemsSB.Append(";");
        //    inv_NewItemsSB.Append(";");

        //    itemInst_UpdateItemsSB.Append(" END ");
        //    itemInst_UpdateItemsCDSB.Append(" END");
        //    itemInst_UpdateItemsSB.Append(itemInst_UpdateItemsCDSB.ToString());
        //    itemInst_UpdateItemsSB.Append(";");

        //    Database.DBReader.SaveToDB(itemInst_NewItemsSB.ToString());
        //    Database.DBReader.SaveToDB(inv_NewItemsSB.ToString());
        //    Database.DBReader.SaveToDB(itemInst_UpdateItemsSB.ToString());

        //    // after updating the database, also change the actual ingame-inventory


        //    return true;
        //}

        //private static bool GiveItemsIngame (int npcInstID, int[] itemDefIDs, int[] amounts)
        //{
        //     for (int i = 0; i < itemDefIDs.Length; i++)
        //    {

        //    }

        //    return true;
        //}

        ///**
        // *   Checks (boolean result) if the inventory of npc with provided ID has the amounts of items of given types 
        // *   (by ItemDef-IDs).
        // *   It solely uses HasItemDefinitions-method at the moment and, therefore, can be used only when providing
        // *   ItemDef-ids. Maybe a functionality will be implemented to check for ItemInst-ids as well.
        // *   @param npcInstID is the ID of the NPCInst-object for which the inventory will be checked
        // *   @param itemRelatedIDs are the IDs of ItemDef-objects which should watched for
        // *   @param amounts are the respective amounts used in conjunction with itemDefIDs
        // *   @param errRealAmounts will store the actual amounts of items found by the method as additional information
        // */
        //public static bool HasItems (int npcInstID, int[] itemRelatedIDs, int[] amounts, out int[] errRealAmounts)
        //{
        //    return HasItemDefenitions(npcInstID, itemRelatedIDs, amounts, out errRealAmounts);
        //}

        ///**
        // *   Checks (boolean result) if the inventory of npc with provided ID has the amounts of items of given types 
        // *   (by ItemDef-IDs).
        // *   @param npcInstID is the ID of the NPCInst-object for which the inventory will be checked
        // *   @param itemDefIDs are the IDs of ItemDef-objects which should watched for
        // *   @param amounts are the respective amounts used in conjunction with itemDefIDs
        // *   @param errRealAmounts will store the actual amounts of items found by the method as additional information
        // */
        //public static bool HasItemDefenitions (int npcInstID, int[] itemDefIDs, int[] amounts, out int[] errRealAmounts)
        //{
        //    bool hasItems = true;
        //    errRealAmounts = new int[itemDefIDs.Length];

        //    // ---------------------------------------------------------------------------------
        //    // start with requesting the whole inventory of NPCInst
        //    // ---------------------------------------------------------------------------------

        //    // get the keys and values for the database-table individually but ensure same order
        //    List<string> inInv_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
        //    List<SQLiteGetTypeEnum> inInv_colTypesVals = new List<SQLiteGetTypeEnum>();
        //    for (int i = 0; i < inInv_colTypesKeys.Count; i++)
        //    {
        //        inInv_colTypesVals.Add( itemInInvTableDict[inInv_colTypesKeys[i]] );
        //    }

        //    System.Text.StringBuilder inInv_completeQuerySB = new System.Text.StringBuilder();
        //    inInv_completeQuerySB.Append("SELECT ");
        //    inInv_completeQuerySB.Append(Utilities.StringUtil.Concatenate<string>(inInv_colTypesKeys, ","));
        //    inInv_completeQuerySB.Append(" FROM `ItemInInventory_inst` WHERE `NPCInstID`=");
        //    inInv_completeQuerySB.Append(npcInstID);
        //    inInv_completeQuerySB.Append(" ORDER BY `ItemInstID` ASC;");
        //    string inInv_completeQuery = inInv_completeQuerySB.ToString();

        //    List<List<List<object>>> inInv_sqlResults = new List<List<List<object>>>();
        //    Database.DBReader.LoadFromDB(ref inInv_sqlResults, inInv_completeQuery);

        //    if ((inInv_sqlResults.Count < 1) || (inInv_sqlResults[0].Count < 1))
        //    {
        //        // no such items in inventory
        //        // errRealAmounts will be filled solely with zeros
        //        return false;
        //    }

        //    int inInv_ItemInstIDIndex = inInv_colTypesKeys.IndexOf("ItemInstID");
        //    int[] inInv_itemInstIDs = new int[inInv_sqlResults[0].Count];
        //    for ( int r = 0; r < inInv_sqlResults[0].Count; r++ )
        //    {
        //        inInv_itemInstIDs[r] = (int) inInv_sqlResults[0][r][inInv_ItemInstIDIndex];
        //    }

        //    // ---------------------------------------------------------------------------------
        //    // continue with checking the amounts of all ItemInst in Item_inst-database-table
        //    // ---------------------------------------------------------------------------------

        //    // get the keys and values for the database-table individually but ensure same order
        //    List<string> itemInst_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
        //    List<SQLiteGetTypeEnum> itemInst_colTypesVals = new List<SQLiteGetTypeEnum>();
        //    for (int i = 0; i < itemInst_colTypesKeys.Count; i++)
        //    {
        //        itemInst_colTypesVals.Add( itemInstTableDict[itemInst_colTypesKeys[i]] );
        //    }

        //    System.Text.StringBuilder itemInst_completeQuerySB = new System.Text.StringBuilder();
        //    itemInst_completeQuerySB.Append("SELECT ");
        //    itemInst_completeQuerySB.Append(Utilities.StringUtil.Concatenate<string>(itemInst_colTypesKeys, ","));
        //    itemInst_completeQuerySB.Append(" FROM `Item_inst` WHERE `ID` IN (");
        //    itemInst_completeQuerySB.Append(Utilities.StringUtil.Concatenate<int>(inInv_itemInstIDs, ","));
        //    itemInst_completeQuerySB.Append(") ORDER BY `ID` ASC;");
        //    string itemInst_completeQuery = itemInst_completeQuerySB.ToString();

        //    List<List<List<object>>> itemInst_sqlResults = new List<List<List<object>>>();
        //    Database.DBReader.LoadFromDB(ref itemInst_sqlResults, itemInst_completeQuery);

        //    if ((itemInst_sqlResults.Count < 1) || (itemInst_sqlResults[0].Count < 1))
        //    {
        //        // shouldn't happen when the database FOREIGN KEY-constraints are set up correctly :P
        //    }

        //    int itemInst_IDIndex = itemInst_colTypesKeys.IndexOf("ID");
        //    int itemInst_ItemDefIDIndex = itemInst_colTypesKeys.IndexOf("ItemDefID");
        //    int itemInst_AmountIndex = itemInst_colTypesKeys.IndexOf("Amount");
        //    // counts the already checked array members of itemDefIDs as well as remember the individual checks
        //    int checkCount = 0;
        //    bool[] checkedAlready = new bool[itemDefIDs.Length];
        //    // temporary index, which holds a found index in itemDefIDs
        //    int tempIndex = -1;

        //    // search all resulted Item_inst-rows which belong to the given npc 
        //    // to find the requested rows with suiting ItemDefIDs
        //    // prematurely break/exit the loop if all ItemDefIDs are found ()
        //    for (int r = 0; r < itemInst_sqlResults[0].Count; r++)
        //    {
        //        // find ItemDefID in the array of checked ItemDefIDs (can result in -1 for not found)
        //        tempIndex = Array.IndexOf<int>(itemDefIDs, (int) itemInst_sqlResults[0][r][itemInst_ItemDefIDIndex]);

        //        if (tempIndex >= 0)
        //        {
        //            // update the array of real found item amounts for error analysis by user
        //            errRealAmounts[tempIndex] = (int) itemInst_sqlResults[0][r][itemInst_AmountIndex];

        //            if (errRealAmounts[tempIndex] < amounts[tempIndex])
        //            {
        //                // if only one of all those items doesn't fulfill its necessary amount,
        //                // then the whole check will be false
        //                hasItems = false;
        //            }

        //            checkedAlready[tempIndex] = true;
        //            checkCount++;
        //            if (checkCount == itemDefIDs.Length)
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    if ((checkCount < itemDefIDs.Length) && (hasItems))
        //    {
        //        // if no conflicts have been found so far but not all entries of itemDefIDs were checked...
        //        for (int c = 0; c < checkedAlready.Length; c++)
        //        {
        //            if (!checkedAlready[c])
        //            {
        //                if (amounts[c] > 0)
        //                {
        //                    // only when necessary amount is positive, it actually matters if 
        //                    // there is enough of that item in the npcs possession
        //                    // otherwise, the amount is implicitly 0 (negative requested amounts are treated as 0, too)
        //                    hasItems = false;
        //                }
        //            }
        //        }
        //    }

        //    return hasItems;
        //}

        ///**
        // *   Broken and unused counterpart to HasItemDefinitions-method ==> DON'T USE THIS!
        // */
        //public static bool HasItemInstances (int npcInstID, int[] itemDefIDs, int[] amounts, out int[] errRealAmounts)
        //{
        //    bool hasItems = true;
        //    errRealAmounts = new int[itemDefIDs.Length];

        //    // ---------------------------------------------------------------------------------
        //    // start with requesting the whole inventory of NPCInst
        //    // ---------------------------------------------------------------------------------

        //    // get the keys and values for the database-table individually but ensure same order
        //    List<string> inInv_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
        //    List<SQLiteGetTypeEnum> inInv_colTypesVals = new List<SQLiteGetTypeEnum>();
        //    for (int i = 0; i < inInv_colTypesKeys.Count; i++)
        //    {
        //        inInv_colTypesVals.Add( itemInInvTableDict[inInv_colTypesKeys[i]] );
        //    }

        //    System.Text.StringBuilder inInv_completeQuerySB = new System.Text.StringBuilder();
        //    inInv_completeQuerySB.Append("SELECT ");
        //    inInv_completeQuerySB.Append(Utilities.StringUtil.Concatenate<string>(inInv_colTypesKeys, ","));
        //    inInv_completeQuerySB.Append(" FROM `ItemInInventory_inst` WHERE `NPCInstID`=");
        //    inInv_completeQuerySB.Append(npcInstID);
        //    inInv_completeQuerySB.Append(" ORDER BY `ItemInstID` ASC;");
        //    string inInv_completeQuery = inInv_completeQuerySB.ToString();

        //    List<List<List<object>>> inInv_sqlResults = new List<List<List<object>>>();
        //    Database.DBReader.LoadFromDB(ref inInv_sqlResults, inInv_completeQuery);

        //    if ((inInv_sqlResults.Count < 1) || (inInv_sqlResults[0].Count < 1))
        //    {
        //        // no such items in inventory
        //        // errRealAmounts will be filled solely with zeros
        //        return false;
        //    }

        //    int inInv_ItemInstIDIndex = inInv_colTypesKeys.IndexOf("ItemInstID");
        //    int[] inInv_itemInstIDs = new int[inInv_sqlResults[0].Count];
        //    for ( int r = 0; r < inInv_sqlResults[0].Count; r++ )
        //    {
        //        inInv_itemInstIDs[r] = (int) inInv_sqlResults[0][r][inInv_ItemInstIDIndex];
        //    }

        //    // ---------------------------------------------------------------------------------
        //    // continue with checking the amounts of all ItemInst in Item_inst-database-table
        //    // ---------------------------------------------------------------------------------

        //    // get the keys and values for the database-table individually but ensure same order
        //    List<string> itemInst_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
        //    List<SQLiteGetTypeEnum> itemInst_colTypesVals = new List<SQLiteGetTypeEnum>();
        //    for (int i = 0; i < itemInst_colTypesKeys.Count; i++)
        //    {
        //        itemInst_colTypesVals.Add( itemInstTableDict[itemInst_colTypesKeys[i]] );
        //    }

        //    System.Text.StringBuilder itemInst_completeQuerySB = new System.Text.StringBuilder();
        //    itemInst_completeQuerySB.Append("SELECT ");
        //    itemInst_completeQuerySB.Append(Utilities.StringUtil.Concatenate<string>(itemInst_colTypesKeys, ","));
        //    itemInst_completeQuerySB.Append(" FROM `Item_inst` WHERE `ID` IN (");
        //    itemInst_completeQuerySB.Append(Utilities.StringUtil.Concatenate<int>(inInv_itemInstIDs, ","));
        //    itemInst_completeQuerySB.Append(") ORDER BY `ID` ASC;");
        //    string itemInst_completeQuery = itemInst_completeQuerySB.ToString();

        //    List<List<List<object>>> itemInst_sqlResults = new List<List<List<object>>>();
        //    Database.DBReader.LoadFromDB(ref itemInst_sqlResults, itemInst_completeQuery);

        //    if ((itemInst_sqlResults.Count < 1) || (itemInst_sqlResults[0].Count < 1))
        //    {
        //        // shouldn't happen when the database FOREIGN KEY-constraints are set up correctly :P
        //    }

        //    int itemInst_IDIndex = itemInst_colTypesKeys.IndexOf("ID");
        //    int itemInst_ItemDefIDIndex = itemInst_colTypesKeys.IndexOf("ItemDefID");
        //    int itemInst_AmountIndex = itemInst_colTypesKeys.IndexOf("Amount");
        //    int tempIndex = 0;
        //    for (int r = 0; r < itemInst_sqlResults[0].Count; r++)
        //    {
        //        // get the index in original order with all expected itemInstIDs
        //        // (including those not found at all after search in inventory)
        //        tempIndex = Array.IndexOf<int>(itemDefIDs, (int) itemInst_sqlResults[0][r][itemInst_ItemDefIDIndex]);

        //        // update the array of real found item amounts for error analysis by user
        //        errRealAmounts[tempIndex] = (int) itemInst_sqlResults[0][r][itemInst_AmountIndex];

        //        if (errRealAmounts[tempIndex] < itemDefIDs[tempIndex])
        //        {
        //            // if only one of all those items doesn't fulfill its necessary amount, the whole check will be false
        //            hasItems = false;
        //        }
        //    }

        //    return hasItems;
        //}

        ////public static bool HasItemsByInst (int npcInstID, int[] itemInstIDs, int[] amounts, out int[] errRealAmounts)
        ////{
        ////    bool hasItems = true;
        ////    errRealAmounts = new int[itemInstIDs.Length];

        ////    // ---------------------------------------------------------------------------------
        ////    // start with searching the inventory of NPCInst for ItemInst fitting the given IDs
        ////    // ---------------------------------------------------------------------------------

        ////    // get the keys and values for the database-table individually but ensure same order
        ////    List<string> inInv_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
        ////    List<SQLiteGetTypeEnum> inInv_colTypesVals = new List<SQLiteGetTypeEnum>();
        ////    for (int i = 0; i < inInv_colTypesKeys.Count; i++)
        ////    {
        ////        inInv_colTypesVals.Add( itemInInvTableDict[inInv_colTypesKeys[i]] );
        ////    }

        ////    System.Text.StringBuilder inInv_completeQuerySB = new System.Text.StringBuilder();
        ////    inInv_completeQuerySB.Append("SELECT ");
        ////    inInv_completeQuerySB.Append(Utilities.String.Concatenate<string>(inInv_colTypesKeys, ","));
        ////    inInv_completeQuerySB.Append(" FROM `ItemInInventory_inst` WHERE `NPCInstID`=");
        ////    inInv_completeQuerySB.Append(npcInstID);
        ////    inInv_completeQuerySB.Append(" AND `ItemInstID` IN (");
        ////    inInv_completeQuerySB.Append(Utilities.String.Concatenate<int>(itemInstIDs, ","));
        ////    inInv_completeQuerySB.Append(") ORDER BY `ItemInstID` ASC;");
        ////    string inInv_completeQuery = inInv_completeQuerySB.ToString();

        ////    List<List<List<object>>> inInv_sqlResults = new List<List<List<object>>>();
        ////    Database.DBReader.LoadFromDB(ref inInv_sqlResults, inInv_completeQuery);

        ////    if ((inInv_sqlResults.Count < 1) || (inInv_sqlResults[0].Count < 1))
        ////    {
        ////        // no such items in inventory
        ////        // errRealAmounts will be filled solely with zeros
        ////        return false;
        ////    }

        ////    int inInv_ItemInstIDIndex = inInv_colTypesKeys.IndexOf("ItemInstID");
        ////    int[] inInv_itemInstIDs = new int[inInv_sqlResults[0].Count];
        ////    for ( int r = 0; r < inInv_sqlResults[0].Count; r++ )
        ////    {
        ////        inInv_itemInstIDs[r] = (int) inInv_sqlResults[0][r][inInv_ItemInstIDIndex];
        ////    }

        ////    if (inInv_itemInstIDs.Length < itemInstIDs.Length)
        ////    {
        ////        // if not all searched ids are found, there must be some missing completely
        ////        hasItems = false;
        ////    }

        ////    // ---------------------------------------------------------------------------------
        ////    // continue with checking the amounts of all ItemInst in Item_inst-database-table
        ////    // ---------------------------------------------------------------------------------

        ////    // get the keys and values for the database-table individually but ensure same order
        ////    List<string> itemInst_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
        ////    List<SQLiteGetTypeEnum> itemInst_colTypesVals = new List<SQLiteGetTypeEnum>();
        ////    for (int i = 0; i < itemInst_colTypesKeys.Count; i++)
        ////    {
        ////        itemInst_colTypesVals.Add( itemInstTableDict[itemInst_colTypesKeys[i]] );
        ////    }

        ////    System.Text.StringBuilder itemInst_completeQuerySB = new System.Text.StringBuilder();
        ////    itemInst_completeQuerySB.Append("SELECT ");
        ////    itemInst_completeQuerySB.Append(Utilities.String.Concatenate<string>(itemInst_colTypesKeys, ","));
        ////    itemInst_completeQuerySB.Append(" FROM `Item_inst` WHERE `ID` IN (");
        ////    itemInst_completeQuerySB.Append(Utilities.String.Concatenate<int>(inInv_itemInstIDs, ","));
        ////    itemInst_completeQuerySB.Append(") ORDER BY `ID` ASC;");
        ////    string itemInst_completeQuery = itemInst_completeQuerySB.ToString();

        ////    List<List<List<object>>> itemInst_sqlResults = new List<List<List<object>>>();
        ////    Database.DBReader.LoadFromDB(ref itemInst_sqlResults, itemInst_completeQuery);

        ////    if ((itemInst_sqlResults.Count < 1) || (itemInst_sqlResults[0].Count < 1))
        ////    {
        ////        // shouldn't happen when the database FOREIGN KEY-constraints are set up correctly :P
        ////    }

        ////    int itemInst_IDIndex = itemInst_colTypesKeys.IndexOf("ID");
        ////    int itemInst_ItemDefIDIndex = itemInst_colTypesKeys.IndexOf("ItemDefID");
        ////    int itemInst_AmountIndex = itemInst_colTypesKeys.IndexOf("Amount");
        ////    int tempIndex = 0;
        ////    for (int r = 0; r < itemInst_sqlResults[0].Count; r++)
        ////    {
        ////        // get the index in original order with all expected itemInstIDs
        ////        // (including those not found at all after search in inventory)
        ////        tempIndex = Array.IndexOf<int>(itemInstIDs, (int) itemInst_sqlResults[0][r][itemInst_IDIndex]);

        ////        // update the array of real found item amounts for error analysis by user
        ////        errRealAmounts[tempIndex] = (int) itemInst_sqlResults[0][r][itemInst_AmountIndex];

        ////        if (errRealAmounts[tempIndex] < itemInstIDs[tempIndex])
        ////        {
        ////            // if only one of all those items doesn't fulfill its necessary amount, the whole check will be false
        ////            hasItems = false;
        ////        }
        ////    }

        ////    return hasItems;
        ////}

    }
}
