using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus;

namespace GUC.Client.Scripts.TFFA
{
    class NameMenu : GUCMainMenu
    {
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Namen eingeben", 70);
            OnEscape = MainMenu.Menu.Open;
        }
    }
}
