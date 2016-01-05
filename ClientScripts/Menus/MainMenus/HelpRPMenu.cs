using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Menus.MainMenus
{
    class HelpRPMenu : GUCMainMenu
    {
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("RP-Guide", 100);
            //massive wall of text
            AddButton("Zurück", "Zurück zum Hilfemenü.", 350, GUCMenus.Help.Open);
            OnEscape = GUCMenus.Help.Open;
        }
    }
}
