using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Client.GUI;
using GUC.Client.WorldObjects;

namespace GUC.Client.Menus
{
    class DropItemMenu : GUCMenu
    {
        GUCVisual back;
        GUCVisual border;
        GUCTextBox tb;

        public DropItemMenu()
        {
            const int backWidth = 120;
            const int backHeight = 64;
            back = new GUCVisual((GUCView.GetScreenSize()[0]-backWidth)/2, (GUCView.GetScreenSize()[1] - backHeight)/2, backWidth, backHeight);
            back.SetBackTexture("Inv_Back.tga");

            const int borderWidth = 80;
            const int borderHeight = 24;
            border = (GUCVisual)back.AddChild(new GUCVisual((GUCView.GetScreenSize()[0] - borderWidth) / 2, (GUCView.GetScreenSize()[1] - borderHeight) / 2, borderWidth, borderHeight));
            border.SetBackTexture("Inv_Titel.tga");

            const int tbWidth = borderWidth - 20;
            tb = (GUCTextBox)border.AddChild(new GUCTextBox((GUCView.GetScreenSize()[0] - tbWidth) / 2, (GUCView.GetScreenSize()[1] - GUCView.FontsizeDefault) / 2, tbWidth,true));
            tb.OnlyNumbers = true;
        }

        Action<ItemInstance,int> callback;
        ItemInstance instance;
        int max;

        public void Open(Action<ItemInstance,int> callback, ItemInstance instance, int max)
        {
            this.callback = callback;
            this.instance = instance;
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
            callback = null;
            instance = null;
            base.Close();
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == VirtualKeys.Escape || key == VirtualKeys.Tab)
            {
                Close();
            }
            else if (key == VirtualKeys.Return || key == VirtualKeys.Control || key == VirtualKeys.Menu)
            {
                if (callback != null && instance != null)
                {
                    int amount = Convert.ToInt32(tb.Input);
                    callback(instance, amount > max ? max : amount);
                }
                Close();
            }
            else
            {
                tb.KeyPressed(key);
            }
        }

        public override void Update(long now)
        {
            tb.Update(now);
        }
    }
}
