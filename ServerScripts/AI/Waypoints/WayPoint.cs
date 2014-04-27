using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class WayPoint
    {
        protected String mName;
        protected Vec3f mPosition;
        protected Vec3f mDirection;

        protected List<WayPoint> connectedWP = new List<WayPoint>();
        protected String[] mConnectedWPString = null;

        public WayPoint(String name, Vec3f position, Vec3f direction, String[] wpList)
        {
            mName = name;
            mPosition = position;
            mDirection = direction;
            

            for (int i = 0; i < wpList.Length; i++ )
            {
                wpList[i] = wpList[i].Trim().ToLower();
            }
            mConnectedWPString = wpList;
        }

        public bool Equals(String name)
        {
            if ( name.ToLower().Trim().Equals( mName.Trim().ToLower() ) )
                return true;

            return false;
        }


        public Vec3f Position { get { return mPosition; } }
        public Vec3f Direction { get { return mDirection; } }


        public String Name { get { return mName; } }

        
        public WayPoint[] ConnectedWP { get { return connectedWP.ToArray(); }
            set {
                connectedWP.Clear();
                connectedWP.AddRange(value);
            }
        }
        public String[] ConnectedWPString { get { return mConnectedWPString; } }
    }
}
