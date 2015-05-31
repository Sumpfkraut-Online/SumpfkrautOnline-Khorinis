using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Sumpfkraut.GUI;

namespace GUC.Sumpfkraut.Menus
{
    partial class MainMenus
    {
        class ExitMenu : AbstractMenu
        {
            //Ja
            //Nein

            protected override void CreateMenu()
            {
                thisMenu = new GUCMainMenu(0);
                thisMenu.AddText("SumpfkrautOnline verlassen?", 100);
                thisMenu.AddMenuButton("Ja", "Ja, ich möchte SumpfkrautOnline verlassen.", CloseGothic, 200);
                thisMenu.AddMenuButton("Nein", "Nein, ich möchte weiterspielen.", Main.Open, 250);
                thisMenu.OnEscape = Main.Open;
            }

            void CloseGothic(object sender, EventArgs e)
            {
                Gothic.zClasses.CGameManager.ExitGameFunc(WinApi.Process.ThisProcess());
            }
        }
    }
}
