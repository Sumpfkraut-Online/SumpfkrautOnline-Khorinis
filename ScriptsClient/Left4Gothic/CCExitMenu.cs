using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;

namespace GUC.Scripts.Left4Gothic
{
    class CCExitMenu : GUCMainMenu
    {
        public readonly static CCExitMenu Menu = new CCExitMenu();

        protected override void OnCreate()
        {
            preferredCursorItem = 1;
            Back.CreateTextCenterX("SumpfkrautOnline verlassen?", 100);
            AddButton("Ja", "Ja, ich möchte SumpfkrautOnline verlassen.", 200, Program.Exit);
            AddButton("Nein", "Nein, ich möchte weiterspielen.", 250, CharCreationMenu.Menu.Open);
            OnEscape = CharCreationMenu.Menu.Open;
        }
    }
}
