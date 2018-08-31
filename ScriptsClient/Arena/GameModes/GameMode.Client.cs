using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena.GameModes
{
    partial class GameMode
    {
        static GameMode()
        {
            GUCScripts.OnWorldEnter += () =>
            {
                if (IsActive && ArenaClient.GMJoined)
                    ActiveMode.OpenJoinMenu();
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
            ScoreBoard?.Close();
            ActiveMode = null;
        }

        public long PhaseEndTime { get; protected set; }

        protected virtual void Start(GameScenario scenario)
        {
            this.Scenario = scenario;
            //this.SetPhase(GamePhase.WarmUp);

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

            OnPhaseChange?.Invoke();

            Log.Logger.Log("Set phase " + phase);
        }
    }
}
