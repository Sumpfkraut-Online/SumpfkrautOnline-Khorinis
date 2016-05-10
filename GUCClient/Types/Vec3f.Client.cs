using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;

namespace GUC.Types
{
    public partial struct Vec3f
    {

        public Vec3f(zVec3 zVec)
        {
            this.X = zVec.X;
            this.Y = zVec.Y;
            this.Z = zVec.Z;
        }


        public static explicit operator Vec3f(zVec3 zVec)
        {
            return new Vec3f(zVec);
        }
    }
}
