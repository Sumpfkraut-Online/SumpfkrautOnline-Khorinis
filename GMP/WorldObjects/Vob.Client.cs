using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Collections;
using Gothic.Objects;
using GUC.Types;
using Gothic.Types;

namespace GUC.WorldObjects
{
    public partial class Vob : WorldObject, IVobObj<uint>
    {
        public zCVob gVob { get; protected set; }

        #region Position
        public Vec3f Position
        {
            get
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
            set
            {
                pos = value;

                if (IsSpawned)
                {
                    float[] arr = pos.ToArray();
                    gVob.TrafoObjToWorld.Direction = arr;
                    gVob.SetPositionWorld(arr);
                    gVob.TrafoObjToWorld.Position = arr;
                }
            }
        }

        public Vec3f Direction
        {
            get
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
            set
            {
                dir = value.IsNull() ? new Vec3f(0, 0, 1) : value;

                if (IsSpawned)
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
        }
        #endregion

        protected override void pCreate()
        {
            this.gVob = this.Instance.CreateVob();
            base.pCreate();
        }
    }
}
