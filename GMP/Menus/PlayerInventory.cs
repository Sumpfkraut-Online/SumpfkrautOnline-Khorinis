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

        public PlayerInventory()
        {
            inv = new GUCInventory(627, 134, 5, 6);
        }

        public override void Open()
        {
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
    }
}
