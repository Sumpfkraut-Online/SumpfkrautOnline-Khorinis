using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    partial class BRMode : GameMode
    {
        new public static bool IsActive { get { return GameMode.IsActive && GameMode.ActiveMode is BRMode; } }
        new public static BRMode ActiveMode { get { return (BRMode)GameMode.ActiveMode; } }
        new public BRScenario Scenario { get { return (BRScenario)base.Scenario; } }

        public static readonly BRMode Instance = new BRMode();

        public override string Name { get { return "Battle Royale"; } }
    }
}
