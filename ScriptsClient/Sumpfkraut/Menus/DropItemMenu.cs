using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.Networking;

namespace GUC.Scripts.Sumpfkraut.Menus
{
    class DropItemMenu : GUCMenu
    {
        public static readonly DropItemMenu Menu = new DropItemMenu();

        GUCVisual back;
        GUCVisual border;
        GUCTextBox tb;

        int maxAmount;
        ItemInst item;
        NPCInst player;

        public DropItemMenu()
        {
            const int backWidth = 120;
            const int backHeight = 64;
            back = new GUCVisual((GUCView.GetScreenSize().X- backWidth)/2, (GUCView.GetScreenSize().Y - backHeight)/2, backWidth, backHeight);
            back.SetBackTexture("Inv_Back.tga");

            const int borderWidth = 80;
            const int borderHeight = 24;
            border = (GUCVisual)back.AddChild(new GUCVisual((GUCView.GetScreenSize().X - borderWidth) / 2, (GUCView.GetScreenSize().Y - borderHeight) / 2, borderWidth, borderHeight));
            border.SetBackTexture("Inv_Titel.tga");

            const int tbWidth = borderWidth - 20;
            tb = (GUCTextBox)border.AddChild(new GUCTextBox((GUCView.GetScreenSize().X - tbWidth) / 2, (GUCView.GetScreenSize().Y - GUCView.FontsizeDefault) / 2, tbWidth,true));
            tb.OnlyNumbers = true;

            player = ScriptClient.Client.Character;
        }

        public void Open(ItemInst item)
        {
            maxAmount = item.Amount;
            tb.Input = maxAmount.ToString();
            Open();
            this.item = item;
        }

        public override void Open()
        {
            back.Show();
            border.Show();
            tb.Show();
            tb.Enabled = true;
            base.Open();
        }

        public override void Close()
        {
            back.Hide();
            border.Hide();
            tb.Hide();
            tb.Enabled = false;
            base.Close();
        }

        protected override void KeyPress(VirtualKeys key, bool hold)
        {
            if (hold) return;

            if (key == VirtualKeys.Escape || key == VirtualKeys.Tab)
            {
                Close();
            }
            else if (key == VirtualKeys.Return || key == VirtualKeys.Control || key == VirtualKeys.Menu)
            {
                int amount = Convert.ToInt32(tb.Input.ToString().Length > 0 ? tb.Input : "0");
                if (amount != 0)
                {
                    amount = amount > maxAmount ? maxAmount : amount;
                    NPCInst.Requests.DropItem(player, item, amount);
                }
                Close();
            }
            else
            {
                tb.KeyPressed(key);
            }
        }

        protected override void Update(long now)
        {
            tb.Update(now);
        }
    }
}


/*
 * class DropItemMenu : GUCMenu
    {
        GUCVisual back;
        GUCVisual border;
        GUCTextBox tb;

        public DropItemMenu()
        {
            const int backWidth = 120;
            const int backHeight = 64;
            back = new GUCVisual((GUCView.GetScreenSize().Width-backWidth)/2, (GUCView.GetScreenSize().Height - backHeight)/2, backWidth, backHeight);
            back.SetBackTexture("Inv_Back.tga");

            const int borderWidth = 80;
            const int borderHeight = 24;
            border = (GUCVisual)back.AddChild(new GUCVisual((GUCView.GetScreenSize().Width - borderWidth) / 2, (GUCView.GetScreenSize().Height - borderHeight) / 2, borderWidth, borderHeight));
            border.SetBackTexture("Inv_Titel.tga");

            const int tbWidth = borderWidth - 20;
            tb = (GUCTextBox)border.AddChild(new GUCTextBox((GUCView.GetScreenSize().Width - tbWidth) / 2, (GUCView.GetScreenSize().Height - GUCView.FontsizeDefault) / 2, tbWidth,true));
            tb.OnlyNumbers = true;
        }

        Action<int> callback1;
        Action<object,int> callback2;
        object obj;
        int max;

        public void Open(Action<int> callback, int max)
        {
            this.callback1 = callback;
            this.callback2 = null;
            this.obj = null;
            this.max = max;

            tb.Input = max.ToString();
            Open();
        }

        public void Open(Action<object,int> callback, object obj, int max)
        {
            this.callback1 = null;
            this.callback2 = callback;
            this.obj = obj;
            this.max = max;

            tb.Input = max.ToString();
            Open();
        }

        public override void Open()
        {
            back.Show();
            tb.Enabled = true;
            base.Open();
        }

        public override void Close()
        {
            back.Hide();
            tb.Enabled = false;
            callback1 = null;
            callback2 = null;
            obj = null;
            base.Close();
        }

        public override void KeyDown(VirtualKeys key, long now)
        {
            if (key == VirtualKeys.Escape || key == VirtualKeys.Tab)
            {
                Close();
            }
            else if (key == VirtualKeys.Return || key == VirtualKeys.Control || key == VirtualKeys.Menu)
            {
                int amount = Convert.ToInt32(tb.Input);
                if (callback1 != null)
                {
                    callback1(amount > max ? max : amount);
                }
                else if (callback2 != null)
                {
                    callback2(obj, amount > max ? max : amount);
                }
                Close();
            }
            else
            {
                tb.KeyPressed(key);
            }
        }

        public override void KeyUp(VirtualKeys key, long now)
        {

        }

        public override void Update(long now)
        {
            tb.Update(now);
        }
    }
 */