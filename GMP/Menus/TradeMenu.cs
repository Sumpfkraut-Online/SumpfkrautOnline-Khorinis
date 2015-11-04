using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Network;
using GUC.Enumeration;
using GUC.Client.WorldObjects;
using GUC.Client.GUI;
using GUC.Client.Network.Messages;
using Gothic.mClasses;
using Gothic.zClasses;
using RakNet;


namespace GUC.Client.Menus
{
    class TradeMenu : GUCMenu
    {

        private static TradeMenu trade;
        public static TradeMenu GetTrade()
        {
            if (trade == null)
            {
                trade = new TradeMenu();
            }
            return trade;
        }

        public VirtualKeys HotKey = VirtualKeys.T;
        public VirtualKeys AcceptTradeKey = VirtualKeys.T;

        GUCInventory inv;
        GUCInventory sellInv;
        GUCInventory buyInv;

        Dictionary<uint, Item> itemsInv;
        Dictionary<uint, Item> itemsBuy;
        Dictionary<uint, Item> itemsSell;

        bool confirmed = false;
        bool otherConfirmed = false;

        public TradeMenu()
        {
            itemsBuy = new Dictionary<uint, Item>();
            itemsSell = new Dictionary<uint, Item>();

            buyInv = new GUCInventory(80, 200, 2, 3); //, "Inv_Back_Sell.tga"); 
            sellInv = new GUCInventory(290, 200, 2, 3); //, "Inv_Back_Sell.tga"); 
            inv = new GUCInventory(500, 200, 4, 3); //, "Inv_Back.tga");

            sellInv.SetContents(itemsSell);
            buyInv.SetContents(itemsBuy);

            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape)
            {
                TradeMessage.SendBreak();
                TradeCancelled();
            }
            else if (key == VirtualKeys.Control)
            {
                if (inv.Enabled)
                {
                    TradeMessage.OfferItem(inv.selectedItem);
                }
                else if (sellInv.Enabled)
                {
                    TradeMessage.RemoveItem(sellInv.selectedItem);
                }
            }
            else if(key == AcceptTradeKey)
            {
                if (!confirmed)
                {
                    confirmed = true;
                    TradeMessage.ConfirmOffer();
                    sellInv.SetAcceptStateColor(true);
                }
                else
                {
                    TradeMessage.DeclineOffer();
                    DeclineOffer();
                }
            }
            else
            {
                if (inv.Enabled)
                {
                    inv.KeyPressed(key);
                    return;
                }
                else if (sellInv.Enabled)
                {
                    sellInv.KeyPressed(key);
                    return;
                }
                else if (buyInv.Enabled)
                {
                    buyInv.KeyPressed(key);
                    return;
                }
            }
        }

        public override void Open()
        {
            itemsInv = new Dictionary<uint, Item>(Player.Inventory);

            buyInv.Show();
            sellInv.Show();
            inv.Show();
            inv.SetContents(Player.Inventory);
            inv.Enabled = true;
            base.Open();
        }

        public override void Close()
        {
            buyInv.Hide();
            sellInv.Hide();
            inv.Hide();
            base.Close();
        }

        public void DeclineOffer()
        {
            confirmed = false;
            sellInv.SetAcceptStateColor(false);
        }

        public void OtherConfirmedOffer()
        {
            otherConfirmed = true;
            buyInv.SetAcceptStateColor(true);
        }

        public void OtherDeclinedOffer()
        {
            otherConfirmed = false;
            buyInv.SetAcceptStateColor(false);
        }

        public void TradeRequested(NPC trader)
        {
            // eine anfrage zum Handel hat stattgefunden
        }

        public void TradeAccepted(NPC trader)
        {
            // init any suff that the trade has been accepted
            Open();
            sellInv.LeftInfoBox = Player.Hero.Name;
            buyInv.LeftInfoBox = trader.Name;
        }

        public void TradeCancelled()
        {
            // trader cancelled trading
            itemsBuy.Clear();
            buyInv.SetContents(itemsBuy);
            buyInv.SetAcceptStateColor(false);
            itemsSell.Clear();
            sellInv.SetContents(itemsSell);
            sellInv.SetAcceptStateColor(false);
            Close();
        }

        public void ChangeItem(Item item, bool self, bool add)
        {
            if(confirmed)
            {
                confirmed = false;
                sellInv.SetAcceptStateColor(false);
            }

            if(otherConfirmed)
            { 
                OtherDeclinedOffer();
            }

            if (self)
            {
                if (add)
                {
                    itemsSell.Add(item.ID, item);
                    itemsInv.Remove(item.ID);
                }
                else
                {
                    itemsInv.Add(item.ID, item);
                    itemsSell.Remove(item.ID);
                }
                inv.SetContents(itemsInv);
                sellInv.SetContents(itemsSell);
            }
            else
            {
                if (add)
                {
                    itemsBuy.Add(item.ID, item);
                }
                else
                {
                    itemsBuy.Remove(item.ID);
                }
                buyInv.SetContents(itemsBuy);
            }
        }
    }
}
