using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Enumeration;

namespace GUC.Server.Scripts
{
    static class DamageScript
    {
        public static void Init()
        {
            NPC.sOnHit += HitEvent;
        }

        public static void HitEvent(NPC attacker, NPC victim)
        {
            if (victim.AttrHealth > 0)
                victim.AttrHealth--;

            victim.AttrHealthUpdate();
        }
    }
}
