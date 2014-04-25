using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public class Vec2f
    {
        protected float[] data;

        public Vec2f(float x, float y)
            : this()
        {
            this.set(x, y);
        }

        public Vec2f(float[] data)
            : this()
        {
            this.set(data);
        }

        public Vec2f()
        {
            this.data = new float[] { 0, 0 };
        }

        public void set(float x, float y)
        {
            this.data[0] = x;
            this.data[1] = y;
        }

        public void set(Vec2f vec)
        {
            if (vec == null)
                throw new ArgumentNullException("Paramter vec can't be null!");

            set(vec.X, vec.Y);
        }

        public void set(float[] vec)
        {
            if (vec == null || vec.Length != 2)
                throw new ArgumentException("Paramter vec can't be null and needs a length of 3");

            set(vec[0], vec[1]);
        }

        public float[] Data { get { return this.data; } }

        public float getDistance(Vec2f value)
        {
            return (this - value).Length;
        }

        public float length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float Length { get { return this.length(); } }

        public Vec2f normalise()
        {
            float len = Length;
            return new Vec2f(X * len, Y * len);
        }

        public static Vec2f operator +(Vec2f a, Vec2f b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec2f(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2f operator -(Vec2f a, Vec2f b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec2f(a.X - b.X, a.Y - b.Y);
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


        public static explicit operator Vec2f(float[] data)
        {
            return new Vec2f(data);
        }
    }
}
