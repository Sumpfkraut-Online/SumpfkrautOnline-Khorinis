using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.Server.Scripts.Sumpfkraut.Database;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{
    class InventoryVobHandler
    {

        private static Dictionary<String, Database.SQLiteGetTypeEnum> itemInInvTableDict = 
            Database.DBTables.InstTableDict[Database.InstTableEnum.ItemInInventory_inst];

        private static Dictionary<String, Database.SQLiteGetTypeEnum> itemInContTableDict = 
            Database.DBTables.InstTableDict[Database.InstTableEnum.ItemInContainer_inst];

        private static Dictionary<String, Database.SQLiteGetTypeEnum> itemInstTableDict = 
            Database.DBTables.InstTableDict[Database.InstTableEnum.Item_inst];


        private static object playerInvLock = new object();
        private static object containerInvLock = new object();



        public static bool TransferItems (int playerID_1, int playerID_2, 
            int[] itemDefIDs_1, int[] itemDefIDs_2, int[] amounts_1, int[] amounts_2)
        {
            bool transP1ToP2 = TransferItems(playerID_1, playerID_2, itemDefIDs_1, amounts_1);
            if (!transP1ToP2)
            {
                Log.Logger.logWarning("TransferItems: Transfer from player 1 to player 2 failed!"
                    + " No items will be traded and traded items will be transfered back to their original owners: "
                    + "playerID_1=" + playerID_1 + ", " + "playerID_2=" + playerID_2);
                return false;
            }
            bool transP2ToP1 = TransferItems(playerID_2, playerID_1, itemDefIDs_2, amounts_2);
            if (!transP2ToP1)
            {
                Log.Logger.logWarning("TransferItems: Transfer from player 2 to player 1 failed!"
                    + " No items will be traded and traded items will be transfered back to their original owners: "
                    + "playerID_1=" + playerID_1 + ", " + "playerID_2=" + playerID_2);
                transP1ToP2 = TransferItems(playerID_1, playerID_2, itemDefIDs_1, amounts_1);
                if (!transP1ToP2)
                {
                    string[] failedItems = new string[itemDefIDs_1.Length];
                    for (int i = 0; i < itemDefIDs_1.Length; i++)
                    {
                        failedItems[i] = itemDefIDs_1[i].ToString() + "=" + amounts_1[i].ToString();
                    }
                    Log.Logger.logWarning("TransferItems: Retransferring itmes from player 2 to player 1 after "
                        + "error in trade failed, too. The following items (by id) with their respective amounts "
                        + "must be transferred back manually:"
                        + string.Join(", ", failedItems));
                }
                return false;
            }
            return true;
        }

        public static bool TransferItems (int donatorID, int acceptorID, int[] itemDefIDs, int[] amounts)
        {
            bool giveItemsDon = RemoveItems(donatorID, itemDefIDs, amounts);
            if (!giveItemsDon)
            {
                Log.Logger.logWarning("TransferItems: Couldn't remove items from donators inventory: "
                    + "donatorID=" + donatorID);
                return false;
            }
            bool giveItemsAccept = GiveItems(acceptorID, itemDefIDs, amounts);
            if (!giveItemsAccept)
            {
                // give back the removed items to the original donator
                giveItemsDon = GiveItems(donatorID, itemDefIDs, amounts);
                if (!giveItemsDon)
                {
                    Log.Logger.logWarning("TransferItems: Failed to give back donators inventory-items after failed "
                        + "attempt to give these items to the acceptor-inventory: " 
                        + "donatorID=" + donatorID + ", acceptorID=" + acceptorID);
                }
                return false;
            }
            return true;
        }

        public static bool RemoveItems (int npcInstID, int[] itemDefIDs, int[] amounts)
        {
            for (int i = 0; i < amounts.Length; i++)
            {
                amounts[i] = -amounts[i];
            }
            return GiveItems(npcInstID, itemDefIDs, amounts);
        }

        public static bool GiveItems (int npcInstID, int[] itemDefIDs, int[] amounts)
        {
            int[] errRealAmounts;
            if (!HasItems(npcInstID, itemDefIDs, amounts, out errRealAmounts))
            {
                StringBuilder warningSB = new StringBuilder();
                warningSB.Append("GiveItems: NPC with npcInstID=" + npcInstID);
                warningSB.Append(" has insufficient amounts of specified itemDefIDs: ");

                bool isFirst = true;
                for (int i = 0; i < itemDefIDs.Length; i++)
                {
                    if (!isFirst)
                    {
                        warningSB.Append(", ");
                    }
                    else
                    {
                        isFirst = false;
                    }
                    warningSB.Append(itemDefIDs[i]);
                    warningSB.Append("=");
                    warningSB.Append(errRealAmounts[i]);
                    warningSB.Append("|");
                    warningSB.Append(amounts[i]);
                }

                Log.Logger.logWarning(warningSB.ToString());
                return false;
            }

            // !! TO DO !!

            //Utilities.String.Concatenate<int>(someIntArr, ",");
            //string completeQuery = "INSERT INTO";
            //Database.DBReader.SaveToDB(completeQuery);
            return true;
        }

        public static bool HasItems (int npcInstID, int[] itemDefIDs, int[] amounts, out int[] errRealAmounts)
        {
            bool hasItems = true;
            errRealAmounts = new int[itemDefIDs.Length];

            // ---------------------------------------------------------------------------------
            // start with requesting the whole inventory of NPCInst
            // ---------------------------------------------------------------------------------

            // get the keys and values for the database-table individually but ensure same order
            List<string> inInv_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
            List<SQLiteGetTypeEnum> inInv_colTypesVals = new List<SQLiteGetTypeEnum>();
            for (int i = 0; i < inInv_colTypesKeys.Count; i++)
            {
                inInv_colTypesVals.Add( itemInInvTableDict[inInv_colTypesKeys[i]] );
            }

            System.Text.StringBuilder inInv_completeQuerySB = new System.Text.StringBuilder();
            inInv_completeQuerySB.Append("SELECT ");
            inInv_completeQuerySB.Append(Utilities.String.Concatenate<string>(inInv_colTypesKeys, ","));
            inInv_completeQuerySB.Append(" FROM `ItemInInventory_inst` WHERE `NPCInstID`=");
            inInv_completeQuerySB.Append(npcInstID);
            inInv_completeQuerySB.Append(" ORDER BY `ItemInstID` ASC;");
            string inInv_completeQuery = inInv_completeQuerySB.ToString();

            List<List<List<object>>> inInv_sqlResults = new List<List<List<object>>>();
            Database.DBReader.LoadFromDB(ref inInv_sqlResults, inInv_completeQuery);

            if ((inInv_sqlResults.Count < 1) || (inInv_sqlResults[0].Count < 1))
            {
                // no such items in inventory
                // errRealAmounts will be filled solely with zeros
                return false;
            }

            int inInv_ItemInstIDIndex = inInv_colTypesKeys.IndexOf("ItemInstID");
            int[] inInv_itemInstIDs = new int[inInv_sqlResults[0].Count];
            for ( int r = 0; r < inInv_sqlResults[0].Count; r++ )
            {
                inInv_itemInstIDs[r] = (int) inInv_sqlResults[0][r][inInv_ItemInstIDIndex];
            }

            // ---------------------------------------------------------------------------------
            // continue with checking the amounts of all ItemInst in Item_inst-database-table
            // ---------------------------------------------------------------------------------

            // get the keys and values for the database-table individually but ensure same order
            List<string> itemInst_colTypesKeys = new List<string>(itemInInvTableDict.Keys);
            List<SQLiteGetTypeEnum> itemInst_colTypesVals = new List<SQLiteGetTypeEnum>();
            for (int i = 0; i < itemInst_colTypesKeys.Count; i++)
            {
                itemInst_colTypesVals.Add( itemInstTableDict[itemInst_colTypesKeys[i]] );
            }

            System.Text.StringBuilder itemInst_completeQuerySB = new System.Text.StringBuilder();
            itemInst_completeQuerySB.Append("SELECT ");
            itemInst_completeQuerySB.Append(Utilities.String.Concatenate<string>(itemInst_colTypesKeys, ","));
            itemInst_completeQuerySB.Append(" FROM `Item_inst` WHERE `ID` IN (");
            itemInst_completeQuerySB.Append(Utilities.String.Concatenate<int>(inInv_itemInstIDs, ","));
            itemInst_completeQuerySB.Append(") ORDER BY `ID` ASC;");
            string itemInst_completeQuery = itemInst_completeQuerySB.ToString();

            List<List<List<object>>> itemInst_sqlResults = new List<List<List<object>>>();
            Database.DBReader.LoadFromDB(ref itemInst_sqlResults, itemInst_completeQuery);

            if ((itemInst_sqlResults.Count < 1) || (itemInst_sqlResults[0].Count < 1))
            {
                // shouldn't happen when the database FOREIGN KEY-constraints are set up correctly :P
            }

            int itemInst_IDIndex = itemInst_colTypesKeys.IndexOf("ID");
            int itemInst_ItemDefIDIndex = itemInst_colTypesKeys.IndexOf("ItemDefID");
            int itemInst_AmountIndex = itemInst_colTypesKeys.IndexOf("Amount");
            int tempIndex = 0;
            for (int r = 0; r < itemInst_sqlResults[0].Count; r++)
            {
                // get the index in original order with all expected itemInstIDs
                // (including those not found at all after search in inventory)
                tempIndex = Array.IndexOf<int>(itemDefIDs, (int) itemInst_sqlResults[0][r][itemInst_ItemDefIDIndex]);

                // update the array of real found item amounts for error analysis by user
                errRealAmounts[tempIndex] = (int) itemInst_sqlResults[0][r][itemInst_AmountIndex];

                if (errRealAmounts[tempIndex] < itemDefIDs[tempIndex])
                {
                    // if only one of all those items doesn't fulfill its necessary amount, the whole check will be false
                    hasItems = false;
                }
            }

            return hasItems;
        }

        //public static bool HasItemsByInst (int npcInstID, int[] itemInstIDs, int[] amounts, out int[] errRealAmounts)
        //{
        //    bool hasItems = true;
        //    errRealAmounts = new int[itemInstIDs.Length];

        //    // ---------------------------------------------------------------------------------
        //    // start with searching the inventory of NPCInst for ItemInst fitting the given IDs
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
        //    inInv_completeQuerySB.Append(Utilities.String.Concatenate<string>(inInv_colTypesKeys, ","));
        //    inInv_completeQuerySB.Append(" FROM `ItemInInventory_inst` WHERE `NPCInstID`=");
        //    inInv_completeQuerySB.Append(npcInstID);
        //    inInv_completeQuerySB.Append(" AND `ItemInstID` IN (");
        //    inInv_completeQuerySB.Append(Utilities.String.Concatenate<int>(itemInstIDs, ","));
        //    inInv_completeQuerySB.Append(") ORDER BY `ItemInstID` ASC;");
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

        //    if (inInv_itemInstIDs.Length < itemInstIDs.Length)
        //    {
        //        // if not all searched ids are found, there must be some missing completely
        //        hasItems = false;
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
        //    itemInst_completeQuerySB.Append(Utilities.String.Concatenate<string>(itemInst_colTypesKeys, ","));
        //    itemInst_completeQuerySB.Append(" FROM `Item_inst` WHERE `ID` IN (");
        //    itemInst_completeQuerySB.Append(Utilities.String.Concatenate<int>(inInv_itemInstIDs, ","));
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
        //        tempIndex = Array.IndexOf<int>(itemInstIDs, (int) itemInst_sqlResults[0][r][itemInst_IDIndex]);

        //        // update the array of real found item amounts for error analysis by user
        //        errRealAmounts[tempIndex] = (int) itemInst_sqlResults[0][r][itemInst_AmountIndex];

        //        if (errRealAmounts[tempIndex] < itemInstIDs[tempIndex])
        //        {
        //            // if only one of all those items doesn't fulfill its necessary amount, the whole check will be false
        //            hasItems = false;
        //        }
        //    }

        //    return hasItems;
        //}

    }
}
