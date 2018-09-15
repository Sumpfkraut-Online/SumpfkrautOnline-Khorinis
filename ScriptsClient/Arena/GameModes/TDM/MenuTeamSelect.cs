using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Log;
using GUC.Utilities;
using GUC.GUI;
using GUC.Network;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    class MenuTeamSelect : GUCMainMenu
    {
        public readonly static MenuTeamSelect Instance = new MenuTeamSelect();
        

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("TEAM DEATHMATCH", 20);
            Back.CreateTextCenterX("Team auswählen", 100);

            const int offset = 150;
            const int distance = 40;
            const int backButtonOffset = 400;

            int y, i = 0;
            while ((y = offset + i * distance) < 340 - distance)
            {
                int index = i;
                AddButton("TEAM", "", y, () => SelectTeam(index));
                i++;
            }

            AddButton("Zuschauen", "Team Deathmatch zuschauen.", 340, Spectate);
            AddButton("Zurück", "Zurück ins Hauptmenü.", backButtonOffset, Menus.MainMenu.Menu.Open);
            OnEscape = Menus.MainMenu.Menu.Open;

            arrow = new GUCVisual(0, 0, 20, 20);
            arrow.SetBackTexture("R.tga");
        }

        void Spectate()
        {
            if (PlayerInfo.HeroInfo.TeamID != TeamIdent.GMSpectator)
            {
                var stream = ArenaClient.GetStream(ScriptMessages.ModeSpectate);
                ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.ReliableOrdered);
            }
            Close();
        }

        public override void Open()
        {
            if (!TDMMode.IsActive || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
                return;

            base.Open();

            SetTexts();

            GameMode.OnModeStart += this.Close;
            GameMode.OnPhaseChange += this.PhaseChange;
        }

        public override void Close()
        {
            base.Close();
            arrow.Hide();

            GameMode.OnModeStart -= this.Close;
            GameMode.OnPhaseChange -= this.PhaseChange;
        }

        void PhaseChange()
        {
            if (GameMode.ActiveMode.Phase == GamePhase.FadeOut)
                Close();
        }

        LockTimer lockTimer = new LockTimer(500);
        void SelectTeam(int index)
        {
            if (!TDMMode.IsActive)
                return;
            
            var teams = TDMMode.ActiveMode.Scenario.Teams;
            if (index < 0 || index >= teams.Length)
                return;
            
            if ((int)PlayerInfo.HeroInfo.TeamID == index)
            {
                MenuClassSelect.Instance.Open();
                Close();
                return;
            }
            
            if (!lockTimer.IsReady)
                return;
            
            var stream = ArenaClient.GetStream(ScriptMessages.TDMTeamSelect);
            stream.Write((byte)index);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        GUCVisual arrow;
        public void UpdateSelectedTeam()
        {
            if (GameMode.IsActive && GameMode.ActiveMode is TDMMode)
            {
                int index = (int)PlayerInfo.HeroInfo.TeamID;
                if (index >= 0)
                {
                    if (Cast.Try(items[index], out MainMenuButton button))
                    {
                        arrow.SetPosY(button.VPos.Y + GUCView.PixelToVirtualY(5), true);
                        arrow.SetPosX(button.VPos.X - GUCView.PixelToVirtualX(25), true);
                        arrow.Show();
                        return;
                    }
                }
            }
            arrow.Hide();
        }

        void SetTexts()
        {
            int index = 0;
            foreach (var team in TDMMode.ActiveMode.Scenario.Teams)
            {
                if (index >= items.Count - 2)
                {
                    Logger.LogWarning("Too many teams to show in the menu!");
                    break;
                }

                var button = (MainMenuButton)items[index];

                int count = PlayerInfo.GetInfos().Count(pi => (int)pi.TeamID == index);

                button.Text = string.Format("{0} ({1})", team.Name, count);
                button.HelpText = team.Name + " als Team wählen.";
                items[index].Enabled = true;
                button.Show();

                index++;
            }

            for (; index < items.Count - 2; index++)
            {
                items[index].Enabled = false;
                items[index].Hide();
            }

            UpdateSelectedTeam();
        }
    }
}
