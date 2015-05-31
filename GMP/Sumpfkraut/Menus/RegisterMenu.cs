using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;
using GUC.States;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        class RegisterMenu : AbstractMenu
        {
            //account name
            //account password
            //register
            //back

            GUCMainMenuTextBox accName;
            GUCMainMenuTextBox accPW;

            protected override void CreateMenu()
            {
                thisMenu = new GUCMainMenu(0);
                thisMenu.AddText("Registrierung", 100);
                accName = thisMenu.AddMenuTextBox("Accountname", "Name deines neuen Accounts eingeben.", Register, 200, 200);
                accPW = thisMenu.AddMenuTextBox("Passwort", "Passwort deines neuen Accounts eingeben.", Register, 250, 200);
                thisMenu.AddMenuButton("Erstellen", "Den neuen Account erstellen.", Register, 300);
                thisMenu.AddMenuButton("Zurück", "Zurück zum Hauptmenü.", Main.Open, 400);
                thisMenu.OnEscape += Main.Open;

                accName.TextBox.allowWhiteSpaces = false;
                accPW.TextBox.allowWhiteSpaces = false;
            }

            void Register(object sender, EventArgs e)
            {
                string regName = accName.TextBox.input;
                string regPW = accPW.TextBox.input;
                if (regName.Length == 0)
                {
                    thisMenu.SetCursor(0);
                    return;
                }
                /*else if (regName.Length < 5)
                {
                    thisMenu.SetCursor(0);
                    thisMenu.SetHelpText("Bitte gebe mindestens 5 Zeichen als Accountnamen ein.");
                    return;
                }*/
                else if (regPW.Length == 0)
                {
                    thisMenu.SetCursor(1);
                    return;
                }
                /*else if (regPW.Length < 5)
                {
                    thisMenu.SetCursor(1);
                    thisMenu.SetHelpText("Bitte gebe mindestens 5 Zeichen als Passwort ein.");
                    return;
                }*/
                else
                {
                    StartupState.clientOptions.name = regName;
                    StartupState.clientOptions.password = regPW;
                    StartupState.clientOptions.Save(StartupState.getConfigPath() + "gmp.xml");
                    Sumpfkraut.Login.LoginMessage.GetMsg().Register();
                }
            }
        }
    }
}
