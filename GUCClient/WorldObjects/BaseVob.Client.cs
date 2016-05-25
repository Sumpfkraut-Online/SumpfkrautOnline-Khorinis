using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using GUC.Types;
using Gothic.Types;
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
        public zCVob gVob { get {return gvob; } }

        #region Position

        public Vec3f GetPosition()
        {
            if (IsSpawned)
            {
                return (Vec3f)gVob.TrafoObjToWorld.Position;
            }
            else
            {
                return pos;
            }
        }

        public void SetPosition(Vec3f pos)
        {
            this.pos = pos;

            if (gvob != null)
            {
                float[] arr = pos.ToArray();
                gVob.TrafoObjToWorld.Position = arr;
                gVob.SetPositionWorld(arr);
                gVob.TrafoObjToWorld.Position = arr;
            }
        }

        public Vec3f GetDirection()
        {
            if (IsSpawned)
            {
                return (Vec3f)gVob.TrafoObjToWorld.Direction;
            }
            else
            {
                return dir;
            }
        }

        public void SetDirection(Vec3f dir)
        {
            this.dir = dir.IsNull() ? new Vec3f(0, 0, 1) : dir;

            if (gvob != null)
            {
                Vec3f zAxis = dir.Normalise();
                Vec3f up = new Vec3f(0.0f, 0.0f, 0.0f);

                if (Math.Abs(zAxis.Y) > 0.5)
                {
                    if (zAxis.Y > 0)
                        up.Z = -1.0f;
                    else
                        up.Z = 1.0f;
                }
                else if (Math.Abs(zAxis.X) < 0.0001 && Math.Abs(zAxis.Y) < 0.0001)
                {
                    if (zAxis.Y > -0.0001)
                    {
                        up.Y = 1.0f;
                    }
                    else
                    {
                        up.Y = -1.0f;
                    }
                }
                else
                {
                    up.Y = 1.0f;
                }

                Vec3f xAxis = up.Cross(zAxis).Normalise();
                Vec3f yAxis = zAxis.Cross(xAxis).Normalise();

                zMat4 trafo = gVob.TrafoObjToWorld;

                trafo[12] = 0;
                trafo[13] = 0;
                trafo[14] = 0;
                trafo[15] = 1;

                trafo[0] = xAxis.X;
                trafo[4] = xAxis.Y;
                trafo[8] = xAxis.Z;

                trafo[1] = yAxis.X;
                trafo[5] = yAxis.Y;
                trafo[9] = yAxis.Z;

                trafo[2] = zAxis.X;
                trafo[6] = zAxis.Y;
                trafo[10] = zAxis.Z;
            }
        }
        #endregion

        /*~BaseVob()
        {
            gvob.refCtr--;
            gvob.Dispose();
        }*/
    }
}
