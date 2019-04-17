using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class GUCBaseVobInst
    {
        #region Network Messages

        internal static class Messages
        {
            #region Positions

            public static void ReadPosDirMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();
                if (World.Current.TryGetVob(id, out GUCBaseVobInst vob))
                {
                    Vec3f newPos = stream.ReadCompressedPosition();
                    Angles newAng = stream.ReadCompressedAngles();
                    vob.Interpolate(newPos, newAng);

                    //vob.ScriptObject.OnPosChanged();
                }
                else
                {
                    VobGuiding.TargetCmd.CheckPos(id, stream.ReadCompressedPosition());
                }
            }

            #endregion
        }

        #endregion

        #region Position & Direction Interpolation

        protected virtual void Interpolate(Vec3f newPos, Angles newAng)
        {
            SetPosition(newPos);
            SetAngles(newAng);
        }

        #endregion

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

        /// <summary> Re-calculates the Environment property. (Is called once every frame by default) </summary>
        public virtual void UpdateEnvironment()
        {
            this.environment = CalculateEnvironment(10);
        }

        protected VobEnvironment CalculateEnvironment(float groundDistToFly)
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

            return new VobEnvironment(inAir, waterStand, waterDepth);
        }

        #endregion

        #endregion

        #region Position & Direction

        /// <summary> Updates the position & angles property. (Is called once every frame by default) </summary>
        public virtual void UpdateOrientation()
        {
            if (this.gvob != null)
            {
                var trafo = this.gVob.TrafoObjToWorld;
                this.pos.X = Vec3f.ClampToWorldLimits(trafo[3]);
                this.pos.Y = Vec3f.ClampToWorldLimits(trafo[7]);
                this.pos.Z = Vec3f.ClampToWorldLimits(trafo[11]);

                this.ang = new Angles(trafo);
            }

        }

        partial void pSetPosition()
        {
            // Sets the position of the gothic vob
            if (this.gvob != null)
            {
                this.gvob.TrafoObjToWorld.Position = this.pos.ToArray();
                this.gvob.SetPositionWorld(pos.X, pos.Y, pos.Z);
            }
        }

        partial void pSetAngles()
        {
            // Sets the rotation matrix of the gothic vob
            if (this.gvob != null)
            {
                ang.SetMatrix(gvob);
            }
        }

        #endregion

        #region Spawn & Despawn

        partial void pBeforeSpawn(World world, Vec3f position, Angles angles)
        {
            // let the instance create the gothic object
            CreateGVob();

            // set position & orientation
            pSetPosition();
            pSetAngles();
        }

        partial void pAfterDespawn()
        {
            DeleteGVob();
        }

        #endregion

        #region Creation & Deletion of the Gothic-Object

        internal virtual void CreateGVob()
        {
            this.gvob = this.instance.CreateVob();
        }

        internal virtual void DeleteGVob()
        {
            // we are finished with this gothic object, decrease the reference counter
            int refCtr = gvob.refCtr - 1;
            gvob.refCtr = refCtr;

            // Free the gothic object if no references are left, otherwise gothic should free it
            if (refCtr <= 0)
                gvob.Dispose();

            gvob = null;
        }

        #endregion

        partial void pOnTick(long now)
        {
            UpdateEnvironment();
            UpdateOrientation();
        }
    }
}
