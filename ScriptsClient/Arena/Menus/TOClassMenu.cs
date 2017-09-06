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
    class TOClassMenu : GUCMainMenu
    {
        public readonly static TOClassMenu Menu = new TOClassMenu();

        GUCVisualText title;
        protected override void OnCreate()
        {
            title = Back.CreateTextCenterX("", 50);

            const int offset = 100;
            const int distance = 50;
            const int backButtonOffset = 350;

            int y, i = 0;
            while ((y = offset + i * distance) < backButtonOffset - distance)
            {
                int index = i;
                AddButton("CLASS", "", y, () => SelectClass(index));
                i++;
            }
            AddButton("Zurück", "Zurück ins Teammenü.", backButtonOffset, TOTeamsMenu.Menu.Open);
            OnEscape = TOTeamsMenu.Menu.Open;
        }

        public override void Open()
        {
            if (!TeamMode.IsRunning)
                return;

            base.Open();

            title.Text = string.Format("Klasse für '{0}' auswählen.", TeamMode.TeamDef.Name);

            int index = 0;
            foreach (var team in TeamMode.TeamDef.ClassDefs)
            {
                if (index >= items.Count - 1)
                {
                    Logger.LogWarning("Too many classes to show in the menu!");
                    break;
                }

                var button = (MainMenuButton)items[index];
                button.Text = team.Name;
                button.HelpText = team.Name + " als Klasse wählen.";
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
        void SelectClass(int index)
        {
            if (TeamMode.TeamDef == null)
                Close();

            if (TeamMode.TeamDef.ClassDefs.ElementAtOrDefault(index) == null)
                return;

            if (!lockTimer.IsReady)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.TOSelectClass);
            stream.Write((byte)index);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);

            Close();
        }
    }
}
