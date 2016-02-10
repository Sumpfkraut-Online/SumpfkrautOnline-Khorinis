using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMob ret = (vob == null || !(vob is oCMob)) ? oCMob.Create() : (oCMob)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
