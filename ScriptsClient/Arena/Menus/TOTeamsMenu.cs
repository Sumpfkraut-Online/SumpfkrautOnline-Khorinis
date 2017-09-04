using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Log;
using GUC.Utilities;

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
        }

        public override void Open()
        {
            var def = ArenaClient.Client.ActiveTODef;
            if (def == null)
                return;

            base.Open();

            int index = 0;
            foreach (var team in def.Teams)
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
        }

        LockTimer lockTimer = new LockTimer(500);
        void SelectTeam(int index)
        {
            var def = ArenaClient.Client.ActiveTODef;
            if (def == null)
                Close();
            
            if (def.Teams.ElementAtOrDefault(index) == null)
                return;
            
            if (!lockTimer.IsReady)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOJoinTeam);
            stream.Write((byte)index);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
        }
    }
}
