using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting;
using GUC.Server.Scripts.AI.Enumeration;

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


        public delegate void RoutineFunction(NPCProto proto);
        public RoutineFunction DailyRoutine = null;
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

        public void update()
        {
            if (mNPC is NPC)
            {
                if (((NPC)mNPC).NPCController == null)
                    setTimer( 10000 * 2000 );
                else
                    setTimer( 10000 * 250  );
            }


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
    }
}
