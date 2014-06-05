using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class FreePoint : FreeOrWayPoint
    {
        public FreePoint(String name, Vec3f position, Vec3f direction)
            : base ( name, position, direction)
        {
            
        }
    }
}
