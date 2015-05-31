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
        class LoginMenu : AbstractMenu
        {
            //account name
            //account password
            //login
            //back

            GUCMainMenuTextBox accName;
            GUCMainMenuTextBox accPW;

            protected override void CreateMenu()
            {
                thisMenu = new GUCMainMenu(0);
                thisMenu.AddText("Anmeldung", 100);
                accName = thisMenu.AddMenuTextBox("Accountname", "Name deines Accounts eingeben.", Login, 200, 200);
                accPW = thisMenu.AddMenuTextBox("Passwort", "Passwort deines Accounts eingeben.", Login, 250, 200);
                thisMenu.AddMenuButton("Einloggen", "In den Account einloggen.", Login, 300);
                thisMenu.AddMenuButton("Zurück", "Zurück zum Hauptmenü.", Main.Open, 400);
                thisMenu.OnEscape = Main.Open;

                accName.TextBox.allowWhiteSpaces = false;
                accPW.TextBox.allowWhiteSpaces = false;
            }

            void Login(object sender, EventArgs e)
            {

                string logName = accName.TextBox.input;
                string logPW = accPW.TextBox.input;
                if (logName.Length == 0)
                {
                    thisMenu.SetCursor(0);
                    return;
                }
                else if (logPW.Length == 0)
                {
                    thisMenu.SetCursor(1);
                    return;
                }
                else
                {
                    StartupState.clientOptions.name = logName;
                    StartupState.clientOptions.password = logPW;
                    StartupState.clientOptions.Save(StartupState.getConfigPath() + "gmp.xml");
                    Sumpfkraut.Login.LoginMessage.GetMsg().Login();
                }
            }
        }
    }
}
