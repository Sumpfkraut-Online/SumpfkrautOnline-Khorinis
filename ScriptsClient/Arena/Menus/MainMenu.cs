using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Scripts.Arena.GameModes;

namespace GUC.Scripts.Arena.Menus
{
    class MainMenu : GUCMainMenu
    {
        public readonly static MainMenu Menu = new MainMenu();

        MainMenuButton gameModeButton;
        MainMenuButton freeModeButton;
        protected override void OnCreate()
        {
            gameModeButton = AddButton("Spielmodus", "Spielmodus beitreten.", 140, GameModeStart);
            freeModeButton = AddButton("Freier Modus", "Dem Freien Modus beitreten.", 180, FFAStart);
            AddButton("Charakter editieren", "Deinen Spielcharakter editieren.", 220, CharCreationMenu.Menu.Open);
            AddButton("Spiel verlassen", "Das Spiel schließen.", 320, ExitMenu.Menu.Open);
            OnEscape = () => { if (!GUCScripts.Ingame) Open(); };

            GameMode.OnModeStart += UpdateGameModeButton;
        }

        void UpdateGameModeButton()
        {
            if (GameMode.IsActive)
            {
                gameModeButton.Text = string.Format("{0} ({1})", GameMode.ActiveMode.Name, PlayerInfo.GetInfos().Count(pi => pi.TeamID >= TeamIdent.GMSpectator));
                gameModeButton.Enabled = true;
            }
            else
            {
                gameModeButton.Text = "Spielmodus";
                gameModeButton.Enabled = false;
            }
        }

        void GameModeStart()
        {
            if (ArenaClient.GMJoined)
            {
                GameMode.ActiveMode.OpenJoinMenu();
                return;
            }

            var stream = ArenaClient.GetStream(ScriptMessages.ModeSpectate);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
            Close();
        }

        void FFAStart()
        {
            if (ArenaClient.FFAJoined)
            {
                FreeModeMenu.Instance.Open();
                return;
            }

            ArenaClient.SendSpectateMessage();
            Close();
        }

        public override void Open()
        {
            base.Open();
            UpdateGameModeButton();
            freeModeButton.Text = string.Format("Freier Modus ({0})", PlayerInfo.GetInfos().Count(pi => pi.TeamID <= TeamIdent.FFASpectator));
        }
    }
}
