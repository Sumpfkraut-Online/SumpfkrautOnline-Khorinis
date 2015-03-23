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

using RakNet;


namespace GUC.GUI
{
    class InvSlot
    {
        private Process process;
        private oCItem item;
        private zCView view;
        private zCWorld rndrWorld;

        public InvSlot(zCView parent, int x, int y, int width, int height)
        {
            process = Process.ThisProcess();
            view = zCView.Create(Process.ThisProcess(), x, y, x + width, y + height);
            using (zString texture = zString.Create(Process.ThisProcess(), "Inv_Slot.tga"))
            {
                view.InsertBack(texture);
            }
            view.FillZ = true;
            view.Blit();
            parent.InsertItem(view, 0);
        }

        public void SetItem(Item item)
        {
            this.item = new oCItem(process, item.Address);
            rndrWorld = zCWorld.Create(process);
            rndrWorld.IsInventoryWorld = true;
            this.item.RenderItem(rndrWorld, view, 0.0f);
        }
    }

    class TradeGUI : InputReceiver
    {
        Process process;

        public int startTradeKey;

        Player trader;
        Boolean trading;


        Action<int> RequestTrade;

        InvSlot inv;
        List<List<InvSlot>> inventory;
        List<InvSlot> myTradeInv;
        List<InvSlot> otherTradeInv;

        zCView thisView;

        public TradeGUI(Action<int> Request)
        {
            process = Process.ThisProcess();
            startTradeKey = (int)VirtualKeys.T;
            trading = false;
            trader = null;

            RequestTrade = Request;
            InputHooked.receivers.Add(this);

            CreateTradeMenu();
        }

        public void KeyReleased(int key)
        {
        }

        public void KeyPressed(int key)
        {
            if (WinApi.User.Window.GetWindowThreadProcessId(WinApi.User.Window.GetForegroundWindow()) != process.ProcessID || zCConsole.Console(process).IsVisible() == 1)
                return;

            if (!trading)
            {
                if (key == startTradeKey && Player.Hero.WeaponMode == 0 && Player.Hero.FocusVob != null && Player.Hero.FocusVob.VobType == VobType.Player)
                {
                    Player target = (Player)sWorld.VobDict[Player.Hero.FocusVob.ID];
                    if (target.WeaponMode == 0 && (Player.Hero.FocusVob.Position - Player.Hero.Position).Length < 100)
                    {
                        RequestTrade(target.ID);
                    }
                }
                else
                {
                    return;
                }
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
            //InputHooked.deaktivateFullControl(process);
        }

        private void CreateTradeMenu()
        {
            thisView = zCView.Create(process, 0, 0, 0x2000, 0x2000);

            int[] screensize = InputHooked.GetScreenSize(process);
            int[] slotsize = InputHooked.PixelToVirtual(process, new int[] { 69, 69 });
            int slotnum = (screensize[1] - 300) / 70;

            List<InvSlot> row;
            inventory = new List<List<InvSlot>>();
            for (int i = 0; i < slotnum; i++)
            {
                row = new List<InvSlot>();
                for (int j = 0; j < 5; j++)
                {
                    int[] pos = InputHooked.PixelToVirtual(process, new int[] { screensize[0] - 390 + j * 70, screensize[1] - 200 - (slotnum - i) * 70 });
                    row.Add(new InvSlot(thisView, pos[0], pos[1], slotsize[0], slotsize[1]));
                }
                inventory.Add(row);
            }
        }

        private void OpenTradeMenu()
        {
          /*  item = new oCItem(process, Player.Hero.ItemList[0].Address);

            zV = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            zV.CreateText(10, 10, item.Name);
            zV.FillZ = true;
            zV.Blit();
            zCView.GetScreen(Process.ThisProcess()).InsertItem(zV, 0);

            rndrWorld = zCWorld.Create(process);
            rndrWorld.IsInventoryWorld = true;

            Program.OnRender += Test;*/
            
            //Test();
            /*int x = 0; int y = 0;
            foreach(Item item in Player.Hero.ItemList)
            {
                inventory[y][x].SetItem(item);
                x++;
                if (x > 5)
                {
                    x = 0;
                    y++;
                    if (y > inventory.Count) break;
                }
            }
            zCView.GetStartscreen(process).InsertItem(thisView, 0);*/
        }
        /*
        zCView zV;
        oCItem item;
        zCWorld rndrWorld;
        public virtual void Test(Process process, long now)
        {
            item.RenderItem(rndrWorld, zV, 0.0f);
        }*/

    }
}
