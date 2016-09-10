using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.Menus.MainMenus
{
    class LoginMenu : GUCMainMenu
    {
        public readonly static LoginMenu Menu = new LoginMenu();

        MainMenuTextBox name;
        MainMenuTextBox pw;

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Anmeldung", 100);

            name = AddTextBox("Accountname:", "Name deines Accounts eingeben.", 200, 200, Login);
            pw = AddTextBox("Passwort:", "Passwort deines Accounts eingeben.", 250, 200, Login);
            AddButton("Einloggen", "In den Account einloggen.", 300, Login);
            AddButton("Zurück", "Zurück zum Hauptmenü.", 400, MainMenu.Menu.Open);
            OnEscape = MainMenu.Menu.Open;
        }

        void Login()
        {
            //var strm = GameClient.Client.GetMenuMsgStream();
            //GameClient.Client.SendMenuMsg(strm, PktPriority.High, PktReliability.Reliable);

            /*if (name.Input.Length == 0)
            {
                SetCursor(0);
                return;
            }
            else if (pw.Input.Length == 0)
            {
                SetCursor(1);
                return;
            }
            else
            {
                StartupState.clientOptions.name = name.Input;
                StartupState.clientOptions.password = pw.Input;
                StartupState.clientOptions.Save(StartupState.getConfigPath() + "gmp.xml");
                Network.Messages.AccountMessage.Login();
            }*/
        }
    }
}
