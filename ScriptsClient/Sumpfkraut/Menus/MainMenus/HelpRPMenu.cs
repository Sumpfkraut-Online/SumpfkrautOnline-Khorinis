using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus
{
    class HelpRPMenu : GUCMainMenu
    {
        public readonly static HelpRPMenu Menu = new HelpRPMenu();

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("RP-Guide", 100);
            //massive wall of text
            AddButton("Zurück", "Zurück zum Hilfemenü.", 350, HelpMenu.Menu.Open);
            OnEscape = HelpMenu.Menu.Open;
        }
    }
}
