using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class WayPoint : FreeOrWayPoint
    {
        protected List<WayPoint> connectedWP = new List<WayPoint>();
        protected String[] mConnectedWPString = null;

        public WayPoint(String name, Vec3f position, Vec3f direction, String[] wpList)
            : base( name, position, direction)
        {
            for (int i = 0; i < wpList.Length; i++ )
            {
                wpList[i] = wpList[i].Trim().ToLower();
            }
            mConnectedWPString = wpList;
        }


        

        
        public WayPoint[] ConnectedWP { get { return connectedWP.ToArray(); }
            set {
                connectedWP.Clear();
                connectedWP.AddRange(value);
            }
        }
        public String[] ConnectedWPString { get { return mConnectedWPString; } }
    }
}
