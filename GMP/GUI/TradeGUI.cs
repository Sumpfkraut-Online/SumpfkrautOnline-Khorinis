using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using WinApi;
using WinApi.User.Enumeration;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Enumeration;
using Gothic.zClasses;
using Gothic.zTypes;
using GUC.Hooks;
using RakNet;


namespace GUC.GUI
{
    class InvSlot
    {
        public Item item;
        private Process process;
        private oCItem oCItem;
        private zCView itemView;
        private zCView thisView;
        private zCViewText amountText;
        private int[] pos;
        private int[] size;

        public InvSlot(zCView parent, int x, int y)
        {
            process = Process.ThisProcess();
            pos = new int[] { x, y };
            size = InputHooked.PixelToVirtual(process, new int[] { 70, 70 });

            //Slot graphics
            thisView = zCView.Create(process, x, y, x + size[0], y + size[1]);
            using (zString texture = zString.Create(process, "Inv_Slot.tga"))
            {
                thisView.InsertBack(texture);
            }
            parent.InsertItem(thisView, 0);

            //Item
            itemView = zCView.Create(process, x, y, x + size[0], y + size[1]);
            itemView.FillZ = true;
            itemView.Blit();
            parent.InsertItem(itemView, 0);
        }

        public void SetItem(Item item)
        {
            RemoveItem();


            if (item.Amount > 1)
            {
                using (zString num = zString.Create(process, item.Amount.ToString()))
                {
                    amountText = thisView.CreateText((61 - thisView.Font.GetFontX(num)) * 8192 / 70, 46 * 8192 / 70, num);
                    amountText.Timed = 0;
                    amountText.Timer = -1;
                }
            }

            this.oCItem = new oCItem(process, item.Address);
            hItem.renderList.Add(itemView, this.oCItem);
            this.item = item;
        }

        public void RemoveItem()
        {
            if (item != null)
            {
                item = null;
            }
            if (oCItem != null)
            {
                hItem.renderList.Remove(itemView); //remove old item
                oCItem = null;
            }
            if (amountText != null)
            {
                amountText.Timed = 1;
                amountText.Timer = 0;
                amountText = null;
            }
        }

        public void Mark()
        {
            using (zString tex = zString.Create(process, "INV_SLOT_HIGHLIGHTED.TGA"))
                thisView.InsertBack(tex);

            itemView.SetPos(pos[0] - InputHooked.PixelToVirtualX(process, 7), pos[1] - InputHooked.PixelToVirtualY(process, 7));
            itemView.SetSize(size[0] + InputHooked.PixelToVirtualX(process, 14), size[1] + InputHooked.PixelToVirtualY(process, 14));
        }

        public void Demark()
        {
            using (zString tex = zString.Create(process, "INV_SLOT.TGA"))
                thisView.InsertBack(tex);

            itemView.SetPos(pos[0], pos[1]);
            itemView.SetSize(size[0], size[1]);
        }
    }

    class TradeGUI : InputReceiver
    {
        Process process;

        int startTradeKey;

        Player trader;
        Boolean trading;

        Action<int> RequestTrade;
        Action<int> OfferItem;
        Action<int> TakeBackItem;

        List<Item> invItems;
        List<Item> sellItems;
        List<Item> buyItems;

        int scrollPos;
        int[] cursor;
        List<List<InvSlot>> inventory;
        List<List<InvSlot>> sellInv;
        List<List<InvSlot>> buyInv;

        zCView invBack;
        zCView sellBack;
        zCView buyBack;

        zCView thisView;

        public TradeGUI(Action<int> Request, Action<int> Offer, Action<int> TakeBack)
        {
            process = Process.ThisProcess();
            startTradeKey = (int)VirtualKeys.T;
            trading = false;
            trader = null;

            RequestTrade = Request;
            OfferItem = Offer;
            TakeBackItem = TakeBack;

            scrollPos = 0;
            cursor = new int[2] { 0, 0 };
            CreateTradeMenu();
            InputHooked.receivers.Add(this);
        }

        public void KeyReleased(int key)
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID || zCConsole.Console(process).IsVisible() == 1)
                return;

