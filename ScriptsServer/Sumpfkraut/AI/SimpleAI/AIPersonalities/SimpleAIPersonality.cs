using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities
{

    public class SimpleAIPersonality : BaseAIPersonality
    {

        new public static readonly string _staticName = "SimpleAIPersonality (static)";



        protected float attackRadius;
        public float AttackRadius { get { return this.attackRadius; } }
        public void SetAttackRadius (float value)
        {
            this.attackRadius = value;
        }



        public SimpleAIPersonality (float awarenessRadius)
        {
            SetObjName("SimpleAIPersonality (default)");
            this.attackRadius = awarenessRadius;
        }



        public override void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine)
        {
            this.aiMemory = aiMemory ?? new AIMemory();
            this.aiRoutine = aiRoutine ?? new SimpleAIRoutine();
        }

        // can be run anytime to let the aiClients recognize their surrounding actively
        public override void MakeActiveObservation (AIAgent aiAgent)
        {
            List<WorldObjects.BaseVob> aiClients = aiAgent.AIClients;
            WorldObjects.NPC npc;
            List<WorldObjects.BaseVob> enemies = new List<WorldObjects.BaseVob>();

            for (int c = 0; c < aiClients.Count; c++)
            {
                if (aiClients[c].GetType() == typeof(WorldObjects.NPC))
                {
                    npc = (WorldObjects.NPC) aiClients[c];
                    npc.World.ForEachNPCRoughInRange(npc, attackRadius, delegate (WorldObjects.NPC nearNPC) 
                    {
                        // mark every player as a threat / enemy
                        if (nearNPC.IsPlayer)
                        {
                            enemies.Add(nearNPC);
                        }
                    });
                }
            }
            
            if (enemies.Count > 0)
            {
                aiMemory.AddAIObservation(new EnemyAIObservation(new AITarget(enemies)));
            }
        }

        public override void ProcessActions (AIAgent aiAgent)
        {
            List<BaseAIAction> aiActions = aiMemory.GetAIActions();
            BaseAIAction aiAction = null;

            for (int i = 0; i < aiActions.Count; i++)
            {
                aiAction = aiActions[i];
                // attack if there is a spotted enemy nearby
                if (aiAction.GetType() == typeof(AttackAIAction))
                {
                    // do not control again, if enemy is still in radius
                    // simply attack the calculated nearest enemy
                    List<WorldObjects.BaseVob> aiClients = aiAgent.AIClients;
                    List<WorldObjects.BaseVob> enemies = aiAction.AITarget.vobTargets;
                    if (enemies.Count < 0) { break; }

                    for (int c = 0; c < aiClients.Count; c++)
                    {
                        // attack or approach enemy
                    }
                }
            }
        }

        public override void ProcessObservations (AIAgent aiAgent)
        {
            // do nothing, if not aiClient is defined (shouldn't happen but oh well)
            if (aiAgent.AIClients.Count < 1) { return; }

            List<BaseAIObservation> aiObservations = aiMemory.GetAIObservations();
            AITarget enemies = null;

            for (int i = 0; i < aiObservations.Count; i++)
            {
                if (aiObservations[i].GetType() == typeof(EnemyAIObservation))
                {
                    enemies = aiObservations[i].AITarget;
                    break;
                }
            }

            if (enemies != null)
            {
                // find enemy who is closest to arithm. center of aiClient-group
                List<WorldObjects.BaseVob> aiClients = aiAgent.AIClients;
                List<WorldObjects.BaseVob> enemyVobs = enemies.vobTargets;
                int closestEnemyIndex = -1;
                float closestEnemyRange = float.MaxValue;
                float tempRange = 0f;
                Types.Vec3f tempPosition;
                Types.Vec3f aiClientCenter = new Types.Vec3f(0f, 0f, 0f);

                foreach (WorldObjects.BaseVob aiClient in aiClients)
                {
                    aiClientCenter += aiClient.GetPosition();
                }
                aiClientCenter.X /= (float) aiClients.Count;
                aiClientCenter.Y /= (float) aiClients.Count;
                aiClientCenter.Z /= (float) aiClients.Count;
                
                for (int e = 0; e < enemyVobs.Count; e++)
                {
                    tempPosition = enemyVobs[e].GetPosition();
                    tempRange = tempPosition.GetDistance(aiClientCenter);

                    if (tempRange < closestEnemyRange)
                    {
                        closestEnemyRange = tempRange;
                        closestEnemyIndex = e;
                    }
                }

                // formulate action to attack closest enemy, overriding all previous actions
                if (closestEnemyIndex > 0)
                {
                    List<BaseAIAction> newAIActions = new List<BaseAIAction> { new AttackAIAction(
                        new AITarget( enemies.vobTargets[closestEnemyIndex] )) };
                    aiMemory.SetAIActions(newAIActions);
                }
            }
        }

    }

}
