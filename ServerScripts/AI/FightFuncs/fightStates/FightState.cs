using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs.fightStates
{
    public abstract class FightState
    {
        private NPC mNPC = null;
        

        public FightState(NPC proto)
        {
            mNPC = proto;
        }


        protected NPC NPC { get { return mNPC; } }


        public abstract void update();

        public virtual void stop()
        {
            mNPC.getAI().FightStates.RemoveFirst();
        }
    }
}
