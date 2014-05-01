using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;
using GUC.Server.Scripts.AI.Enumeration;
using GUC.Server.Scripts.AI.AssessFuncs;
using GUC.Server.Scripts.AI.DataTypes;
using GUC.Server.Scripts.AI.FightFuncs;

namespace GUC.Server.Scripts.AI
{
    public class NPCAI
    {
        protected Timer mAITimer = null;
        protected List<AIStates.AIState> mStates = new List<AIStates.AIState>();
        protected AIStates.AIState mLastState = null;

        protected bool mInterrupted = false;

        protected NPCProto mNPC;


        protected Waypoints.WayPoint lastWP;


        //These thre variables are only for RTN_ACTIVE()
        public int lastRTNDay = -1;
        public int lastRTNHour = -1;
        public int lastRTNMinute = -1;



        public Dictionary<NPCProto, NPCProto> TargetList = new Dictionary<NPCProto, NPCProto>();
        public LinkedList<NPCProto> EnemyList = new LinkedList<NPCProto>();

        public AI_Events.RoutineFunction DailyRoutine = null;
        public AI_Events.FightRoutine FightRoutine = null;


        public AI_Events.AssessDamageFunction AssessDamageRoutine = null;
        public AI_Events.AssessDamageFunction AssessOtherDamageRoutine = null;
        public AI_Events.AssessTargetFunction AssessTargetRoutine = null;
        public AI_Events.AssessTargetFunction AssessEnemyRoutine = null;
        public AI_Events.AssessTargetFunction AssessBodyRoutine = null;
        public AI_Events.UpdateRoutine UpdateRoutine = null;


        public Guilds Guild = Guilds.HUM_NONE;

        public bool inDialog = false;

        /// <summary>
        /// Only for gotoPosition func.
        /// </summary>
        public long lastPosUpdate = 0;

        
        public WalkTypes WalkType = WalkTypes.Run;


        public NPCAI(NPCProto npc)
        {
            mAITimer = new Timer(10000*250);
            mAITimer.OnTick += new Events.TimerEvent(update);
            
            mNPC = npc;

            AssessTargetRoutine = new AI_Events.AssessTargetFunction(AssessTarget.OnAssessTarget);
            AssessEnemyRoutine = new AI_Events.AssessTargetFunction(AssessEnemy.OnAssessEnemy);

            
            UpdateRoutine = new AI_Events.UpdateRoutine(Update.OnUpdate);
            FightRoutine = new AI_Events.FightRoutine(MonsterFightRoutine.FightRoutine);
        }


        public void init()
        {
            mAITimer.Start();
        }

        public void setTimer(long time)
        {
            mAITimer.TimeSpan = time;
        }

        public void addState(AIStates.AIState state)
        {
            mStates.Add(state);
        }

        public void clearStateList()
        {
            mStates.Clear();
        }

        public void removeFirstState()
        {
            mStates.RemoveAt(0);
        }

        public void updateTargetList()
        {
            NPCProto[] newTargetList = this.mNPC.getNearNPC(4000);
            Dictionary<NPCProto, NPCProto> nTL = new Dictionary<NPCProto, NPCProto>();

            foreach (NPCProto proto in newTargetList)
            {
                if (proto == mNPC)
                    continue;
                nTL.Add(proto, proto);

                if (TargetList.ContainsKey(proto))
                    continue;

                if (AssessTargetRoutine != null)
                    AssessTargetRoutine(mNPC, proto);
            }

            TargetList = nTL;
            
            
        }

        public void update()
        {
            if (mNPC is NPC)
            {
                if (((NPC)mNPC).NPCController == null)
                    setTimer( 10000 * 2000 );
                else
                    setTimer( 10000 * 250  );
            }

            if (mNPC.HP == 0)
            {
                EnemyList.Clear();
                return;
            }

            updateTargetList();

            if (EnemyList.Count != 0 && FightRoutine != null)
                FightRoutine(mNPC);



            if (DailyRoutine != null && !inDialog)
                DailyRoutine(this.mNPC);



            if (this.Interrupted)
                return;

            if (mNPC.isDead || mNPC.isUnconscious)
                return;
            

            if (mStates.Count == 0 && mLastState != null && mLastState.Continues)
            {
                mLastState.reset();
                mStates.Add(mLastState);
            }

            if (mStates.Count == 0)
                return;
            mStates[0].update();
        }

        /// <summary>
        /// AI_States won't be called when Interrupted is true.
        /// </summary>
        public bool Interrupted {
            get { return mInterrupted; }
            set { mInterrupted = value; }
        }


        public void addEnemy(NPCProto proto)
        {
            if (EnemyList.Contains(proto))
                return;
            EnemyList.AddLast(proto);
        }

        

        public void removeEnemy(NPCProto proto)
        {
            EnemyList.Remove(proto);
        }
    }
}
