using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects
{
    public partial class MobContainer : MobLockable
    {
        new public oCMobContainer gVob { get; protected set; }
    }
}
