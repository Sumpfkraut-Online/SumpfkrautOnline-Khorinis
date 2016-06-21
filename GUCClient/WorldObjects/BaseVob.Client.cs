using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects
{
    public abstract partial class BaseVob
    {
        #region ScriptObject

        /// <summary>
        /// The underlying ScriptObject interface for all Vobs.
        /// </summary>
        public partial interface IScriptBaseVob : IScriptGameObject
        {
            void OnReadScriptVobMsg(PacketReader stream);
        }

        #endregion

        internal zCVob gvob;
        public zCVob gVob { get { return gvob; } }

        #region Position & Direction

        partial void pGetPosition()
        {
            if (this.gvob != null)
            {
                this.pos = (Vec3f)this.gvob.TrafoObjToWorld.Position;
            }
        }

        partial void pGetDirection()
        {
            if (this.gvob != null)
            {
                this.dir = (Vec3f)this.gvob.TrafoObjToWorld.Direction;
            }
        }

        partial void pSetPosition()
        {
            if (this.gvob != null)
            {
                this.gvob.TrafoObjToWorld.Position = this.pos.ToArray();
                this.gvob.SetPositionWorld(this.pos.X, this.pos.Y, this.pos.Z);
            }
        }

        partial void pSetDirection()
        {
            if (this.gvob != null)
            {
                this.gvob.SetHeadingAtWorld(this.dir.X, this.dir.Y, this.dir.Z);
            }
        }

        #endregion
    }
}
