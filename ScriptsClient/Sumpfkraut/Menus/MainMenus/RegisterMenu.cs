using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Scripts.Menus.MainMenus
{
    /*class RegisterMenu : GUCMainMenu
    {
        MainMenuTextBox name;
        MainMenuTextBox pw;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Registrierung", 100);
            name = AddTextBox("Accountname", "Name deines neuen Accounts eingeben.", 200, 200, Register);
            pw = AddTextBox("Passwort", "Passwort deines neuen Accounts eingeben.", 250, 200, Register);
            AddButton("Erstellen", "Den neuen Account erstellen.", 300, Register);
            AddButton("Zurück", "Zurück zum Hauptmenü.", 400, GUCMenus.Main.Open);
            OnEscape += GUCMenus.Main.Open;
        }

        void Register()
        {
            if (name.Input.Length == 0)
            {
                SetCursor(0);
                return;
            }
            else if (name.Input.Length < 5)
            {
                SetCursor(0);
                SetHelpText("Bitte gebe mindestens 5 Zeichen als Accountnamen ein.");
                return;
            }
            else if (pw.Input.Length == 0)
            {
                SetCursor(1);
                return;
            }
            else if (pw.Input.Length < 5)
            {
                SetCursor(1);
                SetHelpText("Bitte gebe mindestens 5 Zeichen als Passwort ein.");
                return;
            }
            else
            {
                StartupState.clientOptions.name = name.Input;
                StartupState.clientOptions.password = pw.Input;
                StartupState.clientOptions.Save(StartupState.getConfigPath() + "gmp.xml");
                Network.Messages.AccountMessage.Register();
            }
        }
    }*/
}
