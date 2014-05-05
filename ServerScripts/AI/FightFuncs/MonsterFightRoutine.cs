using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs
{
    public static class MonsterFightRoutine
    {
        public static void FightRoutine(NPCProto npc)
        {
            NPCAI ai = npc.getAI();
            NPCProto enemy = ai.EnemyList.First.Value;

            if (enemy.HP == 0)
            {
                ai.EnemyList.Remove(enemy);
                npc.standAnim();
                return;
            }
            else if ((enemy.Position - npc.Position).Length > 2500)//Stop running!
            {
                ai.EnemyList.Remove(enemy);
                npc.standAnim();
                return;
            }

            npc.turnToPosition(enemy.Position);
            npc.gotoPosition(enemy.Position, 400);
        }
    }
}
