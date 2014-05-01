using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AssessFuncs
{
    public static class AssessDamage
    {
        public static void OnDamage(NPCProto victim, NPCProto attacker, int damage, bool dropUnconscious, bool dropDead){
            NPCAI ai = victim.getAI();

            ai.addEnemy(attacker);
        }
    }
}
