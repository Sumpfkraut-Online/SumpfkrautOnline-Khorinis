using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;

namespace GUC.Scripts.Arena.Menus
{
    class FreeModeMenu : GUCMainMenu
    {
        public readonly static FreeModeMenu Instance = new FreeModeMenu();
        
        protected override void OnCreate()
        {
            Back.CreateTextCenterX("FREIER MODUS", 20);

            AddButton("Mitspielen", "Dem Freien Modus beitreten.", 140, Join);
            AddButton("Zuschauen", "Dem Freien Modus zuschauen.", 180, Spectate);

            AddButton("Zurück", "Zurück ins Hauptmenü.", 300, MainMenu.Menu.Open);
            OnEscape = MainMenu.Menu.Open;
        }

        void Join()
        {
            if (PlayerInfo.HeroInfo.TeamID != GameModes.TeamIdent.FFAPlayer)
            {
                ArenaClient.SendJoinGameMessage();
            }
            Close();
        }

        void Spectate()
        {
            if (PlayerInfo.HeroInfo.TeamID != GameModes.TeamIdent.FFASpectator)
            {
                ArenaClient.SendSpectateMessage();
            }
            Close();
        }
    }
}
