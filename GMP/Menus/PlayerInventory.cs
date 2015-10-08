using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.GUI;
using System.Collections.Specialized;
using GUC.Client.Network.Messages;

namespace GUC.Client.Menus
{
    class PlayerInventory : GUCMenu
    {
        GUCInventory inv;

        bool shown = false;

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

        public override void Open()
        {
            if (WorldObjects.Player.Hero.gNpc.GetBodyState() != 0) //only open while standing
                return;

            base.Open();
            inv.SetContents(WorldObjects.Player.Inventory);
            inv.Show();
            inv.Enabled = true;
            shown = true;
        }

        public override void Close()
        {
            base.Close();
            inv.Hide();
            shown = false;
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape || key == VirtualKeys.Tab)
            {
                Close();
            }
            else if (key == VirtualKeys.Menu)
            {
                KeyValuePair<WorldObjects.ItemInstance, int> selected = inv.SelectedItem;
                if (selected.Key == null || selected.Value <= 0)
                    return;

                if (selected.Value > 1)
                    GUCMenus.InputNumber.Open(InventoryMessage.WriteDropItem, selected.Key, selected.Value);
                else
                    InventoryMessage.WriteDropItem(selected.Key, selected.Value);
            }
            else if (key == VirtualKeys.Control)
            {
                KeyValuePair<WorldObjects.ItemInstance, int> selected = inv.SelectedItem;
                if (selected.Key == null || selected.Value <= 0)
                    return;

                InventoryMessage.WriteUseItem(selected.Key);
            }
            else
            {
                inv.KeyPressed(key);
            }
        }

        public void UpdateContents()
        {
            if (shown)
            {
                inv.SetContents(WorldObjects.Player.Inventory);
            }
        }
    }
}
