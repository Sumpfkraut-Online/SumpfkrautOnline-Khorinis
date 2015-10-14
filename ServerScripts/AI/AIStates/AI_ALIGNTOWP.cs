using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.AI.Waypoints;

namespace GUC.Server.Scripts.AI.AIStates
{
    class AI_ALIGNTOWP : AIState
    {
        protected Waypoints.WayPoint endWaypoint = null;

        protected String endWP = "";


        public AI_ALIGNTOWP(NPC proto, String waypoint)
            : base(proto)
        {
            endWP = waypoint;
        }

        public override void init()
        {
            
        }

        public override void update()
        {
            if (!this.isStarted())
            {
                endWaypoint = mNPC.getWayNet().getWaypoint(endWP);
            }

            if (mNPC.turnTo(endWaypoint.Direction))
            {
                stopState();
            }



        }
    }
}
