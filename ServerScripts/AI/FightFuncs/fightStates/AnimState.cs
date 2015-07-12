using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.FightFuncs.fightStates
{
    public class AnimState : FightState
    {
        protected String mAnim = "";

        public AnimState(NPC proto, String animation)
            : base( proto )
        {
            mAnim = animation;
        }

        public override void update()
        {
            NPC.playAnimation(mAnim);
            stop();
        }
    }
}
