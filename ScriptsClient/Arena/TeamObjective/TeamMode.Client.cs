using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Menus;

namespace GUC.Scripts.Arena
{
    static partial class TeamMode
    {
        static long phaseEndTime = 0;
        public static long PhaseEndTime { get { return phaseEndTime; } }

        static TOTeamDef teamDef;
        public static TOTeamDef TeamDef { get { return teamDef; } }

        public static event Action OnPhaseChange;

        public static void ReadWarmup(PacketReader stream)
        {
            string name = stream.ReadString();
            if ((activeTODef = TODef.TryGet(name)) == null)
                throw new Exception("TODef not found: " + name);

            phase = TOPhases.Warmup;
            phaseEndTime = GameTime.Ticks + WarmUpDuration;
            OnPhaseChange?.Invoke();

            TOMessage(string.Format("Team Objective '{0}' startet in wenigen Sekunden!", name));
            Menus.TOInfoScreen.Show();
        }

        public static void ReadStart(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Battle;
            phaseEndTime = GameTime.Ticks + activeTODef.Duration * TimeSpan.TicksPerMinute;
            OnPhaseChange?.Invoke();

            if (teamDef == null)
                return;
            TOMessage("Der Kampf beginnt!");
        }

        public static void ReadFinish(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Finish;
            phaseEndTime = GameTime.Ticks + FinishDuration;
            OnPhaseChange?.Invoke();

            if (teamDef == null)
                return;

            TOMessage("Der Kampf ist vorüber!");

            int count = stream.ReadByte();
            List<TOTeamDef> winners = new List<TOTeamDef>(count);
            for (int i = 0; i < count; i++)
            {
                int index = stream.ReadByte();
                if (index < activeTODef.Teams.Count)
                    winners.Add(activeTODef.Teams[index]);
            }

            if (winners.Count == 0 || winners.Count == activeTODef.Teams.Count)
            {
                TOMessage("Unentschieden.");
            }
            else if (winners.Count == 1)
            {
                TOMessage(winners[0].Name + " hat gewonnen.");
            }
            else
            {
                string message = "";
                for (int i = 0; i < winners.Count; i++)
                {
                    message += winners[i].Name;
                    if (i < winners.Count - 2)
                        message += ", ";
                    else if (i == winners.Count - 2)
                        message += " und ";
                }
                TOMessage(message + " haben gewonnen.");
            }
        }

        public static void ReadJoinTeam(PacketReader stream)
        {
            if (!IsRunning)
                return;

            int index = stream.ReadSByte();
            if (index < 0)
            {
                teamDef = null;
                ArenaClient.Client.ClassDef = null;
            }
            else if (index < activeTODef.Teams.Count)
            {
                var oldTeam = teamDef;
                teamDef = activeTODef.Teams[index];
                TOMessage(string.Format("Du bist {0} beigetreten.", teamDef.Name));

                Menus.TOTeamsMenu.Menu.UpdateSelectedTeam();
                if (oldTeam != teamDef)
                {
                    ArenaClient.Client.ClassDef = null;
                    Menus.TOClassMenu.Menu.Open();
                }
            }
        }

        public static void ReadEnd(PacketReader stream)
        {
            phase = TOPhases.None;
            OnPhaseChange?.Invoke();

            //TOMessage("Team Objective ist vorüber!");
            Menus.TOInfoScreen.Hide();
            activeTODef = null;
            teamDef = null;
            ArenaClient.Client.ClassDef = null;
        }

        public static void ReadGameInfo(PacketReader stream)
        {
            phase = (TOPhases)stream.ReadByte();
            if (phase != TOPhases.None)
            {
                string name = stream.ReadString();
                if ((activeTODef = TODef.TryGet(name)) == null)
                    throw new Exception("TODef not found: " + name);

                phaseEndTime = GameTime.Ticks + stream.ReadUInt() * TimeSpan.TicksPerMillisecond;
                Menus.TOInfoScreen.Show();
            }
        }

        static void TOMessage(string text)
        {
            //ChatMenu.Menu.AddMessage(ChatMode.Private, text);
            if (GUCMenu.GetActiveMenus().FirstOrDefault(m => m is Sumpfkraut.Menus.MainMenus.GUCMainMenu) == null)
                ScreenScrollText.AddText(text, GUI.GUCView.Fonts.Menu);
            Log.Logger.Log(text);
        }
    }
}
