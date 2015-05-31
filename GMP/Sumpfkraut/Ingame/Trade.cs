using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class Trade : GUCInputReceiver
    {
        private static Trade trade;
        public static Trade GetTrade()
        {
            if (trade == null)
            {
                trade = new Trade();
            }
            return trade;
        }

        public VirtualKeys ActivationKey = VirtualKeys.T;

        GUCMenuInventory inv;
        GUCMenuInventory sellInv;
        GUCMenuInventory buyInv;

        Player trader;

        private TradeMessage messenger;
        public Trade()
        {
            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.TradeMessage))
            {
                Program.client.messageListener.Add((byte)NetworkID.TradeMessage, new TradeMessage());
            }
            messenger = (TradeMessage)Program.client.messageListener[(byte)NetworkID.TradeMessage];

            messenger.OnBreakMessage += CloseTradeMenu;
            messenger.OnAcceptMessage += OpenTradeMenu;
            messenger.OnChangeItemMessage += ChangeItem;

            buyInv = new GUCMenuInventory(80, 200, 2, 3, "Inv_Back_Sell.tga"); 
            buyInv.ShowGold = false;
            sellInv = new GUCMenuInventory(290, 200, 2, 3, "Inv_Back_Sell.tga"); 
            sellInv.ShowGold = false;
            inv = new GUCMenuInventory(500, 200, 4, 3, "Inv_Back.tga");
            
            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;

            inv.OnPressCTRL += messenger.OfferItem;
            sellInv.OnPressCTRL += messenger.RemoveItem;
        }

        public void Activate()
        {
                if (Player.Hero.FocusVob != null /* && Player.Hero.FocusVob.VobType == VobType.Player*/)
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

        private void OpenTradeMenu(Player pl)
        {
            trader = pl;
            inv.SetCursor(0, 0);
            inv.Open(Player.Hero.ItemList);
            sellInv.Open(null); sellInv.Title = Player.Hero.Name;
            buyInv.Open(null); buyInv.Title = pl.Name;
            InputHandler.activateFullControl(this);
        }

        private void CloseTradeMenu()
        {
            trader = null;
            sellInv.Hide();
            buyInv.Hide();
            inv.Hide();
            InputHandler.deactivateFullControl(this);
        }

        public void Update(long ticks)
        {
        }
    }
}
