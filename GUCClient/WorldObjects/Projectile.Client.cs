using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class Projectile
    {
        partial void pSpawn()
        {
            this.gVob.SetVisual(this.Model.Visual);
        }

        partial void pOnTick(long now)
        {
            long flyTime = now - startTime;

            if (flyTime <= 0)
            {
                this.SetPosition(startPos);
            }
            else
            {
                this.lastPos = this.pos;
                Vec3f curPos = GetTimedPosition(flyTime);

                if (curPos.GetDistance(startPos) >= destination.GetDistance(startPos))
                {
                    curPos = destination;
                }
                this.SetPosition(curPos);
            }
        }
    }
}
