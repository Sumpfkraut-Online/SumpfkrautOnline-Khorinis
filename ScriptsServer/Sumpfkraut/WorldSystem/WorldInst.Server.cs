using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldInst
    {
        public static readonly List<WorldInst> List = new List<WorldInst>();

        /// <summary> Despawn list for dead npcs </summary>
        public readonly DespawnList<NPCInst> DespawnList_NPC = new DespawnList<NPCInst>(50);
        /// <summary> Despawn list for projectile items (arrows) </summary>
        public readonly DespawnList<ItemInst> DespawnList_PItems = new DespawnList<ItemInst>(3);

        public WorldInst(WorldDef def) : this()
        {
            this.definition = def;
        }

        partial void pCreate()
        {
            this.Weather.StartRainTimer();
            this.Barrier.StartTimer();
        }

        partial void pDelete()
        {
            this.Weather.StopRainTimer();
            this.Barrier.StopTimer();
        }
    }
}
