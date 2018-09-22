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
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.ItemContainers;

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

        public void UpdateAmountEventMethod(ItemInst item)
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
            if (Opened)
                return;

            player = ScriptClient.Client.Character;
            if (player == null || player.IsDead || player.IsInFightMode || player.ModelInst.IsInAnimation())
                return;

            var env = player.Environment;
            if (env.InAir || env.WaterLevel > 0.4f)
                return;

            player.SetMovement(NPCMovement.Stand);

            player.Inventory.OnItemAmountChange += UpdateAmountEventMethod;
            player.Inventory.OnAddItem += UpdateInventory;
            player.Inventory.OnRemoveItem += UpdateInventory;
            player.OnEquip += Player_OnEquip;
            player.OnUnequip += Player_OnUnequip;

            base.Open();
            inv.SetContents(player.Inventory);
            inv.Show();
            inv.Enabled = true;
        }

        private void Player_OnUnequip(ItemInst item)
        {
            UpdateEquipment();
        }

        void Player_OnEquip(ItemInst item)
        {
            UpdateEquipment();
        }

        public override void Close()
        {
            if (!Opened)
                return;

            player.Inventory.OnItemAmountChange -= UpdateAmountEventMethod;
            player.Inventory.OnAddItem -= UpdateInventory;
            player.Inventory.OnRemoveItem -= UpdateInventory;
            player.OnEquip -= Player_OnEquip;
            player.OnUnequip -= Player_OnUnequip;

            base.Close();
            inv.Hide();
        }


        protected override void KeyPress(VirtualKeys key, bool hold)
        {
            switch (key)
            {
                case VirtualKeys.Escape:
                case VirtualKeys.Tab:
                    if (!hold)
                        Close();
                    break;
                case VirtualKeys.Menu: // DROP
                    if (!hold && player != null && !Arena.ArenaClient.GMJoined)
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
                    if (!hold)
                    {
                        ItemInst selItem = inv.GetSelectedItem();
                        if (selItem == null)
                            return;

                        switch (selItem.ItemType)
                        {
                            case ItemTypes.Wep1H:
                            case ItemTypes.Wep2H:
                            case ItemTypes.WepBow:
                            case ItemTypes.WepXBow:
                            case ItemTypes.Armor:
                            case ItemTypes.Torch:
                                if (!Arena.ArenaClient.GMJoined)
                                    if (selItem.IsEquipped)
                                        NPCInst.Requests.UnequipItem(player, selItem);
                                    else
                                        NPCInst.Requests.EquipItem(player, selItem);
                                break;
                            default:
                                NPCInst.Requests.UseItem(player, selItem);
                                break;
                        }
                    }
                    break;
                default:
                    inv.KeyPressed(key);
                    break;
            }
        }
    }
}
