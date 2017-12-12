using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena
{
    enum HordePhase
    {
        Intermission,
        Fight,
        Victory,
        Lost,
    }

    static partial class HordeMode
    {
        public static HordePhase Phase { get; private set; }

        static HordeDef activeDef;
        public static HordeDef ActiveDef { get { return activeDef; } }
        
        static int activeSectionIndex;
        static HordeSection ActiveSection { get { return activeDef.Sections[activeSectionIndex]; } }
    }
}
