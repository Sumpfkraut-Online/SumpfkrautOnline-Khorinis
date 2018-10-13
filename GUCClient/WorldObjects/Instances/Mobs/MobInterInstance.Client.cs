using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobInterInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobInter ret = (vob == null || !(vob is oCMobInter)) ? oCMobInter.Create() : (oCMobInter)vob;
            base.CreateVob(ret);
            ret.SetName(FocusName);
            return ret;
        }
    }
}
