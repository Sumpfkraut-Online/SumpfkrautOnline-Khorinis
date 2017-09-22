using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Log;
using GUC.Utilities;
using GUC.GUI;

namespace GUC.Scripts.Arena.Menus
{
    class TOTeamsMenu : GUCMainMenu
    {
        public readonly static TOTeamsMenu Menu = new TOTeamsMenu();

        protected override void OnCreate()
        {
            Back.CreateTextCenterX("Team auswählen", 50);

            const int offset = 100;
            const int distance = 50;
            const int backButtonOffset = 350;

            int y, i = 0;
            while ((y = offset + i * distance) < backButtonOffset - distance)
            {
                int index = i;
                AddButton("TEAM", "", y, () => SelectTeam(index));
                i++;
            }
            AddButton("Zurück", "Zurück ins Hauptmenü.", backButtonOffset, MainMenu.Menu.Open);
            OnEscape = MainMenu.Menu.Open;

            arrow = new GUCVisual(0, 0, 20, 20);
            arrow.SetBackTexture("R.tga");
        }

        public override void Open()
        {
            if (!TeamMode.IsRunning)
                return;

            base.Open();

            int index = 0;
            foreach (var team in TeamMode.ActiveTODef.Teams)
            {
                if (index >= items.Count - 1)
                {
                    Logger.LogWarning("Too many teams to show in the menu!");
                    break;
                }

                var button = (MainMenuButton)items[index];
                button.Text = team.Name;
                button.HelpText = team.Name + " als Team wählen.";
                items[index].Enabled = true;
                button.Show();

                index++;
            }

            for (; index < items.Count - 1; index++)
            {
                items[index].Enabled = false;
                items[index].Hide();
            }

            UpdateSelectedTeam();
        }

        public override void Close()
        {
            base.Close();
            arrow.Hide();
        }

        LockTimer lockTimer = new LockTimer(500);
        void SelectTeam(int index)
        {
            if (!TeamMode.IsRunning)
            {
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
    }
}
