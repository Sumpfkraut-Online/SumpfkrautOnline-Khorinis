using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs.fightStates
{
    public class GotoState : FightState
    {
        NPCProto mEnemy = null;
        float mRange = 0.0f;

        public GotoState(NPCProto proto, NPCProto enemy, float range)
            : base( proto)
        {
            mEnemy = enemy;
            mRange = range;
        }

        public override void update()
        {
            if (NPC.gotoPosition(mEnemy.Position, mRange))
            {
                NPC.standAnim();
                stop();
            }
        }
    }
}
