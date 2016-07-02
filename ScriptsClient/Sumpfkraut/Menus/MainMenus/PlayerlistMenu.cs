using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.GUI;

namespace GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus
{
    class PlayerlistMenu : GUCMainMenu
    {
        GUCVisual PlayerList;
        public readonly static PlayerlistMenu Menu = new PlayerlistMenu();

        //TODO: translation
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Spielerliste", 25);
            
            int[] screenSize = GUCView.GetScreenSize();
            pos = new int[] { (screenSize[0] - 640) / 2, (screenSize[1] - 480) / 2 };
            
            PlayerList = new GUCVisual(pos[0] + 130, pos[1] + 80, 640, 480);
                        //PlayerList.SetBackTexture("Inv_Back_Sell.tga");
            PlayerList.Show();
            
            int distance = 17;

            for (int i = 0; i < 20; i++)
            {
                PlayerList.CreateText("SpielerName", 0, i * distance);
            }

            for (int i = 0; i < 20; i++)
            {
                PlayerList.CreateText("SpielerName", 250, i * distance);
            }
            
            AddButton("Zurück", "Zurück zum Hauptmenü.", 420, GUCMenus.Main.Open);
            OnEscape = MainMenu.Menu.Open;
        }

        public override void KeyPressed(WinApi.User.Enumeration.VirtualKeys key)
        {
            base.KeyPressed(key);
        }
    }
}
