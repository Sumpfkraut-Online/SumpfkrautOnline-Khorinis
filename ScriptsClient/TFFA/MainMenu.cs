using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Client.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Scripts.TFFA;

namespace GUC.Client.Scripts.TFFA
{
    class MainMenu : GUCMainMenu
    {
        public readonly static MainMenu Menu = new MainMenu();

        MainMenuButton bClass, bContinue;
        bool ingame = false;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Hauptmenü", 70);

            const int offset = 200;
            const int dist = 38;
            AddButton("Team Wählen", "", offset + dist * 0, TeamMenu.Menu.Open);
            bClass = AddButton("Klasse Wählen", "", offset + dist * 1, ClassMenu.Menu.Open);
            AddButton("Name ändern", "", offset + dist * 2, NameMenu.Menu.Open);
            bContinue = AddButton("Weiterspielen", "Aktuelles Spiel fortführen.", offset + dist * 3, Close);
            AddButton("Beenden", "Die Welt von SumpfkrautOnline verlassen.", offset + dist * 5, ExitMenu.Menu.Open);
            bContinue.Enabled = false;
            OnEscape = Open;
        }

        public override void Open()
        {
            Scoreboard.Menu.Close();

            base.Open();
            bClass.Enabled = TFFAClient.Client != null && TFFAClient.Client.Team != Team.Spec;

            if (GUC.Scripts.GUCScripts.Ingame)
            {
                if (!ingame)
                {
                    OnEscape = null; //closes anyway

                    bContinue.Enabled = true;
                    ingame = true;
                }
            }
            else
            {
                if (ingame)
                {
                    OnEscape = Open; //reopen

                    bContinue.Enabled = false;
                    ingame = false;
                }
            }
        }
    }
}
