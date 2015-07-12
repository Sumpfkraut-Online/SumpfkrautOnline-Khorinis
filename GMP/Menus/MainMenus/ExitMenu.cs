using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Client.Menus.MainMenus
{
    class ExitMenu : GUCMainMenu
    {
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("SumpfkrautOnline verlassen?", 100);
            AddButton("Ja", "Ja, ich möchte SumpfkrautOnline verlassen.", 200, CloseGothic);
            AddButton("Nein", "Nein, ich möchte weiterspielen.", 250, GUCMenus.Main.Open);
            OnEscape = GUCMenus.Main.Open;
        }

        void CloseGothic()
        {
            Gothic.zClasses.CGameManager.ExitGameFunc(Program.Process);
        }
    }
}
