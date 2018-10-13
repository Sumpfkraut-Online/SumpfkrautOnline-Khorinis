using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.GUI
{
    public class GUCInventory : GUCView
    {
        const string TextureBackgroundDefault = "INV_BACK.TGA";
        const string TextureTitle = "INV_TITLE.TGA";
        const string TextureSlot = "INV_SLOT.TGA";
        const string TextureSlotHL = "INV_SLOT_HIGHLIGHTED.TGA";
        const string TextureSlotEquip = "INV_SLOT_EQUIPPED.TGA";
        const string TextureSlotEquipHL = "INV_SLOT_EQUIPPED_HIGHLIGHTED.TGA";

        public const int DescriptionBoxWidth = 720;
        public const int DescriptionBoxHeight = 162;
        public const int SlotSize = 70;

        #region Slot

        class Slot : GUCView
        {
            public GUCVisual back;
            GUCVobVisual vis;
            GUCVisualText amount;
            bool shown = false;

            public Slot(int x, int y)
            {
                back = new GUCVisual(x, y, SlotSize, SlotSize);
                back.SetBackTexture(TextureSlot);

                vis = new GUCVobVisual(x, y, SlotSize, SlotSize);

                amount = vis.CreateText("", SlotSize - 5, SlotSize - 5 - FontsizeDefault);
                amount.Format = GUCVisualText.TextFormat.Right;
            }

            ItemInst item = null;
            public ItemInst Item { get { return item; } }
            public void SetItem(ItemInst item)
            {
                if (this.item == item)
                    return;
                this.item = item;

                UpdateBackTex();
                if (item == null)
                {
                    vis.SetVisual(string.Empty);
                    vis.Hide();
                }
                else
                {
                    vis.SetVisual(item.ModelDef.Visual);
                    if (shown)
                    {
                        vis.Show();
                    }
                    int num = item.Amount;
                    if (num > 1)
                    {
                        amount.Text = num.ToString();
                    }
                    else
                    {
                        amount.Text = string.Empty;
                    }

                    item.Definition.PositionInVobVisual(vis);
                }
            }

            bool isSelected = false;
            public bool IsSelected
            {
                get { return this.isSelected; }
                set
                {
                    if (this.isSelected != value)
                    {
                        this.isSelected = value;
                        UpdateBackTex();
                    }
                }
            }

            public void UpdateBackTex()
            {
                if (this.isSelected)
                {
                    back.SetBackTexture((item != null && item.IsEquipped) ? TextureSlotEquipHL : TextureSlotHL);
                }
                else
                {
                    back.SetBackTexture((item != null && item.IsEquipped) ? TextureSlotEquip : TextureSlot);
                }
            }

            public void UpdateSlotAmount()
            {
                int num = item != null ? item.Amount : -1;
                if (num > 1)
                {
                    amount.Text = num.ToString();
                }
                else
                {
                    amount.Text = string.Empty;
                }
            }

            public override void Show()
            {
                back.Show();
                if (item != null)
                {
                    vis.Show();
                    UpdateSlotAmount();
                }
                shown = true;
            }

            public override void Hide()
            {
                vis.Hide();
                back.Hide();
                shown = false;
            }
        }

        #endregion

        // info box on the top right
        GUCVisual rightBack;
        GUCVisual rightVis;
        GUCVisualText rightText;
        public string RightInfoBox;

        // info box on the top left
        GUCVisual leftBack;
        GUCVisual leftVis;
        GUCVisualText leftText;
        public string LeftInfoBox;

        // background
        GUCVisual back;
        Slot[,] slots;

        // cursor
        Vec2i cursor = new Vec2i(0, 0);
        int startPos = 0; //for scrolling

        // description
        GUCVisual descrBack;
        GUCVobVisual descrVis;

        // next inventories
        public GUCInventory Left;
        public GUCInventory Right;

        bool enabled = false;
        public bool Enabled
        {
            get { return this.enabled; }
            set
            {
                enabled = value;
                if (enabled)
                {
                    SelectSlot();
                    back.Show();
                    this.Show(); //to the front!
                }
                else
                {
                    slots[cursor.X, cursor.Y].IsSelected = false;
                    back.Hide();
                    descrBack.Hide();
                    descrVis.Hide();
                }
            }
        }

        #region Constructor

        public GUCInventory(int x, int y, int cols, int rows, string backTex = TextureBackgroundDefault)
        {
            // create the background
            back = new GUCVisual(x, y, cols * SlotSize, rows * SlotSize);
            back.SetBackTexture(backTex);

            // create the slots
            slots = new Slot[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    slots[i, j] = new Slot(x + i * SlotSize, y + j * SlotSize);
                }
            }

            // create the description
            descrBack = new GUCVisual((GetScreenSize().X - DescriptionBoxWidth) / 2, GetScreenSize().Y - DescriptionBoxHeight - 30, DescriptionBoxWidth, DescriptionBoxHeight);
            descrBack.SetBackTexture(backTex); // "Inv_Desc.tga");

            descrBack.CreateTextCenterX("", 10); // title

            const int descrTextDist = FontsizeDefault - 3;
            for (int i = 0; i < 6; i++) // six info rows
            {
                descrBack.CreateText("", 20, 60 + i * descrTextDist);
                GUCVisualText count = descrBack.CreateText("", DescriptionBoxWidth - 20, 60 + i * descrTextDist);
                count.Format = GUCVisualText.TextFormat.Right;
            }

            descrVis = new GUCVobVisual(GetScreenSize().X / 2 + 160, GetScreenSize().Y - 128 - 48, 128, 128);

            // create the right info box
            rightBack = new GUCVisual(x + (cols - 2) * SlotSize, y - 20 - 35, 2 * SlotSize, 35);
            rightBack.SetBackTexture(backTex);
            rightVis = new GUCVisual(x + (cols - 2) * SlotSize, y - 20 - 35, 2 * SlotSize, 35);
            rightVis.SetBackTexture(TextureTitle);
            rightText = rightVis.CreateText("Gold: 0");
            RightInfoBox = "GOLD";

            // create the left info box
            leftBack = new GUCVisual(x, y - 20 - 35, 2 * SlotSize, 35);
            leftBack.SetBackTexture(backTex);
            leftVis = new GUCVisual(x, y - 20 - 35, 2 * SlotSize, 35);
            leftVis.SetBackTexture(TextureTitle);
            leftText = leftVis.CreateText("Gewicht: 0");
            LeftInfoBox = "WEIGHT";
        }

        #endregion

        public ItemInst GetSelectedItem()
        {
            return slots[cursor.X, cursor.Y].Item;
        }

        #region Navigation

        public void SetCursor(int x, int y)
        {
            if (enabled)
            {
                slots[cursor.X, cursor.Y].IsSelected = false;
            }

            int newX = x;
            int newY = y;
            int shownCount = contents.Count - startPos * slots.GetLength(0);

            if (newX < 0)
            {
                newX = 0;
                if (Left != null)
                {
                    // enter another inventory
                    Left.EnterAt(Left.slots.GetLength(0) - 1, y);
                    Left.Enabled = true;
                    this.Enabled = false;
                }
                else if (newY > 0)
                {
                    // switch to the very right and 1 row up, like Gothic
                    newX = slots.GetLength(0) - 1;
                    newY--;
                }

            }
            else if (newX >= slots.GetLength(0) || (cursor.Y - newY == 0 && slots[newX, newY].Item == null)) // moved to border or empty slot(make sure it was move in X)
            {
                newX = slots.GetLength(0) - 1;
                if (Right != null)
                {
                    Right.EnterAt(0, y);
                    Right.Enabled = true;
                    this.Enabled = false;
                }
                else if (newY < shownCount / slots.GetLength(0))
                {
                    // switch to the very left and 1 row down, like Gothic
                    newY++;
                    newX = 0;
                }
            }

            if (contents.Count > 0)
            {
                if (newY < 0)
                {
                    newY = 0;
                    if (startPos > 0)
                    {
                        startPos--; //scroll up
                    }
                    else if (newX > 0)
                    {
                        newX--; //already on top, move to the left
                    }
                }
                else if (newY >= slots.GetLength(1))
                {
                    newY = slots.GetLength(1) - 1;
                    if (contents.Count > (startPos + slots.GetLength(1)) * slots.GetLength(0))
                    {
                        startPos++; //there are more items outside our current view, scroll down
                    }
                    else if (newX > 0)
                    {
                        newX--; //already on bottom, move to the left
                    }
                }

                if (shownCount < newY * slots.GetLength(0) + newX + 1) //cursor position is beyond the item list
                {
                    if (shownCount % slots.GetLength(0) - 1 > newX)
                    {
                        // pressing 'down' results in moving to the right border
                        newY--;
                        newX++;
                    }
                    else
                    {
                        newX = shownCount % slots.GetLength(0) - 1;
                        newY = shownCount / slots.GetLength(0);
                        if (newX < 0)
                        {
                            // X == -1 -> set to the very right
                            // Y is now one row to much => -1
                            newX = slots.GetLength(0) - 1;
                            newY--;
                        }
                    }
                }
            }
            else
            {
                newX = 0;
                newY = 0;
            }

            cursor.X = newX;
            cursor.Y = newY;

            UpdateSlots();
            if (enabled)
            {
                SelectSlot();
            }
        }

        public void EnterAt(int x, int y)
        {
            // checks if the entered position is valid
            // sets cursor to the next valid position
            for (int X = x; X >= 0; --X)
            {
                for (int Y = y; Y >= 0; --Y)
                {
                    if (X < slots.GetLength(0) && Y < slots.GetLength(1))
                    {
                        if (slots[X, Y].Item != null)
                        {
                            cursor.X = X;
                            cursor.Y = Y;
                            SelectSlot();
                            return;
                        }
                    }
                }
            }

            cursor.X = 0;
            cursor.Y = 0;
            SelectSlot();
        }

        void SelectSlot()
        {
            slots[cursor.X, cursor.Y].IsSelected = true;

            ItemInst selItem = GetSelectedItem();

            if (selItem == null)
            {
                descrBack.Hide();
                descrVis.Hide();
            }
            else
            {
                List<GUCVisualText> texts = descrBack.Texts;
                var def = selItem.Definition;

                //set description name
                texts[0].Text = def.Name;

                if (def.Damage != 0)
                {
                    texts[3].Text = "Schaden";
                    texts[4].Text = def.Damage.ToString();
                }
                else
                {
                    texts[3].Text = texts[4].Text = "";
                }

                if (def.Range != 0)
                {
                    texts[5].Text = "Reichweite";
                    texts[6].Text = def.Range.ToString();
                }
                else
                {
                    texts[5].Text = texts[6].Text = "";
                }

                if (def.Protection != 0)
                {
                    texts[7].Text = "Schutz vor Waffen";
                    texts[8].Text = def.Protection.ToString();
                }
                else
                {
                    texts[7].Text = texts[8].Text = "";
                }

                //visual vob
                descrVis.SetVisual(selItem.ModelDef.Visual);

                //show description
                def.PositionInVobVisual(descrVis);

                descrBack.Show();
                descrVis.Show();
            }
        }

        public void KeyPressed(VirtualKeys key)
        {
            if (!enabled)
                return;

            switch (key)
            {
                case VirtualKeys.Right:
                    SetCursor(cursor.X + 1, cursor.Y);
                    return;
                case VirtualKeys.Left:
                    SetCursor(cursor.X - 1, cursor.Y);
                    return;
                case VirtualKeys.Up:
                    SetCursor(cursor.X, cursor.Y - 1);
                    return;
                case VirtualKeys.Down:
                    SetCursor(cursor.X, cursor.Y + 1);
                    return;

                // left/right
                case VirtualKeys.Numpad4:
                    descrVis.OffsetX -= 5;
                    break;
                case VirtualKeys.Numpad6:
                    descrVis.OffsetX += 5;
                    break;

                // up / down
                case VirtualKeys.Numpad8:
                    descrVis.OffsetY += 5;
                    break;
                case VirtualKeys.Numpad2:
                    descrVis.OffsetY -= 5;
                    break;

                // forth / back
                case VirtualKeys.Add:
                    descrVis.OffsetZ -= 5;
                    break;
                case VirtualKeys.Subtract:
                    descrVis.OffsetZ += 5;
                    break;

                // rotation yaw
                case VirtualKeys.Numpad7:
                    descrVis.RotationYaw -= Angles.Deg2Rad(5);
                    break;
                case VirtualKeys.Numpad9:
                    descrVis.RotationYaw += Angles.Deg2Rad(5);
                    break;

                // rotation roll
                case VirtualKeys.Numpad1:
                    descrVis.RotationRoll -= Angles.Deg2Rad(5);
                    break;
                case VirtualKeys.Numpad3:
                    descrVis.RotationRoll += Angles.Deg2Rad(5);
                    break;

                // rotation pitch
                case VirtualKeys.Divide:
                    descrVis.RotationPitch += Angles.Deg2Rad(5);
                    break;
                case VirtualKeys.Multiply:
                    descrVis.RotationPitch -= Angles.Deg2Rad(5);
                    break;
                default:
                    return;
            }

            Log.Logger.Log("Offset: ({0} {1} {2}) Rotation: ({3} {4} {5})",
                            descrVis.OffsetX, descrVis.OffsetY, descrVis.OffsetZ,
                            descrVis.RotationPitch, descrVis.RotationYaw, descrVis.RotationRoll);
        }

        #endregion

        VobSystem.Instances.ItemContainers.ScriptInventory inventory = null;
        public void SetContents(VobSystem.Instances.ItemContainers.ScriptInventory inventory)
        {
            this.inventory = inventory;

            UpdateSlots(); // update slot visuals

            if (enabled)
            {
                SetCursor(cursor.X, cursor.Y); //update cursor
            }
        }

        public void UpdateAmounts()
        {
            for (int y = 0; y < slots.GetLength(1); y++)
                for (int x = 0; x < slots.GetLength(0); x++)
                {
                    slots[x, y].UpdateSlotAmount();
                }
        }

        public void UpdateEquipment()
        {
            for (int y = 0; y < slots.GetLength(1); y++)
                for (int x = 0; x < slots.GetLength(0); x++)
                {
                    slots[x, y].UpdateBackTex();
                }
        }

        List<ItemInst> contents = new List<ItemInst>();
        void UpdateSlots()
        {
            contents.Clear();
            if (this.inventory != null)
                this.inventory.ForEachItem(item => contents.Add(item));
            contents.Sort(ItemSort);

            int i = startPos * slots.GetLength(0);
            for (int y = 0; y < slots.GetLength(1); y++)
                for (int x = 0; x < slots.GetLength(0); x++)
                {
                    if (i < contents.Count)
                    {
                        slots[x, y].SetItem(contents[i]);
                        slots[x, y].UpdateSlotAmount();
                        slots[x, y].UpdateBackTex();
                    }
                    else
                    {
                        slots[x, y].SetItem(null);
                    }
                    i++;
                }
        }

        #region Show & Hide

        public override void Show()
        {
            foreach (Slot slot in slots)
                slot.Show();

            if (RightInfoBox != null && RightInfoBox.Length > 0)
            {
                rightBack.Show();
                rightVis.Show();
            }

            if (LeftInfoBox != null && LeftInfoBox.Length > 0)
            {
                leftBack.Show();
                leftVis.Show();
            }
        }

        public override void Hide()
        {
            Enabled = false;
            foreach (Slot slot in slots)
                slot.Hide();

            rightBack.Hide();
            rightVis.Hide();
            leftBack.Hide();
            leftVis.Hide();
        }

        #endregion

        #region Sorting

        static int ItemSort(ItemInst item1, ItemInst item2)
        {
            return item1.ItemType.CompareTo(item2.ItemType);
        }

        #endregion
    }
}
