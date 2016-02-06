using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Client.GUI;

namespace GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus
{
    class MainMenu : GUCMainMenu
    {
        public readonly static MainMenu Menu = new MainMenu();

        MainMenuButton bLogin, bRegister, bContinue;
        bool ingame = false;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Hauptmenü", 70);

            const int offset = 200;
            const int dist = 38;
            bLogin = AddButton("Anmelden", "In einen bestehenden Account einloggen.", offset + dist * 0, LoginMenu.Menu.Open);
            bRegister = AddButton("Registrieren", "Einen neuen Account erstellen.", offset + dist * 1, null);
            bContinue = AddButton("Weiterspielen", "Aktuelles Spiel fortführen.", offset + dist * 2, Close); //lol
            AddButton("Spielerliste", "Zeigt die angemeldeten Spieler.", offset + dist * 3, PlayerlistMenu.Menu.Open);
            AddButton("Hilfe", "Kurzhilfe & Einführung in das Rollenspiel.", offset + dist * 4, HelpMenu.Menu.Open);
            AddButton("Beenden", "Die Welt von SumpfkrautOnline verlassen.", offset + dist * 5, ExitMenu.Menu.Open);
            bContinue.Enabled = false;
            OnEscape = Open;
        }

        public override void Open()
        {
            if (GUC.Scripts.Init.Ingame)
            {
                if (!ingame)
                {
                    OnEscape = null; //closes anyway

                    bLogin.Text = "Abmelden";
                    bLogin.HelpText = "Aus dem aktuellen Account ausloggen.";
                    bLogin.OnActivate = null;

                    bRegister.Text = "Charakter wechseln";
                    bRegister.HelpText = "Den aktuellen Charakter wechseln.";
                    bRegister.OnActivate = null;

                    bContinue.Enabled = true;
                    ingame = true;
                }
            }
            else
            {
                if (ingame)
                {
                    OnEscape = Open; //reopen

                    bLogin.Text = "Anmelden"; 
                    bLogin.HelpText = "In einen bestehenden Account einloggen.";
                    bLogin.OnActivate = null;
                    
                    bRegister.Text = "Registrieren"; 
                    bRegister.HelpText = "Einen neuen Account erstellen.";
                    bRegister.OnActivate = null;

                    bContinue.Enabled = false;
                    ingame = false;
                }
            }
            base.Open();
        }
    }
}
