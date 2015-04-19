using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.Ingame.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame.GUI
{
    class GUCMenuInventory : GUCMInputReceiver, GUCMVisual
    {
        class Slot
        {
            public Item item
            {
                private set;
                get;
            }
            GUCMenuItem mItem;
            GUCMenuTexture background;
            GUCMenuText amount;
            Vec2i pos;

            public Slot(int x, int y)
            {
                pos = new Vec2i(x, y);
                background = new GUCMenuTexture("Inv_Slot.tga", x, y, 70, 70);
                mItem = new GUCMenuItem(x-1, y-1, 72, 72);
                amount = new GUCMenuText("", x, y);
            }

            public void SetItem(Item item)
            {
                this.item = item;
                if (item == null)
                {
                    mItem.SetItem(null);
                    amount.SetText("");
                }
                else
                {
                    mItem.SetItem(new oCItem(Process.ThisProcess(), item.Address));
                    if (item.Amount > 1)
                    {
                        amount.SetText(item.Amount.ToString());
                        amount.SetPos(pos.X+61-IngameInput.StringPixelWidth(item.Amount.ToString()), pos.Y+46);
                    }
                    else
                    {
                        amount.SetText("");
                    }
                }
            }

            public void Mark()
            {
                background.SetTexture("Inv_Slot_Highlighted.tga");
                mItem.SetSize(pos.X - 9, pos.Y - 9, 88, 88);
            }

            public void Demark()
            {
                background.SetTexture("Inv_Slot.tga");
                mItem.SetSize(pos.X-1, pos.Y-1, 72, 72);
            }

            public void Show()
            {
                background.Show();
                mItem.Show();
                amount.Show();
            }

            public void Hide()
            {
                background.Hide();
                mItem.Hide();
                amount.Hide();
            }
        }

        Process proc;

        GUCMenuTexture background;

        public GUCMenuInventory left;
        public GUCMenuInventory right;

        List<Item> itemList;
        List<List<Slot>> slots;

        int startPos;
        Vec2i cursorPos;
        public bool enabled
        {
            get;
            private set;
        }

        public delegate void PressedCTRLHandler(Item item);
        public event PressedCTRLHandler OnPressCTRL;

        public GUCMenuInventory(int x, int y, int wNum, int hNum, string tex)
        {
            proc = Process.ThisProcess();

            //Create background
            background = new GUCMenuTexture(tex, x, y, wNum * 70, hNum * 70);

            //create slots
            slots = new List<List<Slot>>();
            for (int i = 0; i < wNum; i++)
            {
                List<Slot> s = new List<Slot>();
                for (int j = 0; j < hNum; j++)
                    s.Add(new Slot(x + i * 70, y + j * 70));
                slots.Add(s);
            }

            cursorPos = new Vec2i(0, 0);
            startPos = 0;
            enabled = false;
        }

        public void AddItem(Item item)
        {
            itemList.Add(item);
            UpdateSlots();
        }

        public void RemoveItem(Item item)
        {
            itemList.Remove(item);
            UpdateSlots();
            SetCursor(cursorPos.X, cursorPos.Y);
        }

        private void RemoveCursor()
        {
            if (enabled)
            {
                slots[cursorPos.X][cursorPos.Y].Demark();
                enabled = false;
            }
        }

        public void SetCursor(int x, int y)
        {
            if (enabled)
            {
                slots[cursorPos.X][cursorPos.Y].Demark();
            }
            else
            {
                enabled = true;
            }

            int newX = x;
            int newY = y;

            if (x < 0)
            {
                newX = 0;
            }
            else if (x >= slots.Count)
            {
                newX = slots.Count - 1;
            }

            if (y < 0)
            {
                y = 0;
            }
            else if (y >= slots[newX].Count)
            {
                newY = slots[newX].Count - 1;
            }

            for (int i = newY; i >= 0; i--)
            {
                for (int j = i == newY ? newX : 0; j >= 0; j--)
                {
                    if (slots[j][i].item != null)
                    {
                        cursorPos.X = j;
                        cursorPos.Y = i;
                        slots[cursorPos.X][cursorPos.Y].Mark();
                        return;
                    }
                }
            }
            cursorPos.X = 0;
            cursorPos.Y = 0;
            slots[cursorPos.X][cursorPos.Y].Mark();
        }

        private void UpdateSlots()
        {
            int i = slots.Count * startPos;
            for (int y = 0; y < slots[0].Count; y++)
            {
                for (int x = 0; x < slots.Count; x++)
                {
                    if (i < itemList.Count)
                    {
                        slots[x][y].SetItem(itemList[i]);
                        i++;
                    }
                    else
                    {
                        slots[x][y].SetItem(null);
                    }
                }
            }
        }

        public void Hide()
        {
            background.Hide();
            foreach (List<Slot> s in slots)
                foreach (Slot slot in s)
                    slot.Hide();
        }

        public void Open(List<Item> list)
        {
            startPos = 0;
            if (list == null)
            {
                itemList = new List<Item>();
            }
            else
            {
                itemList = new List<Item>(list);
            }
            UpdateSlots();
            Show();            
        }

        public void Show()
        {
            background.Show();
            foreach (List<Slot> s in slots)
                foreach (Slot slot in s)
                    slot.Show();
        }

        public void KeyPressed(int key)
        {
            if (!enabled)
                return;

            int x = cursorPos.X;
            int y = cursorPos.Y;
            if (key == (int)VirtualKeys.Up)
            {
                if (y > 0)
                {
                    y--;
                }
                else if (y == 0)
                {
                    if (startPos > 0)
                    {
                        startPos--;
                    }
                    else
                    {
                        if (x > 0)
                        {
                            x--;
                        }
                    }
                }
            }
            else if (key == (int)VirtualKeys.Down)
            {
                if (y < slots[x].Count - 1)
                {
                    y++;
                }
                else if (y == slots[x].Count - 1)
                {
                    if (itemList.Count > (startPos + slots[x].Count) * slots.Count)
                    {
                        startPos++;
                    }
                    else
                    {
                        if (x < slots.Count - 1)
                        {
                            x++;
                        }
                    }
                }
            }
            else if (key == (int)VirtualKeys.Left)
            {
                if (x > 0)
                {
                    x--;
                }
                else if (left != null)
                {
                    RemoveCursor();
                    left.SetCursor(0xFFFF, y);
                    return;
                }
            }
            else if (key == (int)VirtualKeys.Right)
            {
                if (x < slots.Count - 1)
                {
                    x++;
                    if (right != null && slots[x][y].item == null)
                    {
                        RemoveCursor();
                        right.SetCursor(0, y);
                        return;
                    }
                }
                else if (right != null)
                {
                    RemoveCursor();
                    right.SetCursor(0, y);
                    return;
                }

            }
            else 
            { 
                if (key == (int)VirtualKeys.Control)
                {
                    if (OnPressCTRL != null)
                    {
                        OnPressCTRL(slots[cursorPos.X][cursorPos.Y].item);
                    }
                }
                return; 
            }
            SetCursor(x, y);
            UpdateSlots();
        }

        public void Update(long ticks)
        {

        }
    }
}
