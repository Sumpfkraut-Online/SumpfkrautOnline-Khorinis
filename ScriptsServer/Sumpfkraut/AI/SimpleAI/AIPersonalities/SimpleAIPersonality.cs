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



        // is not null if there is a guide command active to automatically
        // guide the vobs / npcs to  a specified destination
        protected GUC.WorldObjects.VobGuiding.GuideCmd guideCommand;

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



        
        // utility

        public bool TryFindClosestTarget (VobInst vob, AITarget aiTarget, out VobInst closestTarget)
        {
            closestTarget = null;
            List<VobInst> vobTargets = aiTarget.vobTargets;
            int closestTargetIndex = -1;
            float closestTargetRange = float.MaxValue;
            float currTargetRange = float.MaxValue;

            for (int t = 0; t < vobTargets.Count; t++)
            {
                currTargetRange = vob.GetPosition().GetDistance(vobTargets[t].GetPosition());
                if (currTargetRange <= closestTargetRange)
                {
                    closestTargetIndex = t;
                    closestTargetRange = currTargetRange;
                }
            }

            if (closestTargetIndex == -1) { return false; }
            else
            {
                closestTarget = vobTargets[closestTargetIndex];
                return true;
            }
        }
        
        
        
        // moving around

        public void RunMode (AIAgent aiAgent)
        {
            throw new NotImplementedException();

            //List<VobInst> aiClients = aiAgent.AIClients;
            //for (int i = 0; i < aiClients.Count; i++)
            //{
            //    //aiClients[i]....
            //}
        }

        public void WalkMode (AIAgent aiAgent)
        {
            throw new NotImplementedException();
        }

        public void GoTo (AIAgent aiAgent, Vec3f position)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            for (int i = 0; i < aiClients.Count; i++)
            {
                AI.GuideCommands.GoToPosCommand cmd = new AI.GuideCommands.GoToPosCommand(position);
                aiClients[i].BaseInst.SetGuideCommand(cmd);
                guideCommand = cmd;
            }
        }

        public void GoTo (AIAgent aiAgent, VobInst target)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            for (int i = 0; i < aiClients.Count; i++)
            {
                AI.GuideCommands.GoToVobCommand cmd = new AI.GuideCommands.GoToVobCommand(target);
                aiClients[i].BaseInst.SetGuideCommand(cmd);
                guideCommand = cmd;
            }
        }

        public void Follow (AIAgent aiAgent, AITarget aiTarget)
        {
            throw new NotImplementedException();
        }

        public void Jump (AIAgent aiAgent, int forwardVelocity, int upVelocity)
        {
            throw new NotImplementedException();
        }

        public void ClimbLedge (AIAgent aiAgent, WorldObjects.NPC.ClimbingLedge ledge)
        {
            throw new NotImplementedException();
        }

        public void TurnAround (AIAgent aiAgent, Vec3f direction, float angularVelocity)
        {
            throw new NotImplementedException();
        }



        // combat actions

        public void Attack (AIAgent aiAgent, Vec3f direction)
        {
            throw new NotImplementedException();
        }

        public void Attack (VobInst aggressor, VobInst target)
        {
            Type aggressorType = aggressor.GetType();
            Type targetType = target.GetType();
            float distance = aggressor.GetPosition().GetDistance(target.GetPosition());
            float totalRadius = aggressor.Model.Radius + target.Model.Radius;

            // some sort of effective distance at which attacks can be conducted
            // modify as much as needed to enhance the combat ai
            // e.g. take weapons, animation movements into account
            float fightDistance = totalRadius;

            if (distance <= totalRadius)
            {
                if (aggressorType == typeof(NPCInst))
                {
                    NPCInst aggressorNPC = (NPCInst) aggressor;

                    // do not interrupt an ongoing fight ani
                    if (aggressorNPC.GetFightAni() != null) { return; }

                    if (aggressorNPC.DrawnWeapon == null)
                    {
                        // TODO: draw weapon first if necessary
                    }

                    Visuals.ScriptAniJob scriptAniJob;
                    aggressorNPC.Model.TryGetAniJob((int) Visuals.SetAnis.Attack1HFwd1, out scriptAniJob);
                    if (scriptAniJob != null)
                    {                                       
                        aggressorNPC.
                        Print("npc.IsSpawned = " + npc.GetPosition());
                    }
                }
                else
                {
                    // do nothiing you liveless object!!
                }
            }
        }

        public void Attack (AIAgent aiAgent, AITarget aiTarget)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            List<VobInst> targets = aiTarget.vobTargets;
            VobInst closestTarget = null;

            if (aiTarget.vobTargets.Count < 1) { return; }

            // for now, let each aiClient attack its closest foe
            for (int c = 0; c < aiClients.Count; c++)
            {
                if (TryFindClosestTarget(aiClients[c], aiTarget, out closestTarget))
                {
                    Attack(aiClients[c], closestTarget);
                }
            }
        }

        public void DefendAgainst (AIAgent aiAgent, VobInst defendedVob, VobInst aggressor)
        {
            throw new NotImplementedException();
        }



        // non-hostile actions

        public void Idle (AIAgent aiAgent)
        {
            throw new NotImplementedException();
        }

        public void EquipItem (AIAgent aiAgent, ItemInst item)
        {
            throw new NotImplementedException();
        }

        public void UnequipItem (AIAgent aiAgent, ItemInst item)
        {
            throw new NotImplementedException();
        }

        public void DrawWeapon (AIAgent aiAgent, ItemInst item)
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
