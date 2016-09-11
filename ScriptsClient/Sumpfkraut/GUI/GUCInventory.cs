using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.WorldObjects.Collections;

namespace GUC.Scripts.Sumpfkraut.GUI
{
    public class GUCInventory : GUCView
    {
        public const int DescriptionBoxWidth = 720;
        public const int DescriptionBoxHeight = 162;
        public const int SlotSize = 70;

        #region Slot

        class Slot : GUCView
        {
            public GUCVisual back;
            GUC3DVisual vis;
            GUCVisualText amount;
            bool shown = false;

            const string bgTex = "Inv_Slot.tga";
            const string bgHighlightedTex = "Inv_Slot_Highlighted.tga";

            const string eqTex = "Inv_Slot_Equipped.tga";
            const string eqHighlightedTex = "Inv_Slot_Equipped_Highlighted.tga";

            public Slot(int x, int y)
            {
                back = new GUCVisual(x, y, SlotSize, SlotSize);
                back.SetBackTexture(bgTex);

                vis = new GUC3DVisual(x, y, SlotSize, SlotSize);

                amount = vis.CreateText("", SlotSize - 5, SlotSize - 5 - FontsizeDefault);
                amount.Format = GUCVisualText.TextFormat.Right;
            }

            ItemInst item = null;
            public ItemInst Item
            {
                get { return item; }
                set
                {
                    if (this.item != value)
                    {
                        item = value;
                        UpdateBackTex();
                        if (item == null)
                        {
                            vis.SetVisual(string.Empty);
                            vis.Hide();
                        }
                        else
                        {
                            vis.SetVisual(item.ModelDef.Visual);
                            int num = item.Amount;
                            if (num > 1)
                            {
                                amount.Text = num.ToString();
                            }
                            else
                            {
                                amount.Text = string.Empty;
                            }

                            if (shown)
                            {
                                vis.Show();
                            }
                        }
                    }
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

            void UpdateBackTex()
            {
                if (this.isSelected)
                {
                    back.SetBackTexture((item != null && item.IsEquipped) ? eqHighlightedTex : bgHighlightedTex);
                }
                else
                {
                    back.SetBackTexture((item != null && item.IsEquipped) ? eqTex : bgTex);
                }
            }

            public override void Show()
            {
                back.Show();
                if (item != null)
                {
                    vis.Show();
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
        GUC3DVisual descrVis;

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

        public GUCInventory(int x, int y, int cols, int rows, string backTex = "Inv_Back.tga")
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
            descrBack = new GUCVisual((GetScreenSize()[0] - DescriptionBoxWidth) / 2, GetScreenSize()[1] - DescriptionBoxHeight - 30, DescriptionBoxWidth, DescriptionBoxHeight);
            descrBack.SetBackTexture(backTex); // "Inv_Desc.tga");

            descrBack.CreateTextCenterX("", 10); // title

            const int descrTextDist = FontsizeDefault - 3;
            for (int i = 0; i < 6; i++) // six info rows
            {
                descrBack.CreateText("", 20, 60 + i * descrTextDist);
                GUCVisualText count = descrBack.CreateText("", DescriptionBoxWidth - 20, 60 + i * descrTextDist);
                count.Format = GUCVisualText.TextFormat.Right;
            }

            descrVis = new GUC3DVisual(GetScreenSize()[0] / 2 + 160, GetScreenSize()[1] - 128 - 48, 128, 128);

            // create the right info box
            rightBack = new GUCVisual(x + (cols - 2) * SlotSize, y - 20 - 35, 2 * SlotSize, 35);
            rightBack.SetBackTexture(backTex);
            rightVis = new GUCVisual(x + (cols - 2) * SlotSize, y - 20 - 35, 2 * SlotSize, 35);
            rightVis.SetBackTexture("Inv_Title.tga");
            rightText = rightVis.CreateText("");
            RightInfoBox = "GOLD";

            // create the left info box
            leftBack = new GUCVisual(x, y - 20 - 35, 2 * SlotSize, 35);
            leftBack.SetBackTexture(backTex);
            leftVis = new GUCVisual(x, y - 20 - 35, 2 * SlotSize, 35);
            leftVis.SetBackTexture("Inv_Title.tga");
            leftText = leftVis.CreateText("");
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

            if (newX < 0)
            {
                if (Left != null)
                {
                    Left.EnterAt(Left.slots.GetLength(0) - 1, y);
                    Left.Enabled = true;
                    this.Enabled = false;
                }
                newX = 0;
            }
            else if (newX >= slots.GetLength(0) || (cursor.Y - newY == 0 && slots[newX, newY].Item == null)) // moved to border or empty slot(make sure it was move in X)
            {
                if (Right != null)
                {
                    Right.EnterAt(0, y);
                    Right.Enabled = true;
                    this.Enabled = false;
                }
                newX = slots.GetLength(0) - 1;
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

                int shownCount = contents.Count - startPos * slots.GetLength(0);
                if (shownCount < newY * slots.GetLength(0) + newX + 1) //cursor position is beyond the item list
                {
                    newY = shownCount / slots.GetLength(0);
                    newX = shownCount % slots.GetLength(0) - 1;

                    if (newX < 0) newX = slots.GetLength(0) - 1;
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
                //set description name
                descrBack.Texts[0].Text = selItem.Definition.Name;

                //standard description
                /*for (int i = 0; i < 4; i++)
                {
                    descrBack.Texts[2 * i + 3].Text = selItem.text[i];

                    if (selItem.count[i] > 0)
                    {
                        descrBack.Texts[2 * i + 4].Text = selItem.count[i].ToString();
                    }
                    else
                    {
                        descrBack.Texts[2 * i + 4].Text = "";
                    }
                }

                //special line directly on top
                for (int i = 3; i >= 0; i--)
                {
                    if (selectedItem.text[i].Length == 0)
                    {
                        descrBack.Texts[2 * i + 3].Text = selectedItem.specialLine;
                        break;
                    }
                }

                //weight on bottom
                descrBack.Texts[11].Text = "Gewicht:";
                descrBack.Texts[12].Text = selectedItem.weight.ToString();*/

                //visual vob
                descrVis.SetVisual(selItem.ModelDef.Visual);

                //show
                descrVis.Show();
                descrBack.Show();
            }
        }

        public void KeyPressed(VirtualKeys key)
        {
            if (!enabled)
                return;

            if (key == VirtualKeys.Right)
            {
                SetCursor(cursor.X + 1, cursor.Y);
            }
            else if (key == VirtualKeys.Left)
            {
                SetCursor(cursor.X - 1, cursor.Y);
            }
            else if (key == VirtualKeys.Up)
            {
                SetCursor(cursor.X, cursor.Y - 1);
            }
            else if (key == VirtualKeys.Down)
            {
                SetCursor(cursor.X, cursor.Y + 1);
            }
        }

        #endregion

        VobSystem.Instances.ItemContainers.ScriptInventory inventory = null;
        public void SetContents(VobSystem.Instances.ItemContainers.ScriptInventory inventory)
        {
            this.inventory = inventory;

            if (enabled)
            {
                SetCursor(cursor.X, cursor.Y); //update cursor
            }

            UpdateSlots(); // update slot visuals
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
                        slots[x, y].Item = contents[i];
                    }
                    else
                    {
                        slots[x, y].Item = null;
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
