using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class WayRoute
    {
        protected WayPoint[] waypoints = null;


        public WayRoute(WayPoint[] wList)
        {
            waypoints = wList;
        }

        public WayPoint[] Waypoints { get { return waypoints; } }
    }
}
