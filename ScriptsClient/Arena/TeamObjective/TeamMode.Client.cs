﻿using System;
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

            Log.Logger.Log("TO Warmup: " + name);
            Menus.TOInfoScreen.Show();
        }

        public static void ReadStart(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Battle;
            phaseEndTime = GameTime.Ticks + activeTODef.Duration * TimeSpan.TicksPerMinute;
            Log.Logger.Log("TO Start: " + activeTODef.Name);
        }

        public static void ReadFinish(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Finish;
            phaseEndTime = GameTime.Ticks + FinishDuration;

            Log.Logger.Log("TO Finish: " + activeTODef.Name);

            int count = stream.ReadByte();
            List<TOTeamDef> winners = new List<TOTeamDef>(count);
            for (int i = 0; i < count; i++)
            {
                int index = stream.ReadByte();
                if (index < activeTODef.Teams.Count)
                {
                    winners.Add(activeTODef.Teams[index]);
                    Log.Logger.Log(activeTODef.Teams[index].Name + " is a winner.");
                }
            }
        }
        
        public static void ReadJoinTeam(PacketReader stream)
        {
            if (!IsRunning)
                return;

            int index = stream.ReadByte();
            if (index < activeTODef.Teams.Count)
            {
                var oldTeam = teamDef;
                teamDef = activeTODef.Teams[index];
                Log.Logger.Log("Joined Team " + teamDef.Name);

                if (oldTeam != teamDef)
                    Menus.TOClassMenu.Menu.Open();

            }
        }

        public static void ReadEnd(PacketReader stream)
        {
            phase = TOPhases.None;

            Log.Logger.Log("TO End");
            Menus.TOInfoScreen.Hide();
            activeTODef = null;
            teamDef = null;
        }

        public static void ReadGameInfo(PacketReader stream)
        {
            phase = (TOPhases)stream.ReadByte();
            Log.Logger.Log(phase);
            if (phase != TOPhases.None)
            {
                string name = stream.ReadString();
                if ((activeTODef = TODef.TryGet(name)) == null)
                    throw new Exception("TODef not found: " + name);

                phaseEndTime = GameTime.Ticks + stream.ReadUInt() * TimeSpan.TicksPerMillisecond;
                Menus.TOInfoScreen.Show();
            }
        }
    }
}