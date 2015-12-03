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
            public GUCVisual back;
            GUCVisualVob vis;
            GUCVisualText amount;
            oCItem thisVob;
            bool shown = false;

            string bgTex = "Inv_Slot.tga";
            string bgHighlightedTex = "Inv_Slot_Highlighted.tga";

            public Slot(int x, int y)
            {
                back = new GUCVisual(x, y, slotSize, slotSize);
                back.SetBackTexture(bgTex);

                vis = new GUCVisualVob(x, y, slotSize, slotSize);
                thisVob = oCItem.Create(Program.Process);
                vis.SetVob(thisVob);

                amount = vis.CreateText("", slotSize - 5, slotSize - 5 - FontsizeDefault);
                amount.Format = GUCVisualText.TextFormat.Right;
            }

            Item iItem = null;
            public Item item
            {
                get { return iItem; }
                set
                {
                    iItem = value;

                    if (iItem == null)
                    {
                        vis.Hide();
                    }
                    else
                    {
                        thisVob.SetVisual(iItem.Visual);
                        thisVob.MainFlag = (int)iItem.mainFlags; //for proper item rotation
                        thisVob.Flags = (int)iItem.flags;
                        amount.Text = iItem.Amount > 1 ? iItem.Amount.ToString() : "";
                        if (shown) vis.Show();
                    }
                }
            }

            public void Select()
            {
                back.SetBackTexture(bgHighlightedTex);
                Program.Process.Write(110, thisVob.Address + (int)oCItem.Offsets.inv_zbias);
            }

            public void Deselect()
            {
                back.SetBackTexture(bgTex);
                Program.Process.Write(0, thisVob.Address + (int)oCItem.Offsets.inv_zbias);
            }

            public override void Show()
            {
                back.Show();
                if (iItem != null)
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

        public GUCInventory left;
        public GUCInventory right;
        bool TradeAccepted = false; // AcceptedTradeBackgrounds shall not be unshown

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
                    //zERROR.GetZErr(Program.Process).Report(2, 'G', "Inventory enabled in " + name, 0, "GUCInventory.cs", 0);
                    this.Show(); //to the front!
                }
                else
                {
                    slots[cursor.X, cursor.Y].Deselect();
                    if (!TradeAccepted)
                    {
                        back.Hide();
                    }
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

            descrVis = new GUCVisualVob(GetScreenSize()[0] / 2 + 160, GetScreenSize()[1] - 128 - 48, 128, 128);
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

        public Item selectedItem { get; protected set; }

        #region Navigation

        public void SetCursor(int x, int y)
        {
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Attempt to Set Cursor at " + x.ToString() + "/" + y.ToString() + " in " + this.ToString(), 0, "GUCInventory.cs", 0);
            if (enabled)
            {
                slots[cursor.X, cursor.Y].Deselect();
            }

            int newX = x;
            int newY = y;

            if (newX < 0)
            {
                if (left != null)
                {
                    left.EnterAt(left.slots.GetLength(0) - 1, y);
                    left.Enabled = true;
                    this.Enabled = false;
                }
                newX = 0;
            }
            else if (newX >= slots.GetLength(0) || (cursor.Y - newY == 0 && slots[newX, newY].item == null)) // moved to border or empty slot(make sure it was move in X)
            {
                if (right != null)
                {
                    right.EnterAt(0, y);
                    right.Enabled = true;
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
            zERROR.GetZErr(Program.Process).Report(2, 'G', "EnterAT " + x.ToString() + "/" + y.ToString(), 0, "GUCInventory.cs", 0);
            // checks if the entered position is valid
            // sets cursor to the next valid position
            for (int X = x; X >= 0; --X)
            {
                for (int Y = y; Y >= 0; --Y)
                {
                    if (X < slots.GetLength(0) && Y < slots.GetLength(1))
                    {
                        if (slots[X, Y].item != null)
                        {
                            zERROR.GetZErr(Program.Process).Report(2, 'G', "Slot available at " + X.ToString() + "/" + Y.ToString(), 0, "GUCInventory.cs", 0);
                            cursor.X = X;
                            cursor.Y = Y;
                            SelectSlot();
                            return;
                        }
                    }
                }
            }
            zERROR.GetZErr(Program.Process).Report(2, 'G', "Set default Slot", 0, "GUCInventory.cs", 0);
            cursor.X = 0;
            cursor.Y = 0;
            SelectSlot();
        }

        void SelectSlot()
        {
            selectedItem = slots[cursor.X, cursor.Y].item;

            slots[cursor.X, cursor.Y].Select();

            if (selectedItem == null)
            {
                descrBack.Hide();
                descrVis.Hide();
            }
            else
            {
                //set description name
                descrBack.Texts[0].Text = selectedItem.description;

                //standard description
                for (int i = 0; i < 4; i++)
                {
                    descrBack.Texts[2 * i + 3].Text = selectedItem.text[i];

                    if (selectedItem.count[i] > 0)
                    {
                        descrBack.Texts[2 * i + 4].Text = selectedItem.count[i].ToString();
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
                descrBack.Texts[12].Text = selectedItem.weight.ToString();

                //visual vob
                descrVob.SetVisual(selectedItem.Visual);
                descrVob.MainFlag = (int)selectedItem.mainFlags;
                descrVob.Flags = (int)selectedItem.flags;

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

        List<Item> contents;
        public void SetContents(Dictionary<uint, Item> items)
        {
            contents = items.Values.ToList();
            contents.Sort(inventoryComparer); //sort items

            if (enabled)
            {
                SetCursor(cursor.X, cursor.Y); //update cursor // Mad: only if enabled
            }

            UpdateSlots(); // update slot visuals

            rightText.Text = GetInfoBoxValue(RightInfoBox);
            leftText.Text = GetInfoBoxValue(LeftInfoBox);
        }

        String GetInfoBoxValue(string text)
        {
            switch (text)
            {
                case "GOLD":
                    Item gold = contents.Find(i => i.name == "Gold");
                    return "Gold: " + (gold == null ? 0 : gold.Amount);
                case "WEIGHT":
                    int weight = 0;
                    for (int i = 0; i < contents.Count; i++)
                        weight += contents[i].weight * contents[i].Amount;
                    return String.Format("{0}/{1}", weight, 100); //FIXME: Show capacity
                default:
                    return text;
            }
        }

        public void SetAcceptStateColor(bool set)
        {
            // set == true -> set bg to accept state
            // set == false -> sets bg to normal state
            if (set)
            {
                TradeAccepted = true;
                back.SetBackTexture("Inv_Back_Buy.tga");
                leftBack.SetBackTexture("Inv_Back_Buy.tga");
                rightBack.SetBackTexture("Inv_Back_Buy.tga");
                back.Show();
            }
            else
            {
                TradeAccepted = false;
                back.SetBackTexture("Inv_Back_Sell.tga");
                leftBack.SetBackTexture("Inv_Back_Sell.tga");
                rightBack.SetBackTexture("Inv_Back_Sell.tga");
                if (!enabled)
                {
                    back.Hide();
                }
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
                        slots[x, y].item = contents[i];
                    }
                    else
                    {
                        slots[x, y].item = null;
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

        static List<int> sortList = new List<int>() { oCItem.MainFlags.ITEM_KAT_NF,
                                                      oCItem.MainFlags.ITEM_KAT_FF,
                                                      oCItem.MainFlags.ITEM_KAT_MUN,
                                                      oCItem.MainFlags.ITEM_KAT_FOOD,
                                                      oCItem.MainFlags.ITEM_KAT_RUNE,
                                                      oCItem.MainFlags.ITEM_KAT_ARMOR,
                                                      oCItem.MainFlags.ITEM_KAT_DOCS,
                                                      oCItem.MainFlags.ITEM_KAT_POTIONS,
                                                      oCItem.MainFlags.ITEM_KAT_MAGIC };

        static InventoryComparer inventoryComparer = new InventoryComparer();
        class InventoryComparer : IComparer<Item>
        {
            public int Compare(Item a, Item b)
            {
                int aIndex = sortList.IndexOf(a.mainFlags); // get sort priority
                int bIndex = sortList.IndexOf(a.mainFlags); // get sort priority
                if (aIndex < 0) aIndex = sortList.Count; // not in sortList, to the bottom
                if (bIndex < 0) bIndex = sortList.Count; // not in sortList, to the bottom

                //if (aIndex.CompareTo(bIndex) != 0)
                //{
                return aIndex.CompareTo(bIndex);
                //}
                //return a.count[0].CompareTo(b.count[0]); //just sort by something
            }
        }
        #endregion
    }
}
