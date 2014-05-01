using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AssessFuncs
{
    public static class AssessTarget
    {
        public static void OnAssessTarget(NPCProto npc, NPCProto target)
        {
            NPCAI ai = npc.getAI();
            //Check if target is Enemy and call AssessEnemy:
            if (npc.getAttitudeToGuild(target.getGuild()) == Enumeration.GuildsAttitude.HOSTILE 
                && target.HP != 0 && 
                !target.isUnconscious && ai.AssessEnemyRoutine != null)
            {
                ai.AssessEnemyRoutine(npc, target);
                Console.WriteLine("Enemy found!");
            }

            if (target.HP == 0 && ai.AssessBodyRoutine != null)
            {
                ai.AssessBodyRoutine(npc, target);
            }
        }
    }
}
