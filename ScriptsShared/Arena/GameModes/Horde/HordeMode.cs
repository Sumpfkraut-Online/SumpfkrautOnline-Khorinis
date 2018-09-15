using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    partial class HordeMode : GameMode
    {
        new public static bool IsActive { get { return GameMode.IsActive && GameMode.ActiveMode is HordeMode; } }
        new public static HordeMode ActiveMode { get { return (HordeMode)GameMode.ActiveMode; } }
        new public HordeScenario Scenario { get { return (HordeScenario)base.Scenario; } }

        public static readonly HordeMode Instance = new HordeMode();

        public override string Name { get { return "Horden-Modus"; } }
    }
}
