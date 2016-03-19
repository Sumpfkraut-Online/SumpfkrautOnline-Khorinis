using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobInter : Mob
    {
        new public oCMobInter gVob { get { return (oCMobInter)base.gVob; } }
    }
}
