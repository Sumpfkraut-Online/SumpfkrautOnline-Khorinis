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
    class Trade : IMessage, GUCMInputReceiver
    {
        GUCMenuInventory inv;
        GUCMenuInventory sellInv;
        GUCMenuInventory buyInv;
        bool shown;

        public Trade()
        {
            buyInv = new GUCMenuInventory(80, 200, 2, 3, "Inv_Back_Buy.tga");
            sellInv = new GUCMenuInventory(290, 200, 2, 3, "Inv_Back_Sell.tga");
            inv = new GUCMenuInventory(500, 200, 4, 3, "Inv_Back.tga");

            inv.left = sellInv;
            sellInv.right = inv;
            sellInv.left = buyInv;
            buyInv.right = sellInv;
            
            IngameInput.menus.Add(this);
            shown = false;
        }

        public void KeyPressed(int key)
        {
            if (key == (int)VirtualKeys.T)
            {
                if (!shown)
                {
                    inv.SetCursor(0, 0);
                    inv.Show(Player.Hero.ItemList);
                    sellInv.Show(null);
                    buyInv.Show(null);
                    shown = true;
                    IngameInput.activateFullControl(this);
                }
            }
            else if (key == (int)VirtualKeys.Escape)
            {
                if (shown)
                {
                    sellInv.Hide();
                    buyInv.Hide();
                    inv.Hide();
                    shown = false;
                    IngameInput.deactivateFullControl();
                }
            }
            else if (shown)
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

        public void Update(long ticks)
        {

        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
        }
    }
}
