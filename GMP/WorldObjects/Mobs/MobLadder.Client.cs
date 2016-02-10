using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobLadder : MobInter
    {
        new public oCMobLadder gVob { get { return (oCMobLadder)base.gVob; } }
    }
}
