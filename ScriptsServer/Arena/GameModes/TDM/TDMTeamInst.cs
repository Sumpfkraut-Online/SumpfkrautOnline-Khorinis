using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using GUC.Types;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    class TDMTeamInst
    {
        public int Score;
        public List<ArenaClient> Players = new List<ArenaClient>(10);
        public TDMScenario.TeamDef Definition;

        int spawnIndex = 0;
        public PosAng GetSpawnPoint()
        {
            if (spawnIndex >= Definition.SpawnPoints.Length)
                spawnIndex = 0;

            return Definition.SpawnPoints[spawnIndex++];
        }
    }
}
