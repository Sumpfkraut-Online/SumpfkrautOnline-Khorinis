using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.GUI;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    class PlayerInventory : GUCMenu
    {
        public static readonly PlayerInventory Menu = new PlayerInventory();
        GUCInventory inv;
        NPCInst player;

        public PlayerInventory()
        {
            // Create the player inventory relative to the screen size
            int x, y, cols, rows;
            var screenSize = GUCView.GetScreenSize();

            cols = 5;
            x = screenSize.X - (GUCInventory.SlotSize * cols + screenSize.X / 25);
            y = screenSize.Y / 7 + 15;
            rows = (screenSize.Y - GUCInventory.DescriptionBoxHeight - y) / GUCInventory.SlotSize;

            inv = new GUCInventory(x, y, cols, rows);
        }

        public void UpdateContents()
        {
            inv.SetContents(NPCInst.Hero?.Inventory);
        }

        public void UpdateAmountEventMethod(ItemInst item, int amount)
        {
            inv.UpdateAmounts();
        }

        public void UpdateInventory(ItemInst item)
        {
            UpdateContents();
        }

        public void UpdateEquipment()
        {
            inv.UpdateEquipment();
        }

        public override void Open()
        {
            player = ScriptClient.Client.Character;
            if (player == null || player.IsDead || player.IsInFightMode)
                return;

            var env = player.Environment;
            if (env.InAir)
                return;

            player.SetMovement(NPCMovement.Stand);

            ItemInst.OnSetAmount += UpdateAmountEventMethod;
            VobSystem.Instances.ItemContainers.ScriptInventory.OnAddItem += UpdateInventory;
            VobSystem.Instances.ItemContainers.ScriptInventory.OnRemoveItem += UpdateInventory;

            base.Open();
            inv.SetContents(player.Inventory);
            inv.Show();
            inv.Enabled = true;
        }

        public override void Close()
        {
            ItemInst.OnSetAmount -= UpdateAmountEventMethod;
            VobSystem.Instances.ItemContainers.ScriptInventory.OnAddItem -= UpdateInventory;
            VobSystem.Instances.ItemContainers.ScriptInventory.OnRemoveItem -= UpdateInventory;

            base.Close();
            inv.Hide();
        }


        protected override void KeyDown(VirtualKeys key)
        {
            switch (key)
            {
                case VirtualKeys.Escape:
                case VirtualKeys.Tab:
                    Close();
                    break;
                case VirtualKeys.L: // DROP
                    if (player != null)
                    {
                        ItemInst selectedItem = inv.GetSelectedItem();
                        if (selectedItem != null)
                        {
                            if (selectedItem.Amount > 1)
                            {
                                DropItemMenu.Menu.Open(selectedItem);
                            }
                            else if (selectedItem.Amount == 1)
                            {
                                NPCInst.Requests.DropItem(player, selectedItem, 1);
                            }
                        }
                    }
                    break;
                case VirtualKeys.Control: // USE
                    ItemInst selItem = inv.GetSelectedItem();
                    if (selItem == null)
                        return;

                    if (selItem.ItemType < ItemTypes.MAXWEAPON)
                    {
                        player.LastUsedWeapon = selItem;
                    }

                    switch (selItem.ItemType)
                    {
                        case ItemTypes.Wep1H:
                        case ItemTypes.Wep2H:
                        case ItemTypes.WepBow:
                        case ItemTypes.WepXBow:
                        case ItemTypes.Armor:
                            if (selItem.IsEquipped)
                                NPCInst.Requests.UnequipItem(player, selItem);
                            else
                                NPCInst.Requests.EquipItem(player, selItem);
                            break;
                        default:
                            NPCInst.Requests.UseItem(player, selItem);
                            break;
                    }
                    break;
                default:
                    inv.KeyPressed(key);
                    break;
            }
        }
    }
}
