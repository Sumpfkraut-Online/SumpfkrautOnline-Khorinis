using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs.fightStates
{
    public abstract class FightState
    {
        private NPCProto mNPC = null;
        

        public FightState(NPCProto proto)
        {
            mNPC = proto;
        }


        protected NPCProto NPC { get { return mNPC; } }


        public abstract void update();

        public virtual void stop()
        {
            mNPC.getAI().FightStates.RemoveFirst();
        }
    }
}
