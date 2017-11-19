using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using GUC.Types;

namespace GUC.Scripts.Arena
{
    class TOTeamInst
    {
        public int Score;
        public List<ArenaClient> Players = new List<ArenaClient>(10);
        public TOTeamDef Def;

        public TOTeamInst(TOTeamDef def)
        {
            this.Def = def ?? throw new ArgumentNullException("def");
        }

        int spawnIndex = 0;
        public ValueTuple<Vec3f, Angles> GetSpawnPoint()
        {
            if (spawnIndex >= Def.SpawnPoints.Count())
                spawnIndex = 0;

            return Def.SpawnPoints.ElementAtOrDefault(spawnIndex++);
        }
    }
}
