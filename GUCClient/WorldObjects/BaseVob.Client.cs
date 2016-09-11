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
        #region Network Messages

        internal static class Messages
        {
            #region Positions
            
            public static void ReadPosDirMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();

                BaseVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    Vec3f newPos = stream.ReadCompressedPosition();
                    Vec3f newDir = stream.ReadCompressedDirection();
                    vob.Interpolate(newPos, newDir);

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

        const float MaxPosDistToInterpolate = 5.0f;
        const float MaxDirDiffToInterpolate = 0.5f;
        protected void Interpolate(Vec3f newPos, Vec3f newDir)
        {
            SetPosition(newPos);
            /*Vec3f curPos = GetPosition();
            if (newPos.GetDistance(curPos) < MaxPosDistToInterpolate)
            {
                InterpolatePos(curPos, newPos);
            }
            else
            {
                SetPosition(newPos);
                iPosEndTime = -1;
            }*/

            Vec3f curDir = GetDirection();
            if (newDir.GetDistance(curDir) < MaxDirDiffToInterpolate)
            {
                InterpolateDir(curDir, newDir);
            }
            else
            {
                SetDirection(newDir);
                iDirEndTime = -1;
            }
        }


        Vec3f iPosStart;
        Vec3f iPosEnd;
        long iPosEndTime = -1;

        Vec3f iDirStart;
        Vec3f iDirEnd;
        long iDirEndTime = -1;

        const long InterpolationTimePos = 400000;
        const long InterpolationTimeDir = 400000;
        void InterpolatePos(Vec3f start, Vec3f end)
        {
            iPosStart = start;
            iPosEnd = end;
            iPosEndTime = GameTime.Ticks + InterpolationTimePos;
        }

        void InterpolateDir(Vec3f start, Vec3f end)
        {
            iDirStart = start;
            iDirEnd = end;
            iDirEndTime = GameTime.Ticks + InterpolationTimeDir;
        }

        void UpdateInterpolation(long now)
        {
            if (iPosEndTime != -1)
            {
                long timeDiff = iPosEndTime - now;
                if (timeDiff < 0)
                {
                    this.SetPosition(iPosEnd);
                    iPosEndTime = -1;
                }
                else
                {
                    this.SetPosition(iPosStart + (iPosEnd - iPosStart) * (float)Math.Pow(1.0 - (double)timeDiff / InterpolationTimeDir, 1 / 3d));
                }
            }

            if (iDirEndTime != -1)
            {
                long timeDiff = iDirEndTime - now;
                if (timeDiff < 0)
                {
                    this.SetDirection(iDirEnd);
                    iDirEndTime = -1;
                }
                else
                {
                    this.SetDirection(iDirStart + (iDirEnd - iDirStart) * (float)Math.Pow(1.0 - (double)timeDiff / InterpolationTimeDir, 1 / 3d));
                }
            }
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

        partial void pOnTick(long now)
        {
            this.UpdateInterpolation(now);
        }
    }
}
