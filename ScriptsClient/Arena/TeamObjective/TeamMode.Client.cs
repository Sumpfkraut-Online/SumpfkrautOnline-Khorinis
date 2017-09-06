using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class TeamMode
    {
        static TOTeamDef teamDef;
        public static TOTeamDef TeamDef { get { return teamDef; } }

        public static void ReadWarmup(PacketReader stream)
        {
            string name = stream.ReadString();
            if ((activeTODef = TODef.TryGet(name)) == null)
                throw new Exception("TODef not found: " + name);

            phase = TOPhases.Warmup;

            Log.Logger.Log("TO Warmup: " + name);
            Menus.TOInfoScreen.Show(activeTODef);
        }

        public static void ReadStart(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Battle;
            Log.Logger.Log("TO Start: " + activeTODef.Name);
        }

        public static void ReadFinish(PacketReader stream)
        {
            if (!IsRunning)
                return;

            phase = TOPhases.Finish;

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
    }
}
