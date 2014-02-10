using System;
using System.Collections.Generic;
using System.Text;

namespace Network.Types
{
    public class Vec3f
    {
        public float[] data;

        public Vec3f(float x, float y, float z)
        {
            data = new float[] { x, y, z };
        }

        public Vec3f(float[] data)
        {
            this.data = data;
        }

        public Vec3f()
        {
            this.data = new float[] { 0, 0, 0 };
        }

        public float getDistance(Vec3f value)
        {
            return (this - value).Length();
        }

        public Vec3f cross(Vec3f vec2)
        {
            Vec3f rtn = new Vec3f();
            rtn.X = this.Y * vec2.Z - this.Z * vec2.Y;
            rtn.Y = this.Z * vec2.X - this.X * vec2.Z;
            rtn.Z = this.X * vec2.Y - this.Y * vec2.X;
            return rtn;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vec3f normalise()
        {
            float len = Length();
            return new Vec3f(X * len, Y * len, Z * len);
        }

        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec3f(a.X + b.X, a.Y + b.Y, a.Z+b.Z);
        }

        public static float operator *(Vec3f a, Vec3f b)
        {
            if (a == null && b == null)
                return 0;
            if (a == null)
                return 0;
            if (b == null)
                return 0;

            return (a.X * b.X +  a.Y * b.Y + a.Z * b.Z);
        }

        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public float X
        {
            get { return data[0]; }
            set { data[0] = value; }
        }

        public float Y
        {
            get { return data[1]; }
            set { data[1] = value; }
        }

        public float Z
        {
            get { return data[2]; }
            set { data[2] = value; }
        }


        public static explicit operator Vec3f(float[] data)
        {
            return new Vec3f(data);
        }
    }
}
