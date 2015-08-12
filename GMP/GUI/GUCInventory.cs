using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using WinApi.User.Enumeration;
using GUC.Enumeration;
using Gothic.zClasses;
using GUC.Types;

namespace GUC.Client.GUI
{
    class GUCInventory : GUCView, InputReceiver
    {
        #region slot
        const int slotSize = 70;

        class Slot : GUCView
        {
            GUCVisual back;
            GUCVisualVob vis;
            GUCVisualText amount;
            oCItem thisVob;
            bool shown = false;

            public Slot(int x, int y)
            {
                back = new GUCVisual(x, y, slotSize, slotSize);
                back.SetBackTexture("Inv_Slot.tga");

                vis = new GUCVisualVob(x, y, slotSize, slotSize);
                thisVob = oCItem.Create(Program.Process);
                vis.SetVob(thisVob);

                amount = vis.CreateText("", slotSize - 5, slotSize - 5 - FontsizeDefault);
                amount.Format = GUCVisualText.TextFormat.Right;
            }

            KeyValuePair<ItemInstance, int> item = new KeyValuePair<ItemInstance,int>();
            public KeyValuePair<ItemInstance, int> Item
            {
                get { return item; }
                set
                {
                    item = value;

                    if (item.Key == null)
                    {
                        vis.Hide();
                    }
                    else
                    {
                        thisVob.SetVisual(item.Key.Visual);
                        thisVob.MainFlag = (int)item.Key.MainFlags; //for proper item rotation
                        thisVob.Flags = (int)item.Key.Flags;
                        amount.Text = item.Value > 1 ? item.Value.ToString() : "";
                        if (shown) vis.Show();
                    }              
                }
            }

            public void Select()
            {
                back.SetBackTexture("Inv_Slot_Highlighted.tga");
                Program.Process.Write(110, thisVob.Address + (int)oCItem.Offsets.inv_zbias);
            }

            public void Deselect()
            {
                back.SetBackTexture("Inv_Slot.tga");
                Program.Process.Write(0, thisVob.Address + (int)oCItem.Offsets.inv_zbias);
            }

            public override void Show()
            {
                back.Show();
                if (item.Key != null)
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

        GUCVisual rightBack;
        GUCVisual rightVis;
        GUCVisualText rightText;
        public string RightInfoBox;

        GUCVisual leftBack;
        GUCVisual leftVis;
        GUCVisualText leftText;
        public string LeftInfoBox;

        GUCVisual back;
        Slot[,] slots;
        Vec2i cursor = new Vec2i();
        int startPos = 0; //for scrolling

        GUCVisual descrBack;
        GUCVisualVob descrVis;
        oCItem descrVob;

        bool enabled = false;
        public bool Enabled
        {
            get { return enabled; }
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
                    slots[cursor.X, cursor.Y].Deselect();
                    back.Hide();
                    descrBack.Hide();
                    descrVis.Hide();
                }
            }
        }

        public GUCInventory(int x, int y, int cols, int rows)
            : this(x, y, cols, rows, "Inv_Back.tga")
        {

        }

        public GUCInventory(int x, int y, int cols, int rows, string backTex)
        {
            back = new GUCVisual(x, y, cols * slotSize, rows * slotSize);
            back.SetBackTexture(backTex);
            slots = new Slot[cols, rows];
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    slots[i, j] = new Slot(x + i * slotSize, y + j * slotSize);
                }
            }

            const int descrWidth = 720;
            const int descrHeight = 162;
            const int descrTextDist = FontsizeDefault - 3;

            descrBack = new GUCVisual((GetScreenSize()[0] - descrWidth) / 2, GetScreenSize()[1] - descrHeight - 30, descrWidth, descrHeight);
            descrBack.SetBackTexture(backTex);//"Inv_Desc.tga");
            descrBack.CreateTextCenterX("", 10);
            for (int i = 0; i < 6; i++)
            {
                descrBack.CreateText("", 20, 60 + i * descrTextDist);
                GUCVisualText count = descrBack.CreateText("", descrWidth - 20, 60 + i * descrTextDist);
                count.Format = GUCVisualText.TextFormat.Right;
            }

            descrVis = new GUCVisualVob(GetScreenSize()[0]/2 + 160, GetScreenSize()[1] - 128 - 48, 128, 128);
            descrVob = oCItem.Create(Program.Process);
            descrVis.SetVob(descrVob);

            rightBack = new GUCVisual(x + (cols - 2) * slotSize, y - 20 - 35, 2 * slotSize, 35);
            rightBack.SetBackTexture(backTex);
            rightVis = new GUCVisual(x + (cols - 2) * slotSize, y - 20 - 35, 2 * slotSize, 35);
            rightVis.SetBackTexture("Inv_Title.tga");
            rightText = rightVis.CreateText("");
            RightInfoBox = "GOLD";

