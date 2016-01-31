using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Instances
{
    public partial class MobDoorInstance : MobLockableInstance
    {
        public override zCVob CreateVob(zCVob vob = null)
        {
            oCMobDoor ret = (vob == null || !(vob is oCMobDoor)) ? oCMobDoor.Create() : (oCMobDoor)vob;
            base.CreateVob(ret);
            return ret;
        }
    }
}
