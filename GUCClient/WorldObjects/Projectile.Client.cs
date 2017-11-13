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
            {
                this.SetPosition(startPos);
                this.SetDirection(startDir.Cross(new Vec3f(0, 1, 0)).Normalise());
            }
            else
            {
                Vec3f curPos = GetTimedPosition(flyTime);
                Vec3f curDir = (lastPos - curPos).Normalise();
                

                this.SetPosition(curPos);
                //this.SetDirection(curDir);
                //this.gVob.TrafoObjToWorld.PostRotateY(-90);

                float l = curDir.GetLength();
                float arg = (float)(Math.Atan(curDir.X / curDir.Z) + Math.Asin(curDir.Y / l));

                curDir.X = (float)(l * Math.Sin(arg));
                curDir.Y = 0;
                curDir.Z = (float)(l * Math.Cos(arg));


                this.SetDirection(curDir.Normalise());

                this.lastPos = curPos;
            }
        }
    }
}
