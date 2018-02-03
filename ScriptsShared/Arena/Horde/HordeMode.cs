using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena
{
    enum HordePhase
    {
        WarmUp,
        Fight,
        Stand,
        Victory,
        Lost,
    }

    static partial class HordeMode
    {
        public static event Action<HordePhase> OnPhaseChange;
        public static HordePhase Phase { get; private set; }

        static HordeDef activeDef;
        public static HordeDef ActiveDef { get { return activeDef; } }
    }
}
