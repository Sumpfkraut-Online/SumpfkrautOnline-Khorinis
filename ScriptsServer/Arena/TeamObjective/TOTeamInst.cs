using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Arena
{
    class TOTeamInst
    {
        public int Score;
        public List<ArenaClient> Players = new List<ArenaClient>(10);
        public TOTeamDef Def;

        public void Reset()
        {
            this.Score = 0;
            this.Players.Clear();
            Def = null;
        }
    }
}
