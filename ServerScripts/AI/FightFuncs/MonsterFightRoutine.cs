using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Enumeration;
using GUC.Server.Scripts.AI.FightFuncs.fightStates;

namespace GUC.Server.Scripts.AI.FightFuncs
{
    public static class MonsterFightRoutine
    {
        static Random rand = new Random();
        public static void FightRoutine(NPC npc)
        {
            //Draw Weapon:


            NPCAI ai = npc.getAI();
            NPC enemy = ai.EnemyList.First.Value;
            npc.readyBestWeapon(enemy);



            if (enemy.HP == 0)
            {
                ai.EnemyList.Remove(enemy);
                npc.standAnim();
                npc.unreadyWeapon();
                return;
            }
            else if ((enemy.Position - npc.Position).Length > 3000)//Stop running!
            {
                ai.EnemyList.Remove(enemy);
                npc.standAnim();
                npc.unreadyWeapon();
                return;
            }

            if (npc.getAI().FightStates.First != null)
            {
                npc.getAI().FightStates.First.Value.update();
                return;
            }


            npc.turnToPosition(enemy.Position);
            if (rand.NextDouble() > 0.5)
            {
                npc.getAI().FightStates.AddLast(new AnimState(npc, "T_FISTPARADEJUMPB"));
                npc.getAI().FightStates.AddLast(new WaitState(npc, 10000 * 100));
            }

            if (npc.Position.getDistance(enemy.Position) > npc.getAttackRange())
            {
                npc.getAI().FightStates.AddLast(new GotoState(npc, enemy, npc.getAttackRange()));
            }
            else
            {
                npc.getAI().FightStates.AddLast(new AnimState(npc, npc.getFightRunAnimation()));
                npc.getAI().FightStates.AddLast(new DamageState(npc, enemy));
                npc.getAI().FightStates.AddLast(new WaitState(npc, 10000 * 200));

            }


            //if (npc.gotoPosition(enemy.Position, 200))
            //{
            //    npc.standAnim();
            //    npc.playAnimation(npc.getFightAnimation());
            //}
        }
    }
}
