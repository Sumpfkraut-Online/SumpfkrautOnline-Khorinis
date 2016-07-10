using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.GUI;
using System.Collections.Specialized;
using GUC.Network.Messages;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    /*class PlayerInventory : GUCMenu
    {
        GUCInventory inv;

        public PlayerInventory()
        {
            // Create the player inventory relative to the screen size
            int x, y, cols, rows;
            int[] screenSize = GUCView.GetScreenSize();
            const int descrHeight = 200;
            const int slotSize = 70;

            cols = 5;
            x = screenSize[0] - (slotSize * cols + screenSize[0] / 25);
            y = screenSize[1] / 7 + 15;
            rows = (screenSize[1] - descrHeight - y) / slotSize;
            inv = new GUCInventory(x, y, cols, rows);
        }

        public void UpdateContents()
        {
            if (inv.Enabled)
            {
                inv.SetContents(WorldObjects.Player.Inventory);
            }
        }

        public override void Open()
        {
            if (WorldObjects.Player.Hero.gVob.GetBodyState() != 0) //only open while standing
                return;

            base.Open();
            inv.SetContents(WorldObjects.Player.Inventory);
            inv.Show();
            inv.Enabled = true;
        }

        public override void Close()
        {
            base.Close();
            inv.Hide();
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape || key == VirtualKeys.Tab)
            {
                Close();
            }
            else if (key == VirtualKeys.Menu)
            {
                if (inv.selectedItem == null)
                    return;

                if (inv.selectedItem.Amount > 1)
                    GUCMenus.InputNumber.Open(InventoryMessage.WriteDropItem, inv.selectedItem, inv.selectedItem.Amount);
                else
                    InventoryMessage.WriteDropItem(inv.selectedItem, 1);
            }
            else if (key == VirtualKeys.Control)
            {
                if (inv.selectedItem == null)
                    return;

                InventoryMessage.WriteUseItem(inv.selectedItem);
            }
            else
            {
                inv.KeyPressed(key);
            }
        }
    }*/
}
