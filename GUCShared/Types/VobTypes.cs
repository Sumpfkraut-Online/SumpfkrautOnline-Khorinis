using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public enum GUCVobTypes : byte
    {
        Vob,
        NPC,
        Item,

        Projectile,

        Mob,
        MobInter,
        MobFire,
        MobLadder,
        MobSwitch,
        MobWheel,
        MobContainer,
        MobDoor,
        MobBed,

        Maximum
    }
}
