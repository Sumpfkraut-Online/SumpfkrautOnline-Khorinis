using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;

namespace GUC.Scripts.Arena
{
    static class TeamMode
    {
        const long FinishDuration = 30 * TimeSpan.TicksPerSecond;

        struct TeamInst
        {
            public List<ArenaClient> Players;
            public int Score;

            public void Reset()
            {
                Players.Clear();
                Score = 0;
            }
        }

        static TeamInst Team1;
        static TeamInst Team2;

        static GUCTimer timer = new GUCTimer();

        static TeamMode()
        {
            Team1.Players = new List<ArenaClient>(10);
            Team2.Players = new List<ArenaClient>(10);
        }

        static int toLoopIndex = 0;
        public static void StartNextTO()
        {
            var todef = TODef.GetAll().ElementAtOrDefault(toLoopIndex++);

            if (toLoopIndex >= TODef.GetAll().Count())
                toLoopIndex = 0;

            StartTO(todef);
        }

        public static void StartTO(string name)
        {
            StartTO(TODef.TryGet(name));
        }

        static TODef activeTODef;
        public static void StartTO(TODef def)
        {
            if (def == null)
                return;

            timer.SetInterval(def.Duration * TimeSpan.TicksPerMinute);
            timer.SetCallback(FinishTO);
            timer.Start();

            activeTODef = def;
        }

        public static void FinishTO()
        {
            timer.SetInterval(FinishDuration);
            timer.SetCallback(EndTO);
        }

        public static void EndTO()
        {
            Team1.Reset();
            Team2.Reset();
        }
    }
}