            if (trading && (key == (int)VirtualKeys.Escape || key == (int)VirtualKeys.Tab))
            {
                trading = false;
                trader = null;
                CloseTradeMenu();
            }
        }

        public void KeyPressed(int key)
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID || zCConsole.Console(process).IsVisible() == 1)
                return;

            if (!trading)
            {
                if (key == startTradeKey)
                {//zum testen
                    StartTrading(Player.Hero.ID);
                    return;
                }

                if (key == startTradeKey && Player.Hero.WeaponMode == 0 && Player.Hero.FocusVob != null && Player.Hero.FocusVob.VobType == VobType.Player)
                {
                    Player target = (Player)sWorld.VobDict[Player.Hero.FocusVob.ID];
                    if (target.WeaponMode == 0 && (Player.Hero.FocusVob.Position - Player.Hero.Position).Length < 100)
                    {
                        RequestTrade(target.ID);
                    }
                }
            }
            else
            {
                int x = cursor[0];
                int y = cursor[1];
                switch ((VirtualKeys)key)
                {
                    case VirtualKeys.Left:
                        if (x == 0) //move into buyInv
                        {
                            if (y >= buyInv.Count)
                            {
                                y = buyInv.Count - 1;
                            }
                        }
                        if (x > -4) //already leftest
                        {
                            x--; //move left
                        }
                        break;
                    case VirtualKeys.Right:
                        if (x < inventory[0].Count - 1)
                        {
                            x++; //move right
                        }
                        break;
                    case VirtualKeys.Up:
                        if (y > 0)
                        {
                            y--; //move up
                        }
                        else if (y == 0 && x > 0)
                        {
                            x--;
                        }
                        break;
                    case VirtualKeys.Down:
                        if (x < 0) //cursor is in smaller buy/sell inventories
                        {
                            if (y < buyInv.Count - 1)
                            {
                                y++;
                            }
                        }
                        else
                        {
                            if (y < inventory.Count - 1)
                            {
                                y++;
                            }
                        }
                        break;
                    case VirtualKeys.Control:
                        if (cursor[0] >= 0)
                        {
                            Item item = GetSlot(cursor).item;
                            if (item != null && sellItems.Count < 8)
                            {
                                OfferItem(Player.Hero.ItemList.IndexOf(item));
                                invItems.Remove(item);
                                sellItems.Add(item);
                                UpdateMenuItems();
                            }
                        }
                        else if (cursor[0] >= -2)
                        {
                            Item item = GetSlot(cursor).item;
                            if (item != null)
                            {
                                OfferItem(Player.Hero.ItemList.IndexOf(item));
                                sellItems.Remove(item);
                                invItems.Add(item);
                                UpdateMenuItems();
                            }
                        }
                        break;
                }
                UpdateCursor(x, y);
            }
        }

        public void wheelChanged(int steps)
        {
        }

        public void StartTrading(int traderID)
        {
            trader = (Player)sWorld.VobDict[traderID];
            trading = true;
            OpenTradeMenu();
        }

        public void TakeOffer(int traderID, int itemNum)
        {
            if (traderID == trader.ID)
            {

            }
            //else wtf?
        }

        private void CreateTradeMenu()
        {
            thisView = zCView.Create(process, 0, 0, 0x2000, 0x2000);

            int[] screensize = InputHooked.GetScreenSize(process);
            int slotnum = (screensize[1] - 300) / 70;

            //Inventory Background
            int[] pos = InputHooked.PixelToVirtual(process, new int[] { screensize[0] - 40 - 5 * 70, 120 });
            int[] size = InputHooked.PixelToVirtual(process, new int[] { 5 * 70, slotnum * 70 });
            invBack = zCView.Create(process, pos[0], pos[1], pos[0] + size[0], pos[1] + size[1]);
            using (zString tex = zString.Create(process, "INV_BACK.TGA"))
            {
                invBack.InsertBack(tex);
            }
            thisView.InsertItem(invBack, 0);

            //Selling Background
            pos[0] -= InputHooked.PixelToVirtualX(process, 40 + 2 * 70);
            size = InputHooked.PixelToVirtual(process, new int[] { 2 * 70, 4 * 70 });
            sellBack = zCView.Create(process, pos[0], pos[1], pos[0] + size[0], pos[1] + size[1]);
            using (zString tex = zString.Create(process, "INV_BACK_SELL.TGA"))
            {
                sellBack.InsertBack(tex);
            }
            thisView.InsertItem(sellBack, 0);

            //Buying Background
            pos[0] -= InputHooked.PixelToVirtualX(process, 40 + 2 * 70);
            buyBack = zCView.Create(process, pos[0], pos[1], pos[0] + size[0], pos[1] + size[1]);
            using (zString tex = zString.Create(process, "INV_BACK_BUY.TGA"))
            {
                buyBack.InsertBack(tex);
            }
            thisView.InsertItem(buyBack, 0);

            //Inventory slots
            inventory = new List<List<InvSlot>>();
            for (int i = 0; i < slotnum; i++)
            {
                List<InvSlot> row = new List<InvSlot>();
                for (int j = 0; j < 5; j++)
                {
                    pos = InputHooked.PixelToVirtual(process, new int[] { screensize[0] - 390 + j * 70, 120 + i * 70 });
                    row.Add(new InvSlot(thisView, pos[0], pos[1]));
                }
                inventory.Add(row);
            }

            //Trade slots
            sellInv = new List<List<InvSlot>>();
            buyInv = new List<List<InvSlot>>();
            for (int i = 0; i < 4; i++)
            {
                List<InvSlot> buyRow = new List<InvSlot>();
                List<InvSlot> sellRow = new List<InvSlot>();
                for (int j = 0; j < 2; j++)
                {
                    pos = InputHooked.PixelToVirtual(process, new int[] { screensize[0] - 750 + j * 70, 120 + i * 70 });
                    buyRow.Add(new InvSlot(thisView, pos[0], pos[1]));

                    pos = InputHooked.PixelToVirtual(process, new int[] { screensize[0] - 570 + j * 70, 120 + i * 70 });
                    sellRow.Add(new InvSlot(thisView, pos[0], pos[1]));
                }
                buyInv.Add(buyRow);
                sellInv.Add(sellRow);
            }
        }

        private void OpenTradeMenu()
        {
            cursor[0] = 0;
            cursor[1] = 0;
            inventory[0][0].Mark();

            invItems = new List<Item>(Player.Hero.ItemList);
            buyItems = new List<Item>();
            sellItems = new List<Item>();
            UpdateMenuItems();
            zCView.GetStartscreen(process).InsertItem(thisView, 0);
            InputHooked.deactivateFullControl(process,this);
        }

        private void UpdateMenuItems()
        {
            
            for (int num = 0, y = 0; y < inventory.Count; y++)
                for (int x = 0; x < inventory[y].Count; num++, x++)
                    if (num < invItems.Count)
                    {
                        inventory[y][x].SetItem(invItems[num]);
                    }
                    else
                    {
                        inventory[y][x].RemoveItem();
                    }

            for (int num = 0, y = 0; y < sellInv.Count; y++)
                for (int x = 0; x < sellInv[y].Count; num++, x++)
                    if (num < sellItems.Count)
                    {
                        sellInv[y][x].SetItem(sellItems[num]);
                    }
                    else
                    {
                        sellInv[y][x].RemoveItem();
                    }

            for (int num = 0, y = 0; y < buyInv.Count; y++)
                for (int x = 0; x < buyInv[y].Count; num++, x++)
                    if (num < buyItems.Count)
                    {
                        buyInv[y][x].SetItem(buyItems[num]);
                    }
                    else
                    {
                        buyInv[y][x].RemoveItem();
                    }
        }

        private void UpdateCursor(int x, int y)
        {
            if (cursor[0] != x || cursor[1] != y)
            {
                GetSlot(cursor).Demark();
                GetSlot(x, y).Mark();
                cursor[0] = x;
                cursor[1] = y;
            }
        }

        private void CloseTradeMenu()
        {
            GetSlot(cursor).Demark();

            foreach (List<InvSlot> row in inventory)
                foreach (InvSlot slot in row)
                    slot.RemoveItem();

            foreach (List<InvSlot> row in sellInv)
                foreach (InvSlot slot in row)
                    slot.RemoveItem();

            foreach (List<InvSlot> row in buyInv)
                foreach (InvSlot slot in row)
                    slot.RemoveItem();

            zCView.GetStartscreen(process).RemoveItem(thisView);
            InputHooked.activateFullControl(process);
        }

        private InvSlot GetSlot(int[] cur)
        {
            return GetSlot(cur[0], cur[1]);
        }

        private InvSlot GetSlot(int x, int y)
        {
            if (x <= -3) //Buy inventory
            {
                return buyInv[y][4 + x];
            }
            else if (x <= -1) // sell inventory
            {
                return sellInv[y][2 + x];
            }
            else //player inventory
            {
                return inventory[y][x];
            }
        }
    }
}
