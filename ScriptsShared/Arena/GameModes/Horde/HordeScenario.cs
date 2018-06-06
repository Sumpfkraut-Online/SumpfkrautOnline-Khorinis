using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Types;

namespace GUC.Scripts.Arena.GameModes.Horde
{
    class HordeScenario : GameScenario
    {
        public class Item
        {
            public string ItemDef;
            public Vec3f Position;
            public Angles Angles;
        }

        public class Barrier
        {
            public Vec3f Position;
            public Angles Angles;
            public string Definition;
            public bool AddAfterEvent;
        }

        public class Pair
        {
            public NPCClass Enemy;
            public float CountScale;
        }

        public class Group
        {
            public Pair[] npcs;
            public Vec3f Position;
            public float Range;
        }

        public class Stand
        {
            public Barrier[] Barriers;

            public Vec3f Position;
            public float Range;

            public int Duration; // sec

            public float MaxEnemies;
            public Pair[] Enemies; // enemy + probability
            public Vec3f[] EnemySpawns;

            public NPCClass Boss;

            public string SFXStart;
            public string SFXLoop;
            public string SFXStop;

            public string[] Messages;
        }

        public override GameMode GetMode() { return HordeMode.Instance; }

        public Vec3f SpawnPos;
        public float SpawnRange;
        public Barrier[] SpawnBarriers;
        
        public Item[] Items;

        public NPCClass[] PlayerClasses;

        public Group[] Enemies;
        public Stand[] Stands;
    }
}
