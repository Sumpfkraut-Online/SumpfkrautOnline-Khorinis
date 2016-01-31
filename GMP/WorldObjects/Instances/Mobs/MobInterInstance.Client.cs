using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances
{
    public partial class MobInterInstance : MobInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobInter ret = (vob == null || !(vob is oCMobInter)) ? oCMobInter.Create() : (oCMobInter)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
