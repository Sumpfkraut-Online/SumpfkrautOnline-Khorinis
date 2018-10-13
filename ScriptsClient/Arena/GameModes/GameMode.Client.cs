using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.GUI;

namespace GUC.Scripts.Arena.GameModes
{
    partial class GameMode
    {
        static GUCVisual VictoryVis;
        //static SoundDefinition VictoryWin = new SoundDefinition("SFX_INNOSEYE.WAV");
        static SoundDefinition VictoryWin = new SoundDefinition("LEVELUP.WAV");
        static SoundDefinition VictoryLoss = new SoundDefinition("CHAPTER_01.WAV");

        protected static void DoVictoryStuff(bool win, string winText = "SIEG!", string lossText = "NIEDERLAGE!")
        {
            if (VictoryVis == null)
            {
                const int boxWidth = 260;
                const int boxHeight = 45;
                const int boxOffset = 100;
                var ssize = GUCView.GetScreenSize();
                var vis = new GUCVisual((ssize.X - boxWidth) / 2, boxOffset, boxWidth, boxHeight);
                vis.Font = GUCView.Fonts.Menu;
                vis.SetBackTexture("menu_choice_back.tga");
                var txt = vis.CreateTextCenterX("", 7);
                txt.Show();
                VictoryVis = vis;
            }

            if (win)
            {
                VictoryVis.Texts[0].Text = winText;
                SoundHandler.PlaySound(VictoryWin);
            }
            else
            {
                VictoryVis.Texts[0].Text = lossText;
                SoundHandler.PlaySound(VictoryLoss);
            }
            VictoryVis.Show();
        }

        static GameMode()
        {
            GUCScripts.OnWorldEnter += () =>
            {
                if (IsActive && ArenaClient.GMJoined && NPCClass.Hero == null)
                {
                    ActiveMode.OpenJoinMenu();
                    MissionScreen.Show(ActiveMode.Scenario.MissionInfo);
                }
            };
        }

        public static void ReadGameInfo(PacketReader stream)
        {
            if (!stream.ReadBit())
                return;

            Start(stream.ReadString());
            ActiveMode.Phase = (GamePhase)stream.ReadByte();
            ActiveMode.PhaseEndTime = GameTime.Ticks + stream.ReadUInt() * TimeSpan.TicksPerMillisecond;
        }

        public static void Start(string name)
        {
            var scenario = GameScenario.Get(name);
            if (scenario == null)
                throw new Exception("GameScenario not found: " + name);

            if (ActiveMode != null)
                ActiveMode.End();
            ActiveMode = scenario.GetMode();
            ActiveMode.Start(scenario);
        }

        public abstract Action OpenJoinMenu { get; }
        public abstract ScoreBoardScreen ScoreBoard { get; }
        public virtual void OpenStatusMenu() { }
        protected virtual void End()
        {
            try
            {
                VictoryVis?.Hide();
                MissionScreen.Hide();
                ScoreBoard?.Close();
                ActiveMode = null;
                NPCClass.Hero = null;
            }
            catch (Exception e)
            {
                Log.Logger.LogWarning((VictoryVis != null) + " " + (ScoreBoard != null) + " " + e.ToString());
            }
        }

        public long PhaseEndTime { get; protected set; }

        protected virtual void Start(GameScenario scenario)
        {
            this.Scenario = scenario;
            SetPhase(GamePhase.None);

            OnModeStart?.Invoke();

            Log.Logger.Log(scenario.Name + " startet.");
        }

        public static void ReadPhase(PacketReader stream)
        {
            if (!IsActive)
                return;

            ActiveMode.SetPhase((GamePhase)stream.ReadByte());
        }

        public static event Action OnPhaseChange;
        public static event Action OnModeStart;

        protected virtual void SetPhase(GamePhase phase)
        {
            this.Phase = phase;
            if (phase == GamePhase.WarmUp)
            {
                PhaseEndTime = GameTime.Ticks + Scenario.WarmUpDuration;
            }
            else if (phase == GamePhase.FadeOut)
            {
                PhaseEndTime = GameTime.Ticks + FadeOutDuration;
            }
            else
            {
                PhaseEndTime = GameTime.Ticks + Scenario.FightDuration;
            }

            if (Phase != GamePhase.WarmUp)
                MissionScreen.Hide();

            OnPhaseChange?.Invoke();

            Log.Logger.Log("Set phase " + phase);
        }

    }
}
