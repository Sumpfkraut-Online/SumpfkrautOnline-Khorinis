using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripts.AI.Waypoints;

namespace GUC.Server.Scripts.AI.AIStates
{
    class AI_GOTOWP : AIState
    {
        protected Waypoints.WayRoute route = null;
        protected Waypoints.WayPoint endWaypoint = null;

        protected String endWP = "";
        protected int wpIndex = 0;


        public AI_GOTOWP(NPC proto, String waypoint)
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
                WayNet wn = mNPC.getWayNet();

                if (wn == null)
                {
                    Log.Logger.log(Log.Logger.LOG_WARNING, "NPC " + mNPC.Name + " | " + mNPC.ID + ": GOTO WP: WayNet was not found: "+mNPC.Map);
                    stopState();
                    return;
                }


                Waypoints.WayPoint startWP = wn.getNearestWaypoint(mNPC.Position);
                endWaypoint = mNPC.getWayNet().getWaypoint(endWP);

                  

                if (startWP == null)
                {
                    Log.Logger.log(Log.Logger.LOG_WARNING, "NPC "+mNPC.Name+" | "+mNPC.ID+": GOTO WP: NO WP near Player was found!");
                    stopState();
                    return;
                }
                else if (endWaypoint == null)
                {
                    Log.Logger.log(Log.Logger.LOG_WARNING, "NPC " + mNPC.Name + " | " + mNPC.ID + ": GOTO WP: End WP was not found: "+endWP+"!");
                    stopState();
                    return;
                }
                if (endWaypoint == startWP)
                {
                    Log.Logger.log(Log.Logger.LOG_INFO, "NPC " + mNPC.Name + " | " + mNPC.ID + ": GOTO WP: Start and END-WP are the same!");
                    stopState();
                    return;
                }

                route = mNPC.getWayNet().getWayRoute(startWP, endWaypoint);

                
                mNPC.getAI().lastPosUpdate = 0;
                if (route == null)//No route found!
                {
                    stopState();
                    return;
                }
            }

            if (mNPC.gotoPosition(route.Waypoints[wpIndex].Position, 100))
            {
                wpIndex += 1;
                

                if (route.Waypoints.Length <= wpIndex)
                {
                    stopState();
                }
            }



        }


        protected override void stopState()
        {
            base.stopState();
            wpIndex = 0;
            route = null;
        }
    }
}
