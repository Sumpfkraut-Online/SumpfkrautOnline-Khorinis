using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class Projectile
    {
        #region ScriptObject

        public partial interface IScriptProjectile : IScriptBaseVob
        {
            void UpdatePos(Vec3f newPos, Vec3f oldPos);
            void OnEndPos(Vec3f pos, Vec3f dir);
        }

        #endregion

        Vec3f destination;
        public Vec3f Destination { set { this.destination = value; } get { return this.destination; } }

        partial void pOnTick(long now)
        {
            Vec3f curPos = GetTimedPosition(now - startTime);
            if (curPos.GetDistance(startPos) >= destination.GetDistance(startPos))
            {
                this.ScriptObject.OnEndPos(destination, startDir);
                this.ScriptObject.Despawn();
            }
            else
            {
                this.ScriptObject.UpdatePos(curPos, lastPos);
                this.lastPos = curPos;
            }
        }
    }
}
