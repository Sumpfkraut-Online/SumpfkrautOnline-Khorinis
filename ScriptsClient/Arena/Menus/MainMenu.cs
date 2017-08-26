using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;

namespace GUC.Scripts.Arena.Menus
{
    class MainMenu : GUCMainMenu
    {
        public readonly static MainMenu Menu = new MainMenu();

        protected override void OnCreate()
        {
            preferredCursorItem = 1;
            AddButton("Beitreten", "Dem Spiel beitreten.", 150, null);
            AddButton("Zuschauen", "Dem Spiel zuschauen.", 190, null);
            AddButton("Charakter editieren", "Deinen Spielcharakter editieren.", 250, CharCreationMenu.Menu.Open);
            AddButton("Spiel verlassen", "Das Spiel schließen.", 350, ExitMenu.Menu.Open);
        }
        
    }
}
