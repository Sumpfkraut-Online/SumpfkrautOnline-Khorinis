using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Menus.MainMenus
{
    class PlayerlistMenu : GUCMainMenu
    {
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Spielerliste", 100);
            //list of player names
            AddButton("Zurück", "Zurück zum Hauptmenü.", 350, GUCMenus.Main.Open);
            OnEscape = GUCMenus.Main.Open;
        }
    }
}
