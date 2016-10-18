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

        #region Properties
        
        protected zCVob gvob;
        /// <summary> The correlated gothic-object of this vob. </summary>
        public zCVob gVob { get { return gvob; } }

        #endregion

        #region Position & Direction

        partial void pGetPosition()
        {
            // Updates the position from the correlating gothic-vob's position.
            if (this.gvob != null)
            {   
                this.pos = ((Vec3f)this.gvob.TrafoObjToWorld.Position).CorrectPosition();
            }
        }

        partial void pGetDirection()
        {
            // Updates the direction from the correlating gothic-vob's direction.
            if (this.gvob != null)
            {
                this.dir = ((Vec3f)this.gvob.TrafoObjToWorld.Direction).CorrectDirection();
            }
        }

        partial void pSetPosition()
        {
            // Sets the position of the gothic vob
            if (this.gvob != null)
            {
                this.gvob.TrafoObjToWorld.Position = this.pos.ToArray();
                this.gvob.SetPositionWorld(this.pos.X, this.pos.Y, this.pos.Z);
            }
        }

        partial void pSetDirection()
        {
            // Sets the direction of the gothic vob
            if (this.gvob != null)
            {
                this.gvob.SetHeadingAtWorld(this.dir.X, this.dir.Y, this.dir.Z);
            }
        }

        #endregion

        #region Spawn & Despawn

        partial void pBeforeSpawn(World world, Vec3f position, Vec3f direction)
        {
            // let the instance create the gothic object
            this.gvob = this.instance.CreateVob();

            // set position & orientation
            pSetPosition();
            pSetDirection();
        }

        partial void pAfterDespawn()
        {
            // we are finished with this gothic object, decrease the reference counter
            int refCtr = gvob.refCtr - 1;
            gvob.refCtr = refCtr;

            // Free the gothic object if no references are left, otherwise gothic will free it
            if (refCtr <= 0)
                gvob.Dispose();

            gvob = null;
        }

        #endregion
    }
}
