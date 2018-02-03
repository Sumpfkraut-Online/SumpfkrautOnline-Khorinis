using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public struct PosAng
    {
        public Vec3f Position;
        public Angles Angles;

        public PosAng(Vec3f position, Angles angles)
        {
            this.Position = position;
            this.Angles = angles;
        }

        public PosAng(float x, float y, float z, float pitch, float yaw, float roll) : this(new Vec3f(x,y,z), new Angles(pitch, yaw, roll))
        {
        }

        public PosAng(float x, float y, float z, float yaw) : this(x,y,z, 0, yaw, 0)
        {
        }

        public PosAng(float x, float y, float z) : this(x, y, z, 0, 0, 0)
        {
        }
    }
}
