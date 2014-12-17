using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class FreeOrWayPoint
    {
        protected String mName;
        protected Vec3f mPosition;
        protected Vec3f mDirection;

        public FreeOrWayPoint(String name, Vec3f position, Vec3f direction)
        {
            mName = name;
            mPosition = position;
            mDirection = direction;
        }

        public bool Equals(String name)
        {
            if (name.ToLower().Trim().Equals(mName.Trim().ToLower()))
                return true;

            return false;
        }

        public Vec3f Position { get { return mPosition; } }
        public Vec3f Direction { get { return mDirection; } }


        public String Name { get { return mName; } }


    }
}
