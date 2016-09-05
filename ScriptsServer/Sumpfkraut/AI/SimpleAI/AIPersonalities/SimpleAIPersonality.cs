using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Types;
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



        public SimpleAIPersonality (float attackRadius)
        {
            SetObjName("SimpleAIPersonality (default)");
            this.attackRadius = attackRadius;
        }



        public override void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine)
        {
            this.aiMemory = aiMemory ?? new AIMemory();
            this.aiRoutine = aiRoutine ?? new SimpleAIRoutine();
        }



        // moving around

        public void Run (Vec3f direction)
        {
            throw new NotImplementedException();
        }

        public void Walk (Vec3f direction)
        {
            throw new NotImplementedException();
        }

        public void Jump (int forwardVelocity, int upVelocity)
        {
            throw new NotImplementedException();
        }

        public void ClimbLedge (WorldObjects.NPC.ClimbingLedge ledge)
        {
            throw new NotImplementedException();
        }

        public void TurnAround (Vec3f direction, float angularVelocity)
        {
            throw new NotImplementedException();
        }



        // combat actions

        public void Attack (Vec3f direction)
        {
            throw new NotImplementedException();
        }

        public void Attack (VobInst target)
        {
            throw new NotImplementedException();
        }

        public void Attack (List<VobInst> targets)
        {
            throw new NotImplementedException();
        }

        public void DefendAgainst (VobInst defendedVob, VobInst aggressor)
        {
            throw new NotImplementedException();
        }



        // non-hostile actions

        public void Idle ()
        {
            throw new NotImplementedException();
        }

        public void EquipItem (ItemInst item)
        {
            throw new NotImplementedException();
        }

        public void UnequipItem (ItemInst item)
        {
            throw new NotImplementedException();
        }

        public void DrawWeapon (ItemInst item)
        {
            throw new NotImplementedException();
        }



        // can be run anytime to let the aiClients recognize their surrounding actively
        public override void MakeActiveObservation (AIAgent aiAgent)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            NPCInst npc;
            List<VobInst> enemies = new List<VobInst>();

            for (int c = 0; c < aiClients.Count; c++)
            {
                if (aiClients[c].GetType() == typeof(NPCInst))
                {
                    npc = (NPCInst) aiClients[c];
                    if (npc == null) { Print("npc == null");  return; }
                    if (npc.BaseInst == null) { Print("npc.BaseInst == null");  return; }
                    if (npc.BaseInst.World == null) { Print("npc.BaseInst.World == null");  return; }
                    if (npc.BaseInst.World.ScriptObject == null) { Print("npc.BaseInst.World.ScriptObject == null");  return; }
                    if (npc.World == null) { Print("npc.World == null");  return; }
                    if (npc.World.BaseWorld == null) { Print("npc.World.BaseWorld == null"); return; }
                    Print("OINK");

                    npc.World.BaseWorld.ForEachNPCRough(npc.BaseInst, attackRadius, 
                        delegate (WorldObjects.NPC nearNPC)
                    {
                        if (aiAgent.HasAIClient(nearNPC))
                        {
                            enemies.Add((VobInst) nearNPC.ScriptObject);
                        }
                        //// mark every player a threat / enemy
                        //if (nearNPC.IsPlayer)
                        //{
                        //    enemies.Add((VobInst) nearNPC.ScriptObject);
                        //}
                    });
                }
            }

            //Print(enemies.Count);
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
                    List<VobInst> aiClients = aiAgent.AIClients;
                    List<VobInst> enemies = aiAction.AITarget.vobTargets;
                    NPCInst npc;
                    
                    if (enemies.Count < 1) { break; }

                    for (int c = 0; c < aiClients.Count; c++)
                    {
                        // simply hit the enemy by magical means without necessary physical contact :D
                        if (aiClients[c].GetType() == typeof(NPCInst))
                        {
                            npc = (NPCInst) aiClients[c];
                            for (int e = 0; e < enemies.Count; e++)
                            {
                                try
                                {
                                    //((NPCInst) enemies[e]).Hit(npc, 1);
                                    //Print(">>>>>> BAAAAAAAM! <<<<<<");
                                    //WorldObjects.NPC.ActiveAni jumpAni = npc.GetJumpAni();
                                    //if (jumpAni != null)
                                    //{
                                    //    npc.StartAniJump((Visuals.ScriptAni) jumpAni.Ani.AniJob, 0, 10);
                                    //}

                                    Visuals.ScriptAniJob scriptAniJob;
                                    npc.Model.TryGetAniJob((int) Visuals.SetAnis.JumpFwd, out scriptAniJob);
                                    if (scriptAniJob != null)
                                    {
                                        if (npc.GetJumpAni() != null) { continue; }
                                        
                                        npc.StartAniJump(scriptAniJob.DefaultAni, 50, 50);
                                        Print("npc.IsSpawned = " + npc.GetPosition());
                                    }
                                    
                                    // !!! TO DO !!!
                                    // go to enemy or prepare attack (draw weapon) 
                                    // or start / proceeed attack animation
                                }
                                catch (Exception ex)
                                {
                                    MakeLogWarning(ex);
                                }
                            }
                        }
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
                List<VobInst> aiClients = aiAgent.AIClients;
                List<VobInst> enemyVobs = enemies.vobTargets;
                int closestEnemyIndex = -1;
                float closestEnemyRange = float.MaxValue;
                float tempRange = 0f;
                Types.Vec3f tempPosition;
                Types.Vec3f aiClientCenter = new Types.Vec3f(0f, 0f, 0f);

                foreach (VobInst aiClient in aiClients)
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
                if (closestEnemyIndex > -1)
                {
                    List<BaseAIAction> newAIActions = new List<BaseAIAction> { new AttackAIAction(
                        new AITarget( enemies.vobTargets[closestEnemyIndex] )) };
                    aiMemory.SetAIActions(newAIActions);
                }
            }
        }

    }

}
