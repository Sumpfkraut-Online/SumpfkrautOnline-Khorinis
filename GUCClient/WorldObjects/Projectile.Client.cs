using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class Projectile
    {
        partial void pOnTick(long now)
        {
            long flyTime = now - startTime;
            if (flyTime <= 0)
                return;

            Vec3f curPos = GetTimedPosition(flyTime);
            Vec3f curDir = (lastPos - curPos).Cross(new Vec3f(0, 1, 0)).Normalise();
            
            float[] arr = curPos.ToArray();
            this.gVob.TrafoObjToWorld.Position = arr;
            this.gVob.SetPositionWorld(arr);
            this.gVob.TrafoObjToWorld.Position = arr;
            
            gvob.SetHeadingAtWorld(curDir.X, curDir.Y, curDir.Z);

            this.lastPos = curPos;
        }
    }
}
