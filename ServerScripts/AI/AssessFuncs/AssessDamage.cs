using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AssessFuncs
{
    public static class AssessDamage
    {
        public static void OnDamage(NPCProto npc, NPCProto victim, NPCProto attacker, int damage, bool dropUnconscious, bool dropDead){
            NPCAI ai = victim.getAI();

            if (victim.HP == 0)
            {
                victim.standAnim();
                victim.playAnimation("S_DEADB");
                return;
            }

            if (attacker == null)
                return;
            ai.addEnemy(attacker);
        }
    }
}
