using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AIStates
{
    public abstract class AIState
    {
        protected bool mStarted = false;
        protected bool mContinues = false;//Continue this state when it is the last?

        protected NPCProto mNPC = null;
        public AIState(NPCProto proto)
        {
            mNPC = proto;
        }


        public abstract void init();
        public abstract void update();

        

        

        public bool isStarted()
        {
            bool started = mStarted;
            mStarted = true;
            return started;
        }


        public virtual bool Continues { get { return mContinues; } }

        public virtual void reset()
        {
            mStarted = false;
        }

        protected virtual void stopState()
        {
            mNPC.standAnim();
            mNPC.getAI().removeFirstState();
        }
    }
}
