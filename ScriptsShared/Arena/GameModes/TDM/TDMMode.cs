using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Utilities;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Network;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    partial class TDMMode : GameMode
    {
        public const int ScoreLimit = 50;

        new public static bool IsActive { get { return GameMode.IsActive && GameMode.ActiveMode is TDMMode; } }
        new public static TDMMode ActiveMode { get { return (TDMMode)GameMode.ActiveMode; } }
        new public TDMScenario Scenario { get { return (TDMScenario)base.Scenario; } }

        public static readonly TDMMode Instance = new TDMMode();

        public override string Name { get { return "Team Deathmatch"; } }
    }
}
