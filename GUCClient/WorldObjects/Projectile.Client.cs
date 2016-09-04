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
                Vec3f curDir = (lastPos - curPos).Cross(new Vec3f(0, 1, 0)).Normalise();

                this.SetPosition(curPos);
                this.SetDirection(curDir);

                this.lastPos = curPos;
            }
        }
    }
}
