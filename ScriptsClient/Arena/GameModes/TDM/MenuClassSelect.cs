using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Log;
using GUC.Utilities;
using GUC.GUI;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    class MenuClassSelect : GUCMainMenu
    {
        public readonly static MenuClassSelect Instance = new MenuClassSelect();

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
            AddButton("Zurück", "Zurück ins Team-Menü.", backButtonOffset, MenuTeamSelect.Instance.Open);
            OnEscape = MenuTeamSelect.Instance.Open;
        }

        public override void Open()
        {
            if (!TDMMode.IsActive || PlayerInfo.HeroInfo.TeamID < 0 || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
                return;

            base.Open();

            title.Text = string.Format("Klasse für '{0}' auswählen.", TDMMode.HeroTeam.Name);

            int index = 0;
            foreach (var team in TDMMode.HeroTeam.Classes)
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

            GameMode.OnPhaseChange += TOPhaseChanged;
        }

        public override void Close()
        {
            base.Close();
            GameMode.OnPhaseChange -= TOPhaseChanged;
        }

        LockTimer lockTimer = new LockTimer(500);
        void SelectClass(int index)
        {
            if (!TDMMode.IsActive || PlayerInfo.HeroInfo.TeamID < 0 || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
            {
                Close();
                return;
            }

            if (!TDMMode.HeroTeam.Classes.TryGet(index, out NPCClass classDef))
                return;

            if (classDef == NPCClass.Hero)
            {
                Close();
                return;
            }

            if (!lockTimer.IsReady)
                return;

            var stream = ArenaClient.GetStream(ScriptMessages.ModeClassSelect);
            stream.Write((byte)index);
            ArenaClient.SendScriptMessage(stream, NetPriority.Low, NetReliability.Reliable);

            NPCClass.Hero = classDef;

            Close();
        }

        void TOPhaseChanged()
        {
            if (!TDMMode.IsActive || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
            {
                Close();
                return;
            }
        }
    }
}

