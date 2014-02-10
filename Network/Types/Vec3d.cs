using System;
using System.Collections.Generic;
using System.Text;

namespace Network.Types
{
    public class Vec3d
    {
        public double[] data;

        public Vec3d(double x, double y, double z)
        {
            data = new double[] { x, y, z };
        }

        public Vec3d(float x, float y, float z)
        {
            data = new double[] { x, y, z };
        }

        public Vec3d(float[] data)
        {
            this.data = new double[3]{data[0], data[1], data[2]};
        }

        public Vec3d(double[] data)
        {
            this.data = data;
        }

        public Vec3d()
        {
            this.data = new double[] { 0, 0, 0 };
        }

        public double getDistance(Vec3d value)
        {
            return (this - value).Length();
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vec3d cross(Vec3d vec2)
        {
            Vec3d rtn = new Vec3d();
            rtn.X = this.Y * vec2.Z - this.Z * vec2.Y;
            rtn.Y = this.Z * vec2.X - this.X * vec2.Z;
            rtn.Z = this.X * vec2.Y - this.Y * vec2.X;
            return rtn;
        }

        public Vec3d normalise()
        {
            float len = Length();
            return new Vec3d(X * len, Y * len, Z * len);
        }

        public static Vec3d operator +(Vec3d a, Vec3d b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3d operator -(Vec3d a, Vec3d b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public double X
        {
            get { return data[0]; }
            set { data[0] = value; }
        }

        public double Y
        {
            get { return data[1]; }
            set { data[1] = value; }
        }

        public double Z
        {
            get { return data[2]; }
            set { data[2] = value; }
        }


        public static explicit operator Vec3d(float[] data)
        {
            return new Vec3d(data);
        }

        public static explicit operator Vec3d(double[] data)
        {
            return new Vec3d(data);
        }
    }
}
