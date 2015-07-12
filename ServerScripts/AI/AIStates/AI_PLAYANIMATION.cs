using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AIStates
{
    class AI_PLAYANIMATION : AIState
    {
        protected Waypoints.WayPoint endWaypoint = null;

        protected String animation = "";


        public AI_PLAYANIMATION(NPC proto, String anim)
            : base(proto)
        {
            animation = anim;
        }

        public override void init()
        {
            
        }

        public override void update()
        {
            mNPC.clearAnimation();
            mNPC.playAnimation(animation);
            stopState();
        }
    }
}
