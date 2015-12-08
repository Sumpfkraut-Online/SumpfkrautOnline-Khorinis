using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum VobType : byte
    {
        None,

        Vob,
        NPC,
        Item,

        Mob,
        MobInter,
        MobFire,
        MobLadder,
        MobSwitch,
        MobWheel,
        MobContainer,
        MobDoor
    }
}
