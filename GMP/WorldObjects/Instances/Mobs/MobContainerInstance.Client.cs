using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobContainerInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobContainer ret = (vob == null || !(vob is oCMobContainer)) ? oCMobContainer.Create() : (oCMobContainer)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
