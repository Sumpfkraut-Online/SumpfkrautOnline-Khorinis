using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;

namespace GUC.Scripts.Arena.GameModes
{
    abstract class GameScenario
    {
        protected static List<GameScenario> scenarios = new List<GameScenario>();
        public static GameScenario Get(string name) { return scenarios.Find(s => string.Equals(s.Name, name, StringComparison.OrdinalIgnoreCase)); }
        public static GameScenario Get(int id) { return id >= 0 && id < scenarios.Count ? scenarios[id] : null; }
        public static int Count { get { return scenarios.Count; } }

        static GameScenario()
        {
            Log.Logger.Log("Load Game Scenarios...");
            //TDM.TDMScenario.Init();
            //BattleRoyale.BRScenario.Init();
            Horde.HordeScenario.Init();

            Random rand = new Random("Scenarios".GetHashCode());
            scenarios = scenarios.OrderBy(o => rand.Next()).ToList();
        }

        public abstract GameMode GetMode();

        public long WarmUpDuration = 30 * TimeSpan.TicksPerSecond;
        public long FightDuration;
        public string Name;
        public string WorldPath;
        public WorldTime WorldTime = new WorldTime(0, 8);
        public float WorldTimeScale = 1.0f;
        public float WorldBarrier = -1;
        public float WorldWeather = -1;

        public PosAng SpecPoint;

        public float MaxWorldDistance = float.MaxValue;
        public float MaxHeight = float.MaxValue;
        public float MaxDepth = float.MinValue;
    }
}
