using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs.fightStates
{
    public class WaitState : FightState
    {
        public long time = 0;
        public long timespan = 0;
        public WaitState(NPCProto npc, long time)
            : base(npc)
        {
            timespan = time;
        }

        public override void update()
        {
            long now = DateTime.Now.Ticks;
            if (time == 0)
            {
                time = now + timespan;
                return;
            }

            if (time < now)
            {
                stop();
            }

        }
    }
}
