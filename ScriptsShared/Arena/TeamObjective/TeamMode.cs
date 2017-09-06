using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class TeamMode
    {
        const long WarmUpDuration = 30 * TimeSpan.TicksPerSecond;
        const long FinishDuration = 30 * TimeSpan.TicksPerSecond;

        static TOPhases phase;
        public static TOPhases Phase { get { return phase; } }
        
        static TODef activeTODef;
        public static TODef ActiveTODef { get { return activeTODef; } }
        public static bool IsRunning { get { return activeTODef != null; } }
    }
}
