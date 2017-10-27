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
            teamButton = AddButton("Team-Modus", "Einem Team im TeamObjective-Modus beitreten oder zuschauen.", 140, TOTeamsMenu.Menu.Open);
            AddButton("Freier Modus", "Dem Spiel beitreten.", 180, () => { ArenaClient.SendJoinGameMessage(); Close(); });
            AddButton("Zuschauen", "Dem Spiel zuschauen.", 220, () => { ArenaClient.SendSpectateMessage(); Close(); });
            AddButton("Charakter editieren", "Deinen Spielcharakter editieren.", 260, CharCreationMenu.Menu.Open);
            AddButton("Spiel verlassen", "Das Spiel schließen.", 340, ExitMenu.Menu.Open);
            OnEscape = () => { if (!GUCScripts.Ingame) Open(); };
        }

        public override void Open()
        {
            base.Open();
            UpdateTeamButton();
            TeamMode.OnPhaseChange += UpdateTeamButton;
        }

        public override void Close()
        {
            base.Close();
            TeamMode.OnPhaseChange -= UpdateTeamButton;
        }

        void UpdateTeamButton()
        {
            teamButton.Enabled = TeamMode.IsRunning && TeamMode.Phase != TOPhases.Finish && TeamMode.Phase != TOPhases.None;
            if (!teamButton.Enabled && CurrentItem == teamButton)
                MoveCursor();
        }
    }
}
