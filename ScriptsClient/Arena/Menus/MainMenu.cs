using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;

namespace GUC.Scripts.Arena.Menus
{
    class MainMenu : GUCMainMenu
    {
        public readonly static MainMenu Menu = new MainMenu();

        MainMenuButton teamButton;
        protected override void OnCreate()
        {
            AddButton("Beitreten", "Dem Spiel beitreten.", 140, () => { ArenaClient.SendJoinGameMessage(); Close(); });
            AddButton("Zuschauen", "Dem Spiel zuschauen.", 180, () => { ArenaClient.SendSpectateMessage(); Close(); });
            teamButton = AddButton("Team beitreten", "Team beitreten.", 220, null);
            AddButton("Charakter editieren", "Deinen Spielcharakter editieren.", 260, CharCreationMenu.Menu.Open);
            AddButton("Spiel verlassen", "Das Spiel schließen.", 340, ExitMenu.Menu.Open);

            teamButton.Enabled = false;
        }

        public override void Open()
        {
            base.Open();
        }
    }
}
