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


        
        // maps VobInst to GuideCmd which is used by the GUC to let clients calculate 
        // movement paths to a destination and guide the vob to it
        protected Dictionary<VobInst, GUC.WorldObjects.VobGuiding.GuideCmd> guideCmdByVobInst =
            new Dictionary<VobInst, WorldObjects.VobGuiding.GuideCmd>() { };

        protected float aggressionRadius;
        public float AggressionRadius { get { return this.aggressionRadius; } }
        public void SetAggressionRadius (float value)
        {
            this.aggressionRadius = value;
        }

        protected float turnAroundVelocity;
        public float TurnAroundVelocity { get { return this.turnAroundVelocity; } }
        public void SetAroundTurnVelocity (float value)
        {
            this.turnAroundVelocity = value;
        }



        public SimpleAIPersonality (float aggressionRadius, float turnAroundVelocity)
        {
            SetObjName("SimpleAIPersonality (default)");
            this.aggressionRadius = aggressionRadius;
            this.turnAroundVelocity = turnAroundVelocity;
        }



        public override void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine)
        {
            this.aiMemory = aiMemory ?? new AIMemory();
            this.aiRoutine = aiRoutine ?? new SimpleAIRoutine();
            this.lastTick = DateTime.Now;
            this.guideCmdByVobInst = new Dictionary<VobInst, WorldObjects.VobGuiding.GuideCmd>() { };
        }



        
        // utility

        public static bool TryFindClosestTarget (VobInst vob, AITarget aiTarget, out VobInst closestTarget)
        {
            closestTarget = null;
            List<VobInst> vobTargets = aiTarget.VobTargets;
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

        public static bool TryFindClosestTarget (VobInst vob, List<VobInst> vobTargets, out VobInst closestTarget)
        {
            closestTarget = null;
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

        public void GoTo (VobInst guided, Vec3f position)
        {
            // initialize new guide and remove old one from GUC-memory automatically
            AI.GuideCommands.GoToPosCommand cmd = new AI.GuideCommands.GoToPosCommand(position);
            guided.BaseInst.SetGuideCommand(cmd);
            // replace possible old guide from script-memory or insert new value
            guideCmdByVobInst[guided] = cmd;
        }

        public void GoTo (VobInst guided, VobInst target)
        {
            // initialize new guide and remove old one from GUC-memory automatically
            AI.GuideCommands.GoToVobCommand cmd = new AI.GuideCommands.GoToVobCommand(target);
            guided.BaseInst.SetGuideCommand(cmd);
            // replace possible old guide from script-memory or insert new value
            guideCmdByVobInst[guided] = cmd;
        }
        
        public void GoTo (AIAgent aiAgent, Vec3f position)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            for (int i = 0; i < aiClients.Count; i++)
            {
                GoTo(aiClients[i], position);
            }
        }

        public void GoTo (AIAgent aiAgent, VobInst target)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            for (int i = 0; i < aiClients.Count; i++)
            {
                GoTo(aiClients[i], target);
            }
        }

        public void GoTo (AIAgent aiAgent, AITarget aiTarget)
        {
            // let each client follow its nearest VobInst from aiTarget respectively
            List<VobInst> followers = aiAgent.AIClients;
            VobInst closestTarget = null;
            for (int f = 0; f < followers.Count; f++)
            {
                if (TryFindClosestTarget(followers[f], aiTarget, out closestTarget))
                {
                    GoTo(followers[f], closestTarget);
                }
            }
        }

        public void Jump (AIAgent aiAgent, int forwardVelocity, int upVelocity)
        {
            throw new NotImplementedException();
        }

        public void ClimbLedge (AIAgent aiAgent, WorldObjects.NPC.ClimbingLedge ledge)
        {
            throw new NotImplementedException();
        }

        public void TurnAround (VobInst guided, Vec3f direction, float angularVelocity)
        {
            guided.BaseInst.SetDirection(direction);
        }

        public void TurnAround (AIAgent aiAgent, Vec3f direction, float angularVelocity)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            for (int c = 0; c < aiClients.Count; c++)
            {
                TurnAround(aiClients[c], direction, angularVelocity);
            }
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

            if (aggressorType == typeof(NPCInst))
            {
                NPCInst aggressorNPC = (NPCInst) aggressor;

                if (aggressorNPC.DrawnWeapon == null)
                {
                    // TODO: draw weapon first if necessary
                }

                if (distance > totalRadius)
                {
                    // approach first to be able to strike at closer distance
                    //Print(aggressor.GetObjName() + " approaches " + target.GetObjName());
                    GoTo(aggressor, target);
                }
                else
                {
                    // close enough to strike target
                    //Print(aggressor.GetObjName() + " attacks " + target.GetObjName());

                    // do not interrupt an ongoing fight ani
                    if (aggressorNPC.GetFightAni() != null) { return; }

                    Visuals.ScriptAniJob scriptAniJob;
                    aggressorNPC.Model.TryGetAniJob((int) Visuals.SetAnis.Attack1HFwd1, out scriptAniJob);
                    if (scriptAniJob != null)
                    {
                        aggressorNPC.StartAnimation(scriptAniJob.BaseAniJob.DefaultAni);
                    }
                }
            }
            else
            { 
                // Do nothing, you lifeless object ! 
            }
        }

        public void Attack (AIAgent aiAgent, AITarget aiTarget)
        {
            List<VobInst> aiClients = aiAgent.AIClients;
            List<VobInst> targets = aiTarget.VobTargets;
            VobInst closestTarget = null;

            if (aiTarget.VobTargets.Count < 1) { return; }

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
            VobInst currVob;
            List<VobInst> enemies = new List<VobInst>();

            for (int c = 0; c < aiClients.Count; c++)
            {
                if (aiClients[c].GetType() == typeof(NPCInst))
                {
                    currVob = aiClients[c];

                    // find all enemies in the radius of aggression
                    currVob.World.BaseWorld.ForEachNPCRough(currVob.BaseInst, aggressionRadius, 
                        delegate (WorldObjects.NPC nearNPC)
                    {
                        if (!aiAgent.HasAIClient(nearNPC))
                        {
                            enemies.Add((VobInst) nearNPC.ScriptObject);
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

            if (aiActions.Count < 1) { return; }

            BaseAIAction aiAction = aiActions[0]; // current action
            AIActions.Enumeration.AiActionType actionType = aiAction.ActionType;

            switch (actionType)
            {
                case AIActions.Enumeration.AiActionType.GoToAIAction:
                    GoTo(aiAgent, aiAction.AITarget);
                    break;
                case AIActions.Enumeration.AiActionType.FollowAIAction:
                    GoTo(aiAgent, aiAction.AITarget);
                    break;
                case AIActions.Enumeration.AiActionType.AttackAIAction:
                    Attack(aiAgent, aiAction.AITarget);
                    break;
            }

            //for (int i = 0; i < aiActions.Count; i++)
            //{
            //    aiAction = aiActions[i];
                // attack if there is a spotted enemy nearby
                //if (aiAction.GetType() == typeof(AttackAIAction))
                //{
                    //// do not control again, if enemy is still in radius
                    //// simply attack the calculated nearest enemy
                    //List<VobInst> aiClients = aiAgent.AIClients;
                    //List<VobInst> enemies = aiAction.AITarget.vobTargets;
                    //NPCInst npc;
                    
                    //if (enemies.Count < 1) { break; }

                    //for (int c = 0; c < aiClients.Count; c++)
                    //{
                    //    // simply hit the enemy by magical means without necessary physical contact :D
                    //    if (aiClients[c].GetType() == typeof(NPCInst))
                    //    {
                    //        npc = (NPCInst) aiClients[c];
                    //        try
                    //        {
                    //            Visuals.ScriptAniJob scriptAniJob;
                    //            npc.Model.TryGetAniJob((int) Visuals.SetAnis.JumpFwd, out scriptAniJob);
                    //            if (scriptAniJob != null)
                    //            {
                    //                if (npc.GetJumpAni() != null) { continue; }

                    //                npc.StartAniJump(scriptAniJob.DefaultAni, 50, 50);
                    //                Print("npc.IsSpawned = " + npc.GetPosition());
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            MakeLogWarning(ex);
                    //        }
                    //    }
                    //}
                //}
            //}
        }

        public override void ProcessObservations (AIAgent aiAgent)
        {
            // do nothing, if not aiClient is defined (shouldn't happen but oh well)
            if (aiAgent.AIClients.Count < 1) { return; }

            List<BaseAIObservation> aiObservations = aiMemory.GetAIObservations();
            List<VobInst> enemies = new List<VobInst>();

            // search all observations for enemies nearby and add them to the list of targets
            for (int i = 0; i < aiObservations.Count; i++)
            {
                if (aiObservations[i].GetType() == typeof(EnemyAIObservation))
                {
                    enemies.AddRange(aiObservations[i].AITarget.VobTargets);
                }
            }

            // reset the observations after retrieving all necessary info
            aiObservations.Clear();

            if (enemies.Count > 0)
            {
                List<BaseAIAction> newAIActions = new List<BaseAIAction> { new AttackAIAction(
                        new AITarget( enemies )) };
                    aiMemory.SetAIActions(newAIActions);
            }
        }

    }

}
