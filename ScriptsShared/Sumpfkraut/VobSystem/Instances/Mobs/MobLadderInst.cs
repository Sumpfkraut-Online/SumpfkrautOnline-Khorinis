using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs;

namespace GUC.Scripts.Sumpfkraut.Mobs
{
    public partial class MobLadderInst : MobInst
    {

        #region properties
        #endregion

        public MobLadderInst(MobDef def)
        {
            Log.Logger.Log("Created Mob Ladder");
            Definition = def;
        }

    }
}
