using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class TeamMode
    {
        static long phaseEndTime = 0;
        public static long PhaseEndTime { get { return phaseEndTime; } }

        static TOTeamDef teamDef;
        public static TOTeamDef TeamDef { get { return teamDef; } }

        public static void ReadWarmup(PacketReader stream)
        {
            string name = stream.ReadString();
            if ((activeTODef = TODef.TryGet(name)) == null)
                throw new Exception("TODef not found: " + name);

            phase = TOPhases.Warmup;
            phaseEndTime = GameTime.Ticks + WarmUpDuration;

            TOMessage(string.Format("Team Objective '{0}' startet in wenigen Sekunden!", name));
            Menus.TOInfoScreen.Show();
        }

        public static void ReadStart(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Battle;
            phaseEndTime = GameTime.Ticks + activeTODef.Duration * TimeSpan.TicksPerMinute;
            TOMessage("Der Kampf beginnt!");
        }

        public static void ReadFinish(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Finish;
            phaseEndTime = GameTime.Ticks + FinishDuration;
            TOMessage("Zeit ist vorüber!");

            int count = stream.ReadByte();
            for (int i = 0; i < count; i++)
            {
                int index = stream.ReadByte();
                if (index < activeTODef.Teams.Count)
                {
                    TOMessage(activeTODef.Teams[index].Name + (count > 1 ? " ist ein Gewinner." : " hat gewonnen."));
                }
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
            }
            else if (index < activeTODef.Teams.Count)
            {
                var oldTeam = teamDef;
                teamDef = activeTODef.Teams[index];
                TOMessage(string.Format("Du bist {0} beigetreten.", teamDef.Name));

                Menus.TOTeamsMenu.Menu.UpdateSelectedTeam();
                if (oldTeam != teamDef)
                    Menus.TOClassMenu.Menu.Open();
            }
        }

        public static void ReadEnd(PacketReader stream)
        {
            phase = TOPhases.None;

            TOMessage("Team Objective ist vorüber!");
            Menus.TOInfoScreen.Hide();
            activeTODef = null;
            teamDef = null;
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
            ChatMenu.Menu.AddMessage(ChatMode.Private, text);
            Log.Logger.Log(text);
        }
    }
}
