using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobWheelInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobWheel ret = (vob == null || !(vob is oCMobWheel)) ? oCMobWheel.Create() : (oCMobWheel)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
