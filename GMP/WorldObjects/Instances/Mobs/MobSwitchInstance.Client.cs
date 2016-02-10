using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances.Mobs
{
    public partial class MobSwitchInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobSwitch ret = (vob == null || !(vob is oCMobSwitch)) ? oCMobSwitch.Create() : (oCMobSwitch)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
