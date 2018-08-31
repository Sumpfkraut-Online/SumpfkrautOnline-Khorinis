using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    class BRJoinMenu : GUCMainMenu
    {
        public readonly static BRJoinMenu Instance = new BRJoinMenu();

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("BATTLE ROYALE", 20);

            AddButton("Mitspielen", "Dem Spielmodus beitreten.", 140, Join);
            AddButton("Zuschauen", "Dem Spielmodus zuschauen.", 180, Spectate);

            AddButton("Zurück", "Zurück ins Hauptmenü.", 300, Arena.Menus.MainMenu.Menu.Open);
            OnEscape = Arena.Menus.MainMenu.Menu.Open;
        }

        void Join()
        {
            if (PlayerInfo.HeroInfo.TeamID < TeamIdent.GMPlayer)
            {
                var stream = ArenaClient.GetStream(ScriptMessages.BRJoin);
                ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
            }
            Close();
        }

        void Spectate()
        {
            if (PlayerInfo.HeroInfo.TeamID != TeamIdent.GMSpectator)
            {
                var stream = ArenaClient.GetStream(ScriptMessages.ModeSpectate);
                ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
            }
            Close();
        }
    }
}
