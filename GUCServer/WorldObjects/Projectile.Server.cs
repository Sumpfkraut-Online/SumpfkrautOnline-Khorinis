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

        int lifeDistance = ushort.MaxValue;
        public int LifeDistance
        {
            get { return this.lifeDistance; }
            set { this.lifeDistance = value; }
        }

        partial void pOnTick(long now)
        {
            Vec3f curPos = GetTimedPosition(now - startTime);
            if (curPos.GetDistance(startPos) >= lifeDistance)
            {
                this.ScriptObject.OnEndPos(startPos + dir * lifeDistance, dir);
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
