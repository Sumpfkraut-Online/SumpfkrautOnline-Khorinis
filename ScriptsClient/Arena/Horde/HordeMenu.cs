using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Log;
using GUC.Utilities;
using GUC.GUI;

namespace GUC.Scripts.Arena
{
    class HordeMenu : GUCMainMenu
    {
        public static readonly HordeMenu Menu = new HordeMenu();

        GUCVisualText title;
        protected override void OnCreate()
        {
            title = Back.CreateTextCenterX("", 50);

            const int offset = 100;
            const int distance = 50;
            const int specButtonOffset = 300;

            int y, i = 0;
            while ((y = offset + i * distance) < specButtonOffset - distance)
            {
                int index = i;
                AddButton("CLASS", "", y, () => SelectClass(index));
                i++;
            }
            AddButton("Zuschauen", "HordeModus zuschauen.", specButtonOffset, Spectate);
            AddButton("Zurück", "Zurück ins Hauptmenü.", specButtonOffset + distance, Arena.Menus.MainMenu.Menu.Open);
            OnEscape = Arena.Menus.MainMenu.Menu.Open;
        }

        void Spectate()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordeSpectate);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);
            Close();
        }

        public override void Open()
        {
            base.Open();

            title.Text = string.Format("Klasse auswählen");

            int index = 0;
            foreach (var classDef in HordeMode.ActiveDef.Classes)
            {
                if (index >= items.Count - 2)
                {
                    Logger.LogWarning("Too many classes to show in the menu!");
                    break;
                }

                var button = (MainMenuButton)items[index];
                button.Text = classDef.Name;
                button.HelpText = classDef.Name + " als Klasse wählen.";
                items[index].Enabled = true;
                button.Show();

                index++;
            }

            for (; index < items.Count - 2; index++)
            {
                items[index].Enabled = false;
                items[index].Hide();
            }
        }

        public override void Close()
        {
            base.Close();
        }

        LockTimer lockTimer = new LockTimer(200);
        void SelectClass(int index)
        {
            var classDef = HordeMode.ActiveDef.Classes.ElementAtOrDefault(index);
            if (classDef == null)
                return;

            if (classDef == ArenaClient.Client.HordeClass)
            {
                Close();
                return;
            }

            if (!lockTimer.IsReady)
                return;

            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.HordeJoin);
            stream.Write((byte)index);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);

            ArenaClient.Client.HordeClass = classDef;

            Close();
        }
    }
}
