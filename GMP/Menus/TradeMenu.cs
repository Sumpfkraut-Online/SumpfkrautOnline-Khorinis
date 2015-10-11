using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Enumeration;
using GUC.Client.WorldObjects;
using GUC.Network;
using GUC.Client.GUI;
using RakNet;
using GUC.Client.Network.Messages;
using Gothic.mClasses;
using Gothic.zClasses;


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

        GUCInventory inv;
        GUCInventory sellInv;
        GUCInventory buyInv;

        Dictionary<ItemInstance, int> itemsInv;
        Dictionary<ItemInstance, int> itemsBuy;
        Dictionary<ItemInstance, int> itemsSell;

        public TradeMenu()
        {

            itemsInv = new Dictionary<ItemInstance, int>(Player.Inventory);
            itemsBuy = new Dictionary<ItemInstance, int>();
            itemsSell = new Dictionary<ItemInstance, int>();

            

            buyInv = new GUCInventory(80, 200, 2, 3); //, "Inv_Back_Sell.tga"); 
            sellInv = new GUCInventory(290, 200, 2, 3); //, "Inv_Back_Sell.tga"); 
            inv = new GUCInventory(500, 200, 4, 3); //, "Inv_Back.tga");
            
            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;

            /*

            inv.OnPressCTRL += messenger.OfferItem;
            sellInv.OnPressCTRL += messenger.RemoveItem;
             */
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape)
                Close();
        }

        public override void Open()
        {
            inv.SetContents(itemsInv);

            buyInv.Show();
            sellInv.Show();
            inv.Show();
            base.Open();
        }

        public override void Close()
        {
            buyInv.Hide();
            sellInv.Hide();
            inv.Hide();
            base.Close();
        }

        public void TradeRequested(NPC trader)
        {

        }

        public void TradeCancelled()
        {
            // trader cancelled trading
        }

        public void ChangeItem(ItemInstance item, bool self, bool add)
        {
            if (self)
            {
                if (add)
                {
                    if (itemsSell.ContainsKey(item))
                    {
                        itemsSell.Add(item, 1);
                    }
                    else
                    {
                        itemsSell[item]++;
                    }
                    if (itemsInv[item] > 1)
                    {
                        itemsInv[item]--;
                    }
                    else
                    {
                        itemsInv.Remove(item);
                    }
                }
                else
                {
                    if (itemsInv.ContainsKey(item))
                    {
                        itemsInv[item]++;
                    }
                    else
                    {
                        itemsInv.Add(item, 1);
                    }
                    if (itemsSell[item] > 1)
                    {
                        itemsSell[item]--;
                    }
                    else
                    {
                        itemsSell.Remove(item);
                    }
                }
                inv.SetContents(itemsInv);
                sellInv.SetContents(itemsSell);
            }
            else
            {
                if (add)
                {
                    if(itemsBuy.ContainsKey(item))
                    {
                        itemsBuy[item]++;
                    }
                    else
                    {
                        itemsBuy.Add(item, 1);
                    }
                }
                else
                {
                    if (itemsBuy[item] > 1)
                    {
                        itemsBuy[item]--;
                    }
                    else
                    {
                        itemsBuy.Remove(item);
                    }
                }
                buyInv.SetContents(itemsBuy);
            }
        }

        /*
        public void Activate()
        {
            if (Player.Hero.FocusVob != null /* && Player.Hero.FocusVob.VobType == VobType.Player)
            {
                messenger.SendRequest(Player.Hero.FocusVob.ID);
            }
        }

        private void ChangeItem(Item item, bool self, bool add)
        {
            if (self)
            {
                if (add)
                {
                    sellInv.AddItem(item);
                    inv.RemoveItem(item);
                }
                else
                {
                    sellInv.RemoveItem(item);
                    inv.AddItem(item);
                }
            }
            else
            {
                if (add)
                {
                    buyInv.AddItem(item);
                }
                else
                {
                    buyInv.RemoveItem(item);
                }
            }
        }

        public void KeyPressed(int key)
        {
            if (key == (int)Chat.GetChat().ActivationKey)
            { //chat can be opened during trade
                Chat.GetChat().Open();
            }
            else if (key == (int)VirtualKeys.Escape)
            {
                messenger.SendBreak();
                CloseTradeMenu();
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

        private void Open(Player pl)
        {
            trader = pl;
            inv.SetCursor(0, 0);
            inv.Open(Player.Hero.ItemList);
            sellInv.Open(null); sellInv.Title = Player.Hero.Name;
            buyInv.Open(null); buyInv.Title = pl.Name;
        }

        private void Close()
        {
            trader = null;
            sellInv.Hide();
            buyInv.Hide();
            inv.Hide();
        }

        public void Update(long ticks)
        {
        }*/

    }

  
}