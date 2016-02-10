using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobBedInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobBed ret = (vob == null || !(vob is oCMobBed)) ? oCMobBed.Create() : (oCMobBed)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
