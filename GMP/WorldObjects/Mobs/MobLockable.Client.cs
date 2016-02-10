using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Mobs
{
    public abstract partial class MobLockable : MobInter
    {
        new public oCMobLockable gVob { get { return (oCMobLockable)base.gVob; } }
    }
}
