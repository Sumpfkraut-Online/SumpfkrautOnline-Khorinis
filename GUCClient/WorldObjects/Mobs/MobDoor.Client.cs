﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects.Mobs
{
    public partial class MobDoor : MobLockable
    {
        new public oCMobDoor gVob { get { return (oCMobDoor)base.gVob; } }
    }
}
