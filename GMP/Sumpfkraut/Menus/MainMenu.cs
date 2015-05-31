using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;
using GUC.Types;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        class MainMenu : AbstractMenu
        {
            //Anmelden / Abmelden
            //Registrieren / Charakter wechseln
            //Spielerliste
            //RP-Guide
            //Beenden

            GUCMainMenuButton bLogin;
            GUCMainMenuButton bRegister;
            GUCMainMenuButton bContinue;
            bool ingame;
            ColorRGBA grey;

            protected override void CreateMenu()
            {
                const int offset = 150;
                const int dist = 35;
                grey = new ColorRGBA(150, 150, 150);
                thisMenu = new GUCMainMenu(0);
                thisMenu.AddText("Hauptmenü", 70);
                bLogin = thisMenu.AddMenuButton("Anmelden", "In einen bestehenden Account einloggen.", Login.Open, offset + dist*0);
                bRegister = thisMenu.AddMenuButton("Registrieren", "Einen neuen Account erstellen.", Register.Open, offset + dist * 1);
                bContinue = thisMenu.AddMenuButton("Weiterspielen", "Aktuelles Spiel fortführen.", Close, offset + dist * 2); //lol
                thisMenu.AddMenuButton("Spielerliste", "Zeigt die angemeldeten Spieler.", PlayerList.Open, offset + dist * 3);
                thisMenu.AddMenuButton("RP-Guide", "Eine kurze Einführung ins Rollenspiel.", RPGuide.Open, offset + dist * 4);
                thisMenu.AddMenuButton("Intro spielen", "Introsequenz noch einmal abspielen.", null, offset + dist * 5);
                thisMenu.AddMenuButton("Credits", "Credits", null, offset + dist * 6);
                thisMenu.AddMenuButton("Beenden", "Die Welt von SumpfkrautOnline verlassen.", Exit.Open, offset + dist * 7);
                bContinue.SetColor(grey);
                bContinue.ignore = true;
                ingame = false;
            }

            public override void Open(object sender, EventArgs e)
            {
                base.Open(null, null);
                ChangeButtons();
            }
            
            private void ChangeButtons()
            {
                if (Program._state is States.StartupState)
                {
                    if (ingame)
                    {
                        thisMenu.OnEscape = null;
                        bLogin.SetText("Anmelden"); bLogin.SetHelpText("In einen bestehenden Account einloggen.");
                        bRegister.SetText("Registrieren"); bRegister.SetHelpText("Einen neuen Account erstellen.");
                        bLogin.Center();
                        bRegister.Center();
                        bLogin.func = Login.Open;
                        bRegister.func = Register.Open;
                        bContinue.SetColor(grey);
                        bContinue.ignore = true;
                        ingame = false;
                    }
                }
                else
                {
                    if (!ingame)
                    {
                        thisMenu.OnEscape = Close;
                        bLogin.SetText("Abmelden"); bLogin.SetHelpText("Aus dem aktuellen Account ausloggen.");
                        bRegister.SetText("Charakter wechseln"); bRegister.SetHelpText("Den aktuellen Charakter wechseln.");
                        bLogin.Center();
                        bRegister.Center();
                        bLogin.func = null;
                        bRegister.func = null;
                        bContinue.SetColor(ColorRGBA.White);
                        bContinue.ignore = false;
                        ingame = true;
                    }
                }
            }
        }
    }
}
