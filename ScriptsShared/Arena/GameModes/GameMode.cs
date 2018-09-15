using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripting;

namespace GUC.Scripts.Arena.GameModes
{
    abstract partial class GameMode
    {
        public const long FadeOutDuration = 30 * TimeSpan.TicksPerSecond;

        public static GameMode ActiveMode { get; private set; }
        public static bool IsActive { get { return ActiveMode != null; } }

        public virtual string Name { get; }
        public GameScenario Scenario { get; private set; }

        public GamePhase Phase { get; private set; }
    }
}
