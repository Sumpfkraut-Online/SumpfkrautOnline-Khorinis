using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations;
using GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.AI.GuideCommands;
using GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs;
using GUC.Scripts.Sumpfkraut.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities
{

    public class SimpleAIPersonality : BaseAIPersonality
    {
        public List<SimpleAIPersonality> Group = new List<SimpleAIPersonality>();


        // maps VobInst to GuideCmd which is used by the GUC to let clients calculate 
        // movement paths to a destination and guide the vob to it
        protected Dictionary<VobInst, GuideCommandInfo> guideCommandByVobInst;

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
            this.aggressionRadius = aggressionRadius;
            this.turnAroundVelocity = turnAroundVelocity;
        }



        public override void Init (AIMemory aiMemory, BaseAIRoutine aiRoutine)
        {
            this.aiMemory = aiMemory ?? new AIMemory();
            this.aiRoutine = aiRoutine ?? new SimpleAIRoutine();
            this.lastTick = DateTime.Now;
            this.guideCommandByVobInst = new Dictionary<VobInst, GuideCommandInfo>();
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
                if (Cast.Try(vobTargets[t], out NPCInst npc) && (npc.IsDead || npc.IsUnconscious))
                    continue;

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
                if (Cast.Try(vobTargets[t], out NPCInst npc) && (npc.IsDead || npc.IsUnconscious))
                    continue;

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

        protected void SubscribeGuideCommand (GuideCommandInfo guideCmdInfo)
        {
            guideCommandByVobInst[guideCmdInfo.GuidedVobInst] = guideCmdInfo;
        }

        protected void UnsubscribeGuideCommand (GuideCommandInfo guideCmdInfo)
        {
            guideCommandByVobInst.Remove(guideCmdInfo.GuidedVobInst);
        }

        protected void UnsubscribeGuideCommand (VobInst guided)
        {
            guideCommandByVobInst.Remove(guided);
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
            // find out if there already is an existing, similar guide-command 
            // and recycle it if possible before assigning a new one (costly on client-side)
            if (guideCommandByVobInst.TryGetValue(guided, out GuideCommandInfo oldInfo))
            {
                if (oldInfo.GuideCommand.CmdType == (byte)CommandType.GoToPos)
                {
                    if (((GoToPosCommand)oldInfo.GuideCommand).Destination.Equals(position))
                    {
                        oldInfo.UpdateInfo(oldInfo.GuideCommand, oldInfo.GuidedVobInst, DateTime.MaxValue);
                        return;
                    }
                }
            }

            // initialize new guide and remove old one from GUC-memory automatically
            GoToPosCommand cmd = new GoToPosCommand(position);
            guided.BaseInst.SetGuideCommand(cmd);
            // replace possible old guide from script-memory or insert new value
            GuideCommandInfo info = new GuideCommandInfo(cmd, guided);
            SubscribeGuideCommand(info);
        }

        public void GoTo (VobInst guided, VobInst target)
        {
            // find out if there already is an existing, similar guide-command 
            // and recycle it if possible before assigning a new one (costly on client-side)
            if (guideCommandByVobInst.TryGetValue(guided, out GuideCommandInfo oldInfo))
            {
                if (oldInfo.GuideCommand.CmdType == (byte)CommandType.GoToVob)
                {
                    if (((GoToVobCommand)oldInfo.GuideCommand).Target.Equals(target))
                    {
                        oldInfo.UpdateInfo(oldInfo.GuideCommand, oldInfo.GuidedVobInst, DateTime.MaxValue);
                        return;
                    }
                }
            }

            // initialize new guide and remove old one from GUC-memory automatically
            GoToVobCommand cmd = new GoToVobCommand(target);
            guided.BaseInst.SetGuideCommand(cmd);
            
            // replace possible old guide from script-memory or insert new value
            GuideCommandInfo info = new GuideCommandInfo(cmd, guided);
            SubscribeGuideCommand(info);
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
            //guided.BaseInst.SetDirection(direction);
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
            if (aggressor is NPCInst aggressorNPC)
            {
                if (!aggressorNPC.IsInFightMode && !aggressorNPC.ModelInst.IsInAnimation())
                {
                    ItemInst weapon;
                    if ((weapon = aggressorNPC.GetEquipmentBySlot(NPCSlots.OneHanded1)) != null
                     || (weapon = aggressorNPC.GetEquipmentBySlot(NPCSlots.TwoHanded)) != null)
                    {
                        aggressorNPC.EffectHandler.TryDrawWeapon(weapon);
                    }
                    else
                    {
                        aggressorNPC.EffectHandler.TryDrawFists();
                    }
                }

                float fightRange = target.ModelDef.Radius + aggressorNPC.GetFightRange();

                var cmd = aggressorNPC.BaseInst.CurrentCommand;
                if (cmd == null || !(cmd is GoToVobLookAtCommand) || ((GoToVobLookAtCommand)cmd).Target != target)
                {
                    aggressorNPC.BaseInst.SetGuideCommand(new GoToVobLookAtCommand(target, fightRange - 20f)); // -20 cm for safety
                }

                float distance = aggressorNPC.GetPosition().GetDistance(target.GetPosition());
                if (distance < fightRange)
                {
                    aggressorNPC.EffectHandler.TryFightMove(FightMoves.Fwd);
                }
                else if (distance < fightRange + 100f)
                {
                    aggressorNPC.EffectHandler.TryFightMove(FightMoves.Run);
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



        /// <summary>
        /// Force active observation of the environment. 
        /// Can be run anytime to let the aiClients recognize their surrounding actively.
        /// </summary>
        /// <param name="aiAgent"></param>
        public override void MakeActiveObservation (AIAgent aiAgent)
        {
            var aiClients = aiAgent.AIClients;
            VobInst currVob;
            var enemies = new List<VobInst>();

            for (int c = 0; c < aiClients.Count; c++)
            {
                if (aiClients[c].GetType() == typeof(NPCInst))
                {
                    currVob = aiClients[c];

                    // find all enemies in the radius of aggression
                    currVob.World.BaseWorld.ForEachNPCRough(currVob.BaseInst, aggressionRadius, 
                        delegate (WorldObjects.NPC nearNPC)
                    {
                        var npc = (NPCInst)nearNPC.ScriptObject;

                        if (nearNPC.IsPlayer && !nearNPC.IsDead && !npc.IsUnconscious)
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

        /// <summary>
        /// Pass action on which the ai decided upon to respective subroutines.
        /// </summary>
        /// <param name="aiAgent"></param>
        public override void ProcessActions (AIAgent aiAgent)
        {
            var aiActions = aiMemory.GetAIActions();
            if (aiActions.Count < 1) { return; }

            var aiAction = aiActions[0]; // current action
            var ts = new TypeSwitch()
                .Case((GoToAIAction a) => GoTo(aiAgent, a.AITarget))
                .Case((FollowAIAction a) => GoTo(aiAgent, a.AITarget))
                .Case((AttackAIAction a) => Attack(aiAgent, a.AITarget));
        }


        /// <summary>
        /// Create AIAction- from AIObservation-objects. This is where the ai makes decisions.
        /// </summary>
        /// <param name="aiAgent"></param>
        public override void ProcessObservations (AIAgent aiAgent)
        {
            // do nothing, if not aiClient is defined (shouldn't happen but oh well)
            if (aiAgent.AIClients.Count < 1) { return; }

            var aiObservations = aiMemory.GetAIObservations();
            var enemies = new List<VobInst>();

            // search all observations for enemies nearby and add them to the list of targets
            for (int i = 0; i < aiObservations.Count; i++)
            {
                if (aiObservations[i].GetType() == typeof(EnemyAIObservation))
                {
                    // only add those enemies which aren't already there
                    enemies = enemies.Union(aiObservations[i].AITarget.VobTargets).ToList();
                }
            }

            // reset the observations after retrieving all necessary info
            aiObservations.Clear();

            if (enemies.Count > 0)
            {
                var newAIActions = new List<BaseAIAction> { new AttackAIAction(
                        new AITarget( enemies )) };
                    aiMemory.SetAIActions(newAIActions);
            }
        }

    }

}
