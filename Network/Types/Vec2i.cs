using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public struct Vec2i
    {
        public static Vec2i Null
        {
            get { return new Vec2i(0, 0); }
        }

        public int X;
        public int Y;

        public Vec2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vec2i(float x, float y)
        {
            this.X = (int)x;
            this.Y = (int)y;
        }

        public Vec2i(int[] data)
        {
            if (data != null)
            {
                if (data.Length >= 2)
                {
                    this.X = data[0];
                    this.Y = data[1];
                    return;
                }
                else if (data.Length == 1)
                {
                    this.X = data[0];
                    this.Y = 0;
                    return;
                }
            }
            this.X = 0;
            this.Y = 0;
        }
        public int this[int i]
        {
            get
            {
                if (i == 0) return X;
                else if (i == 1) return Y;
                else throw new ArgumentOutOfRangeException("Vec2i index is out of range (0..1).");
            }
            set
            {
                if (i == 0) X = value;
                else if (i == 1) Y = value;
                else throw new ArgumentOutOfRangeException("Vec2i index is out of range (0..1).");
            }
        }

        public static explicit operator Vec2i(int[] data)
        {
            return new Vec2i(data);
        }

        public void Reset()
        {
            this.X = 0;
            this.Y = 0;
        }

        public float GetLength()
        {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y);
        }

        public float GetDistance(Vec2i value)
        {
            return (this - value).GetLength();
        }

        #region Operators

        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X + b.X, a.Y + b.Y);
        }

        public static float operator *(Vec2i a, Vec2i b)
        {
            return (a.X * b.X + a.Y * b.Y);
        }

        public static Vec2i operator *(float factor, Vec2i a)
        {
            return new Vec2i(a.X * factor, a.Y * factor);
        }

        public static Vec2i operator *(Vec2i a, float factor)
        {
            return factor * a;
        }

        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X - b.X, a.Y - b.Y);
        }

        #endregion

        #region Equality

        const float nullLimit = 0.0000001f;
        public bool IsNull()
        {

            if (this.X < nullLimit && this.X > -nullLimit &&
                this.Y < nullLimit && this.Y > -nullLimit)
                return true;
            else
                return false;
        }

        public static bool operator ==(Vec2i a, Vec2i b)
        {
            return (a - b).IsNull();
        }

        public static bool operator !=(Vec2i a, Vec2i b)
        {
            return !(a == b);
        }

        #endregion
    }
}
