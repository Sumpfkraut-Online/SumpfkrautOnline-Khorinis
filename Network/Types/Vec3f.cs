using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public class Vec3f
    {
        protected float[] data;

        public Vec3f(float x, float y, float z)
            : this()
        {
            this.set(x, y, z);
        }

        public Vec3f(float[] data)
            : this()
        {
            this.set(data);
        }

        public Vec3f(Vec3f data)
            : this()
        {
            this.set(data);
        }

        public Vec3f()
        {
            this.data = new float[] { 0, 0, 0 };
        }

        public void set(float x, float y, float z)
        {
            this.data[0] = x;
            this.data[1] = y;
            this.data[2] = z;
        }

        public void set(Vec3f vec)
        {
            if (vec == null)
                throw new ArgumentNullException("Paramter vec can't be null!");

            set(vec.X, vec.Y, vec.Z);
        }

        public void set(float[] vec)
        {
            if (vec == null || vec.Length != 3)
                throw new ArgumentException("Paramter vec can't be null and needs a length of 3");

            set(vec[0], vec[1], vec[2]);
        }

        public float[] Data { get { return this.data; } }

        public float getDistance(Vec3f value)
        {
            return (this - value).Length;
        }

        public bool isNull()
        {
            if (data[0] - 0.000001f < 0 && data[0] + 0.000001f > 0 &&
                data[1] - 0.000001f < 0 && data[1] + 0.000001f > 0 &&
                data[2] - 0.000001f < 0 && data[2] + 0.000001f > 0)
                return true;
            else
                return false;
        }

        public Vec3f cross(Vec3f vec2)
        {
            Vec3f rtn = new Vec3f();
            rtn.X = this.Y * vec2.Z - this.Z * vec2.Y;
            rtn.Y = this.Z * vec2.X - this.X * vec2.Z;
            rtn.Z = this.X * vec2.Y - this.Y * vec2.X;
            return rtn;
        }

        public float length()
        {
            return (float)Math.Sqrt((double)X * (double)X + (double)Y * (double)Y + (double)Z * (double)Z);
        }

        public float Length { get { return this.length(); } }

        public Vec3f normalise()
        {
            float len = Length;
            return new Vec3f(X / len, Y / len, Z / len);
        }

        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static float operator *(Vec3f a, Vec3f b)
        {
            if (a == null && b == null)
                return 0;
            if (a == null)
                return 0;
            if (b == null)
                return 0;

            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

        public static Vec3f operator *(Vec3f a, float factor)
        {
            if (a == null)
                return a;
            if (a == null)
                return a;

            return new Vec3f(a.X * factor, a.Y * factor, a.Z * factor);
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

        public bool Equal(Vec3f b)
        {
            if (this == b)
                return true;

            if ((this - b).isNull())
                return true;

            return false;
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

        public override string ToString()
        {
            return " Vec3f("+X+", "+Y+", "+Z+") ";
        }
    }
}
