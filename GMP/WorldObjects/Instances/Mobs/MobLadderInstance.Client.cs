using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobLadderInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobLadder ret = (vob == null || !(vob is oCMobLadder)) ? oCMobLadder.Create() : (oCMobLadder)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
