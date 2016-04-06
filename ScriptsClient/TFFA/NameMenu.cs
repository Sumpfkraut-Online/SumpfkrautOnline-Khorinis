using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Scripts.Sumpfkraut.Menus;
using GUC.Client.GUI;
using WinApi.User.Enumeration;

namespace GUC.Client.Scripts.TFFA
{
    class NameMenu : GUCMenu
    {
        public static readonly NameMenu Instance = new NameMenu();

        GUCVisual back;
        GUCVisualText title;
        GUCTextBox textBox;

        public NameMenu()
        {
            int[] res = GUCView.GetScreenSize();
            back = new GUCVisual(res[0] / 2 - 320, res[1] / 2 - 240 - 20 - 100, 640, 100);
            back.SetBackTexture("Menu_Choice_Back.tga");
            back.Font = GUCView.Fonts.Menu;

            title = back.CreateText("Name:", 100, 80);

            textBox = new GUCTextBox(res[0] / 2 - 100, res[1]/2 - 240 - 20 - 80, 400, true);
            back.AddChild(textBox);
        }

        public override void Open()
        {
            base.Open();
            back.Show();
            textBox.Show();
        }

        public override void Close()
        {
            base.Close();
            back.Hide();
            textBox.Hide();
        }

        public override void KeyDown(VirtualKeys key, long now)
        {
            textBox.KeyPressed(key);
        }

        public override void KeyUp(VirtualKeys key, long now)
        {
        }

        public override void Update(long now)
        {
            textBox.Update(now);
        }
    }
}
