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
            int[] screenSize = GUCView.GetScreenSize();

            cols = 5;
            x = screenSize[0] - (GUCInventory.SlotSize * cols + screenSize[0] / 25);
            y = screenSize[1] / 7 + 15;
            rows = (screenSize[1] - GUCInventory.DescriptionBoxHeight - y) / GUCInventory.SlotSize;

            inv = new GUCInventory(x, y, cols, rows);
            player = ScriptClient.Client.Character;
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
            ItemInst.OnSetAmount += UpdateAmountEventMethod;
            VobSystem.Instances.ItemContainers.ScriptInventory.OnAddItem += UpdateInventory;
            VobSystem.Instances.ItemContainers.ScriptInventory.OnRemoveItem += UpdateInventory;

            if (player == null)
                return;

            var env = player.Environment;
            if (env.InAir)
                return;

            if (player.Movement != NPCMovement.Stand)
                player.SetMovement(NPCMovement.Stand);

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

        public override void Update(long now)
        {
        }

        public override void KeyDown(VirtualKeys key, long now)
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
                            /*Animations.AniJob dropJob = 
                             player.StartAniJump(ani, 10, 10);
                             player.ModelInst.StartAnimation(dropJob, 0.2f);
                             if (player.Model.TryGetAniJob((int)SetAnis.DropItem, out dropJob))
                             {
                                 if (selItem.Amount > 1)
                                 {

                                 }
                                 else
                                 {
                                     ScriptClient.Client.BaseClient.DoStartAni(dropJob.BaseAniJob, selItem);
                                 }
                             }
                         */
                         if (selectedItem.Amount > 1)
                            {
                                DropItemMenu.Menu.Open(selectedItem);
                            }
                         else if(selectedItem.Amount == 1)
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

                    if(selItem.ItemType < ItemTypes.MAXWEAPON)
                    {
                        player.LastUsedWeapon = selItem;
                    }
                    
                    switch(selItem.ItemType)
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

        public override void KeyUp(VirtualKeys key, long now)
        {
        }
    }
}
