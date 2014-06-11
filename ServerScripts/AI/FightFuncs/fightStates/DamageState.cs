using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs.fightStates
{
    public class DamageState : FightState
    {
        NPCProto mEnemy = null;
        public DamageState(NPCProto npc, NPCProto enemy)
            : base(npc)
        {
            mEnemy = enemy;
        }


        public override void update()
        {
            NPC.hitEnemy(mEnemy);
            stop();
        }
    }
}
