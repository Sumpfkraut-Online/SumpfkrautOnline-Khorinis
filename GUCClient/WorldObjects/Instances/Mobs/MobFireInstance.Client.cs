using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobFireInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobFire ret = (vob == null || !(vob is oCMobFire)) ? oCMobFire.Create() : (oCMobFire)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
