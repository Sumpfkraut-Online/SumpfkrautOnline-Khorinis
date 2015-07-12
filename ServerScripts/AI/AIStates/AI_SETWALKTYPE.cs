using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripts.AI.Enumeration;

namespace GUC.Server.Scripts.AI.AIStates
{
    class AI_SETWALKTYPE : AIState
    {
        protected Waypoints.WayPoint endWaypoint = null;

        protected WalkTypes walkType = WalkTypes.Run;


        public AI_SETWALKTYPE(NPC proto, WalkTypes walkType)
            : base(proto)
        {
            this.walkType = walkType;
        }

        public override void init()
        {
            
        }

        public override void update()
        {
            mNPC.getAI().WalkType = this.walkType;
            stopState();
        }
    }
}