            leftBack = new GUCVisual(x, y - 20 - 35, 2 * slotSize, 35);
            leftBack.SetBackTexture(backTex);
            leftVis = new GUCVisual(x, y - 20 - 35, 2 * slotSize, 35);
            leftVis.SetBackTexture("Inv_Title.tga");
            leftText = leftVis.CreateText("");
            LeftInfoBox = "WEIGHT";
        }

        public KeyValuePair<ItemInstance,int> SelectedItem
        {
            get { return slots[cursor.X, cursor.Y].Item; }
        }

        #region Navigation

        public void SetCursor(int x, int y)
        {
            if (enabled)
            {
                slots[cursor.X, cursor.Y].Deselect();
            }


            int newX = x;
            int newY = y;

            if (newX < 0)
            {
                //if left != null
                newX = 0;
            }
            else if (newX >= slots.GetLength(0))
            {
                //if right != null
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

            SelectSlot();
        }

        void SelectSlot()
        {
            ItemInstance instance = slots[cursor.X, cursor.Y].Item.Key;
            int amount = slots[cursor.X, cursor.Y].Item.Value;

            slots[cursor.X, cursor.Y].Select();

            if (instance == null)
            {
                descrBack.Hide();
                descrVis.Hide();
            }
            else
            {
                //set description box
                if (instance.Name != null && instance.Name.Length > 0)
                {
                    descrBack.Texts[0].Text = instance.Name.Value;
                }

                for (int i = 0; i < 6; i++)
                {
                    if (instance.Text[i] != null && instance.Text[i].Length > 0)
                    {
                        descrBack.Texts[2*i+1].Text = instance.Text[i].Value;
                        descrBack.Texts[2*i+2].Text = instance.Count[i].ToString();
                    }
                }

                descrVob.SetVisual(instance.Visual);
                descrVob.MainFlag = (int)instance.MainFlags;
                descrVob.Flags = (int)instance.Flags;

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

        List<KeyValuePair<ItemInstance, int>> contents;
        public void SetContents(Dictionary<ItemInstance, int> items)
        {
            contents = items.ToList();
            contents.Sort(inventoryComparer);

            SetCursor(cursor.X, cursor.Y); //update cursor

            UpdateSlots();

            KeyValuePair<ItemInstance,int> gold = contents.Find(i => i.Key.Name.Value == "Gold");
            int weight = 0;
            foreach (KeyValuePair<ItemInstance,int> pair in contents)
            {
                weight += pair.Key.Weight * pair.Value;
            }

            if (RightInfoBox == "GOLD")
            {
                rightText.Text = "Gold: " + gold.Value;
            }
            else if (RightInfoBox == "WEIGHT")
            {
                rightText.Text = weight + "/100";
            }

            if (LeftInfoBox == "GOLD")
            {
                leftText.Text = "Gold: " + gold.Value;
            }
            else if (LeftInfoBox == "WEIGHT")
            {
                leftText.Text = weight + "/1000";
            }
        }

        void UpdateSlots()
        {
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
                        slots[x, y].Item = new KeyValuePair<ItemInstance, int>();
                    }
                    i++;
                }
        }

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

        #region sorting

        /*static List<MainFlags> sortList = new List<MainFlags>() { MainFlags.ITEM_KAT_NF,
                                                                  MainFlags.ITEM_KAT_FF,
                                                                  MainFlags.ITEM_KAT_MUN,
                                                                  MainFlags.ITEM_KAT_FOOD,
                                                                  MainFlags.ITEM_KAT_RUNE,
                                                                  MainFlags.ITEM_KAT_ARMOR,
                                                                  MainFlags.ITEM_KAT_DOCS,
                                                                  MainFlags.ITEM_KAT_POTIONS,
                                                                  MainFlags.ITEM_KAT_MAGIC };*/

        static InventoryComparer inventoryComparer = new InventoryComparer();
        class InventoryComparer : IComparer<KeyValuePair<ItemInstance, int>>
        {
            public int Compare(KeyValuePair<ItemInstance, int> a, KeyValuePair<ItemInstance, int> b)
            {
               /* int aIndex = sortList.IndexOf(a.Key.MainFlags);
                int bIndex = sortList.IndexOf(a.Key.MainFlags);
                if (aIndex < 0) aIndex = sortList.Count;
                if (bIndex < 0) bIndex = sortList.Count;

                if (aIndex.CompareTo(bIndex) != 0)
                {
                    return aIndex.CompareTo(bIndex);
                }*/
                return a.Key.Count[0].CompareTo(b.Key.Count[0]); //just sort by something
            }
        }
        #endregion
    }
}
