using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AssessFuncs
{
    public static class AssessOtherDamage
    {
        public static void OnDamage(NPC npc, NPC victim, NPC attacker, 
            int damage, bool dropUnconscious, bool dropDead)
        {
            NPCAI ai = npc.getAI();

            if (victim.HP != 0)
            {
                if (npc.getAttitudeToGuild(victim.getGuild()) == Enumeration.GuildsAttitude.FRIENDLY
                    && npc.getAttitudeToGuild(attacker.getGuild()) != Enumeration.GuildsAttitude.FRIENDLY)
                {
                    ai.addEnemy(attacker);
                }
                else if (npc.getAttitudeToGuild(attacker.getGuild()) == Enumeration.GuildsAttitude.FRIENDLY
                   && npc.getAttitudeToGuild(victim.getGuild()) != Enumeration.GuildsAttitude.FRIENDLY)
                {
                    ai.addEnemy(victim);
                }
            }else if (ai.AssessBodyRoutine != null)
            {
                ai.AssessBodyRoutine(npc, victim);
            }


        }
    }
}
