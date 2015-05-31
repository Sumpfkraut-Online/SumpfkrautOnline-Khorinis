using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;
using GUC.Sumpfkraut.Login;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        class CharListMenu : AbstractMenu
        {
            //20 slots filled with characters of the logged in account
            //back

            public CharListMenu()
            {
                LoginMessage.GetMsg().OnLogin += ReceiveCharList;
            }

            protected override void CreateMenu()
            {
                thisMenu = new GUCMainMenu(2);
                thisMenu.AddCharList(20, 40, 60, SelectEmptySlot, SelectCharacter);
                thisMenu.AddText("Charakterauswahl", 80, 15);
                thisMenu.AddCharacter(378, 8, 277, 208);
                thisMenu.AddMenuButton("Ausloggen", "Aus dem eingeloggten Account ausloggen.", Main.Open, 150, 425);
                thisMenu.OnEscape = Main.Open;
            }

            void ReceiveCharList(List<Sumpfkraut.Login.LoginMessage.CharInfo> chars)
            {
                Open(); thisMenu.FillCharList(chars);
            }

            void SelectCharacter(object sender, EventArgs e)
            {
                LoginMessage.GetMsg().StartInWorld(thisMenu.GetCharIndex());
            }

            void SelectEmptySlot(object sender, EventArgs e)
            {
                ((CharCreationMenu)CharCreation).Open(thisMenu.GetCharIndex());
            }
        }
    }
}
