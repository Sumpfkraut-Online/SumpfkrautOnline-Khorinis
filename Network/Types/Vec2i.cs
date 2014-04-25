using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public class Vec2i
    {
        protected int[] data;

        public Vec2i(int x, int y)
            : this()
        {
            this.set(x, y);
        }

        public Vec2i(int[] data)
            : this()
        {
            this.set(data);
        }

        public Vec2i()
        {
            this.data = new int[] { 0, 0 };
        }

        public void set(int x, int y)
        {
            this.data[0] = x;
            this.data[1] = y;
        }

        public void set(Vec2i vec)
        {
            if (vec == null)
                throw new ArgumentNullException("Paramter vec can't be null!");

            set(vec.X, vec.Y);
        }

        public void set(int[] vec)
        {
            if (vec == null || vec.Length != 2)
                throw new ArgumentException("Paramter vec can't be null and needs a length of 3");

            set(vec[0], vec[1]);
        }

        public int[] Data { get { return this.data; } }

        public float getDistance(Vec2i value)
        {
            return (this - value).Length;
        }

        public float length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public float Length { get { return this.length(); } }

        public Vec2i normalise()
        {
            float len = Length;
            return new Vec2i((int)(X * len), (int)(Y * len));
        }

        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec2i(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            if (a == null && b == null)
                return null;
            if (a == null)
                return b;
            if (b == null)
                return a;

            return new Vec2i(a.X - b.X, a.Y - b.Y);
        }

        public int X
        {
            get { return data[0]; }
            set { data[0] = value; }
        }

        public int Y
        {
            get { return data[1]; }
            set { data[1] = value; }
        }


        public static explicit operator Vec2i(int[] data)
        {
            return new Vec2i(data);
        }
    }
}
