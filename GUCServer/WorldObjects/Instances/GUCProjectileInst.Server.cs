using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.WorldObjects
{
    public partial class GUCProjectileInst
    {
        #region ScriptObject

        public partial interface IScriptProjectile : IScriptBaseVob
        {
            void UpdatePos();
            void OnEndPos();
        }

        #endregion

        GUCItemInst item;
        public GUCItemInst Item
        {
            get { return this.item; }
            set
            {
                if (this.IsSpawned)
                    throw new NotSupportedException();
                this.item = value;
            }
        }

        partial void pOnTick(long now)
        {
            this.lastPos = pos;
            this.pos = GetTimedPosition(now - startTime).ClampToWorldLimits();

            if (pos.GetDistance(startPos) >= destination.GetDistance(startPos))
            {
                // projectile reached its destination
                this.ScriptObject.OnEndPos();
                this.ScriptObject.Despawn();
            }
            else
            {
                // Update networking
                this.world.UpdateVobCell(this, pos);

                if (lastUpdatePos.GetDistancePlanar(this.pos) > 100)
                {
                    lastUpdatePos = this.pos;
                    CleanClientList();
                    UpdateClientList();
                }

                // tell server scripts
                this.ScriptObject.UpdatePos();
            }
        }
    }
}
