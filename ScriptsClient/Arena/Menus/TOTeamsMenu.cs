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

namespace GUC.Scripts.Arena.Menus
{
    class TOTeamsMenu : GUCMainMenu
    {
        public readonly static TOTeamsMenu Menu = new TOTeamsMenu();

        GUCVisualText tdmName;

        protected override void OnCreate()
        {
            tdmName = Back.CreateTextCenterX("", 20);
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

            AddButton("Zuschauen", "TeamObjective zuschauen.", 340, Spectate);
            AddButton("Zurück", "Zurück ins Hauptmenü.", backButtonOffset, MainMenu.Menu.Open);
            OnEscape = MainMenu.Menu.Open;

            arrow = new GUCVisual(0, 0, 20, 20);
            arrow.SetBackTexture("R.tga");
        }

        void Spectate()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.SpectateTeam);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
            Close();
        }

        public override void Open()
        {
            if (!TeamMode.IsRunning)
                return;

            base.Open();

            SetTexts();

            tdmName.Text = TeamMode.ActiveTODef.Name.ToUpper();

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOTeamCount);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);

            TeamMode.OnPhaseChange += TOPhaseChanged;
        }

        public override void Close()
        {
            base.Close();
            arrow.Hide();

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOTeamCount);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);

            TeamMode.OnPhaseChange -= TOPhaseChanged;
        }

        LockTimer lockTimer = new LockTimer(500);
        void SelectTeam(int index)
        {
            if (!TeamMode.IsRunning || TeamMode.Phase == TOPhases.Finish || TeamMode.Phase == TOPhases.None)
            {
                if (TeamMode.TeamDef == null)
                    Sumpfkraut.Menus.ScreenScrollText.AddText("TeamObjective ist vorüber.", GUCView.Fonts.Menu);
                Close();
                return;
            }

            var team = TeamMode.ActiveTODef.Teams.ElementAtOrDefault(index);
            if (team == null)
                return;

            if (team == TeamMode.TeamDef)
            {
                TOClassMenu.Menu.Open();
                Close();
                return;
            }

            if (!lockTimer.IsReady)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOJoinTeam);
            stream.Write((byte)index);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }

        GUCVisual arrow;
        public void UpdateSelectedTeam()
        {
            if (TeamMode.ActiveTODef != null)
            {
                var team = TeamMode.TeamDef;
                int index = TeamMode.ActiveTODef.Teams.IndexOf(team);
                if (team != null && index >= 0)
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

        void SetTexts(List<int> counts = null)
        {
            int index = 0;
            foreach (var team in TeamMode.ActiveTODef.Teams)
            {
                if (index >= items.Count - 2)
                {
                    Logger.LogWarning("Too many teams to show in the menu!");
                    break;
                }

                var button = (MainMenuButton)items[index];
                button.Text = team.Name;
                if (counts != null && index < counts.Count && counts[index] > 0)
                    button.Text += string.Format(" ({0})", counts[index]);
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

        public void ReadCountMessage(PacketReader stream)
        {
            if (!this.isOpen)
                return;

            int count = stream.ReadByte();
            List<int> counts = new List<int>(count);
            for (int i = 0; i < count; i++)
                counts.Add(stream.ReadByte());

            SetTexts(counts);
        }

        void TOPhaseChanged()
        {
            if (!TeamMode.IsRunning || TeamMode.Phase == TOPhases.Finish || TeamMode.Phase == TOPhases.None)
            {
                Sumpfkraut.Menus.ScreenScrollText.AddText("TeamObjective ist vorüber.", GUCView.Fonts.Menu);
                Close();
                return;
            }
        }
    }
}
