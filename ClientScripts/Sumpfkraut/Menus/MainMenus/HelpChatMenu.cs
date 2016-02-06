using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus
{
    class HelpChatMenu : GUCMainMenu
    {
        public readonly static HelpChatMenu Menu = new HelpChatMenu();

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Chathilfe", 100);

            AddButton("Zurück", "Zurück zum Hilfemenü.", 400, HelpMenu.Menu.Open);
            OnEscape = HelpMenu.Menu.Open;
        }
    }
}
