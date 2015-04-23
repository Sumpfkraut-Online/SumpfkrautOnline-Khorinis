using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.Ingame.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class Trade : GUCMInputReceiver
    {
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

            buyInv = new GUCMenuInventory(80, 200, 2, 3, "Inv_Back_Buy.tga");
            sellInv = new GUCMenuInventory(290, 200, 2, 3, "Inv_Back_Sell.tga");
            inv = new GUCMenuInventory(500, 200, 4, 3, "Inv_Back.tga");

            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;

            inv.OnPressCTRL += messenger.OfferItem;
            sellInv.OnPressCTRL += messenger.RemoveItem;
            
            IngameInput.menus.Add(this);
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

        GUCMenuText test;
        public void KeyPressed(int key)
        {
            if (key == (int)VirtualKeys.O)
            {
                int xPtr = Process.ThisProcess().Alloc(4).ToInt32();
                int yPtr = Process.ThisProcess().Alloc(4).ToInt32();
                int address = ((IntPtr)(Player.Hero.Address + 0x06CC)).ToInt32();
                Process.ThisProcess().THISCALL<NullReturnCall>((uint)address, (uint)0x007A7660, new CallValue[] { (IntArg)xPtr, (IntArg)yPtr });

                if (test == null)
                {
                    test = new GUCMenuText("TEST", 10, 10);
                    test.Show();
                }
                test.Text = Process.ThisProcess().ReadInt(xPtr).ToString() + " " + Process.ThisProcess().ReadInt(yPtr).ToString();
                Process.ThisProcess().Free(new IntPtr(xPtr), 4);
                Process.ThisProcess().Free(new IntPtr(yPtr), 4);
            }
            else if (key == (int)VirtualKeys.T)
            {
                if (trader == null) //trade menu is not shown
                {
                    /*if (Player.Hero.FocusVob != null && Player.Hero.FocusVob.VobType == VobType.Player)
                    {
                       SendRequest(Player.Hero.FocusVob.ID);
                    }*/
                    messenger.SendRequest(Player.Hero.ID);
                }
                else
                {
                    messenger.SendBreak();
                    CloseTradeMenu();
                }
            }
            else if (key == (int)VirtualKeys.Escape)
            {
                if (trader != null)
                {
                    messenger.SendBreak();
                    CloseTradeMenu();
                }
            }
            else if (trader != null)
            {
                if (inv.enabled)
                {
                    inv.KeyPressed(key);
                    return;
                }
                else if (sellInv.enabled)
                {
                    sellInv.KeyPressed(key);
                    return;
                }
                else if (buyInv.enabled)
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
            sellInv.Open(null);
            buyInv.Open(null);
            IngameInput.activateFullControl(this);
        }

        private void CloseTradeMenu()
        {
            trader = null;
            sellInv.Hide();
            buyInv.Hide();
            inv.Hide();
            IngameInput.deactivateFullControl();
        }

        public void Update(long ticks)
        {
        }
    }
}
