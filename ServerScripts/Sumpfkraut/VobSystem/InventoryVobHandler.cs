using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.VobSystem
{
    class InventoryVobHandler
    {

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
            //Utilities.String.Concatenate<int>(someIntArr, ",");
            //string completeQuery = "INSERT INTO";
            //Database.DBReader.SaveToDB(completeQuery);
            return true;
        }

        public static bool HasItems (int npcInstID, int[] itemDefIDs, int[] amounts, ref int[] errRealAmounts)
        {
            //System.Text.StringBuilder completeQuerySB = new System.Text.StringBuilder();
            //Database.DBReader.LoadFromDB();
            return true;
        }

    }
}
