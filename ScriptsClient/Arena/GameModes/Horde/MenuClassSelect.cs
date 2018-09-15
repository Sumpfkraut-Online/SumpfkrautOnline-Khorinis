using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Scripts.Sumpfkraut.GUI.MainMenu;
using GUC.Log;
using GUC.Utilities;
using GUC.GUI;

namespace GUC.Scripts.Arena.GameModes.Horde
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
            AddButton("Zuschauen", "Dem Spielmodus zuschauen.", 300, Spectate);
            AddButton("Zurück", "Zurück ins Hauptmenü.", backButtonOffset, Menus.MainMenu.Menu.Open);
            OnEscape = Menus.MainMenu.Menu.Open;
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
            if (!HordeMode.IsActive || PlayerInfo.HeroInfo.TeamID < TeamIdent.GMSpectator || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
                return;

            base.Open();

            title.Text = "Klasse auswählen.";

            int index = 0;
            foreach (var classDef in HordeMode.ActiveMode.Scenario.PlayerClasses)
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

            GameMode.OnPhaseChange += PhaseChanged;
        }

        public override void Close()
        {
            base.Close();
            GameMode.OnPhaseChange -= PhaseChanged;
        }

        LockTimer lockTimer = new LockTimer(500);
        void SelectClass(int index)
        {
            if (!HordeMode.IsActive || PlayerInfo.HeroInfo.TeamID < TeamIdent.GMSpectator || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
            {
                Close();
                return;
            }

            if (!HordeMode.ActiveMode.Scenario.PlayerClasses.TryGet(index, out NPCClass classDef))
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

        void PhaseChanged()
        {
            if (!HordeMode.IsActive || GameMode.ActiveMode.Phase == GamePhase.FadeOut)
            {
                Close();
                return;
            }
        }
    }
}

