﻿using System;
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
        }

        #endregion

        #region Properties

        zCVob gvob;
        /// <summary> The correlated gothic-object of this vob. </summary>
        public zCVob gVob { get { return gvob; } }

        #region Environment

        partial void pGetEnvironment()
        {
            this.environment = CalculateEnvironment(10);
        }

        protected Environment CalculateEnvironment(float groundDistToFly)
        {
            var collObj = gVob.CollObj;
            float waterLevel = collObj.WaterLevel;
            float groundLevel = collObj.GroundLevel;
            if (Math.Abs(waterLevel - groundLevel) < 0.01f)
                waterLevel = float.MinValue;

            var bbox = gVob.BBox3D;
            float top = bbox.Max.Y;
            float bottom = bbox.Min.Y;
            float height = top - bottom;

            bool inAir = (gVob.BitField1 & zCVob.BitFlag0.physicsEnabled) != 0 || bottom - groundLevel > groundDistToFly;

            float waterStand = waterLevel - bottom;
            if (waterStand <= 0) waterStand = 0;
            else if (waterStand >= height) waterStand = 1;
            else waterStand /= height;

            float waterDepth = waterLevel - groundLevel;
            if (waterDepth <= 0) waterDepth = 0;
            else if (waterDepth >= height) waterDepth = 1;
            else waterDepth /= height;

            return new Environment(inAir, waterStand, waterDepth);
        }

        #endregion

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
            CreateGVob();

            // set position & orientation
            pSetPosition();
            pSetDirection();
        }

        partial void pAfterDespawn()
        {
            DeleteGVob();
        }

        #endregion

        #region Creation & Deletion of the Gothic-Object

        internal void CreateGVob()
        {
            this.gvob = this.instance.CreateVob();
        }

        internal void DeleteGVob()
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
