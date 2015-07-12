using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Menus.MainMenus
{
    class HelpChatMenu : GUCMainMenu
    {
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Chathilfe", 100);

            AddButton("Zurück", "Zurück zum Hilfemenü.", 400, GUCMenus.Help.Open);
            OnEscape = GUCMenus.Help.Open;
        }
    }
}
