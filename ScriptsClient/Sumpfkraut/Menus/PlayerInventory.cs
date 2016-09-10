using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.GUI;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    class PlayerInventory : GUCMenu
    {
        public static readonly PlayerInventory Menu = new PlayerInventory();

        GUCInventory inv;

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
        }

        public void UpdateContents()
        {
            //inv.SetContents(ScriptClient.Client.Character?.BaseInst.Inventory);
        }

        public override void Open()
        {
            NPCInst player = ScriptClient.Client.Character;
            if (player == null)
                return;

            //if (player.Movement != MoveState.Stand || player.Environment > EnvironmentState.Wading)
            //    return;

            base.Open();
            //inv.SetContents(player.BaseInst.Inventory);
            inv.Show();
            inv.Enabled = true;
        }

        public override void Close()
        {
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
                case VirtualKeys.Menu: // DROP
                    NPCInst player = ScriptClient.Client.Character;
                    if (player != null)
                    {
                        /*ItemInst selItem = inv.GetSelectedItem();
                        if (selItem != null)
                        {
                            ScriptAniJob dropJob;
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
                        }*/
                    }

                    /*if (inv.selectedItem == null)
                        return;

                    if (inv.selectedItem.Amount > 1)
                        GUCMenus.InputNumber.Open(InventoryMessage.WriteDropItem, inv.selectedItem, inv.selectedItem.Amount);
                    else
                        InventoryMessage.WriteDropItem(inv.selectedItem, 1);*/
                    break;
                case VirtualKeys.Control: // USE
                    /*if (inv.selectedItem == null)
                        return;

                    InventoryMessage.WriteUseItem(inv.selectedItem);*/
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
