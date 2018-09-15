using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;
using GUC.Utilities;

namespace GUC.Scripts.Arena.GameModes
{
    abstract class GameScenario
    {
        protected static List<GameScenario> scenarios = new List<GameScenario>();
        public static ReadOnlyList<GameScenario> Scenarios { get { return scenarios; } }

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

        public PosAng SpawnPos;
        public float SpawnRange;
        public Barrier[] SpawnBarriers;

        public string SpawnWorld;
        public Vec3f SpawnWorldPos;
        public float SpawnWorldRange;

        public PosAng SpecPoint;

        public float MaxWorldDistance = float.MaxValue;
        public float MaxHeight = float.MaxValue;
        public float MaxDepth = float.MinValue;



        public class Barrier
        {
            public Vec3f Position;
            public Angles Angles;
            public string Definition;
            public bool AddAfterEvent;

            public Barrier(string vobDef, float x, float y, float z, float pitch, float yaw, float roll, bool addAfterEvent = false)
            {
                this.Definition = vobDef;
                this.Position = new Vec3f(x, y, z);
                this.Angles = new Angles(pitch, yaw, roll);
                this.AddAfterEvent = addAfterEvent;
            }
        }

    }
}
