using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public struct Vec3f
    {
        public static Vec3f Null = new Vec3f(0,0,0);

        public float X;
        public float Y;
        public float Z;

        public Vec3f(float[] data)
        {
            if (data.Length >= 3)
            {
                this.X = data[0];
                this.Y = data[1];
                this.Z = data[2];
            }
            else if (data.Length == 2)
            {
                this.X = data[0];
                this.Y = data[1];
                this.Z = 0;
            }
            else if (data.Length == 1)
            {
                this.X = data[0];
                this.Y = 0;
                this.Z = 0;
            }
            else
            {
                this.X = 0;
                this.Y = 0;
                this.Z = 0;
            }
        }

        public Vec3f(Vec3f vec)
        {
            this = vec;
        }

        public Vec3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void Reset()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        #region Math Methods

        public float GetLength()
        {
            return (float)Math.Sqrt((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z);
        }

        public Vec3f Normalize()
        {
            float len = GetLength();
            if (len != 0)
            {
                return new Vec3f(this.X / len, this.Y / len, this.Z / len);
            }
            return this;
        }

        public float GetDistance(Vec3f value)
        {
            return (this - value).GetLength();
        }

        public Vec3f Cross(Vec3f vec)
        {
            return new Vec3f(this.Y * vec.Z - this.Z * vec.Y,
                             this.Z * vec.X - this.X * vec.Z,
                             this.X * vec.Y - this.Y * vec.X);
        }

        #endregion

        #region Operators

        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static float operator *(Vec3f a, Vec3f b)
        {
            return (a.X * b.X + a.Y * b.Y + a.Z * b.Z);
        }

        public static Vec3f operator *(float factor, Vec3f a)
        {
            return new Vec3f(a.X * factor, a.Y * factor, a.Z * factor);
        }

        public static Vec3f operator *(Vec3f a, float factor)
        {
            return factor * a;
        }

        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        #endregion

        #region Conversion

        public static explicit operator Vec3f(float[] data)
        {
            return new Vec3f(data);
        }

        public override string ToString()
        {
            return String.Format("Vec3f({0} / {1} / {2})", this.X, this.Y, this.Z);
        }

        public float[] ToArray()
        {
            return new float[3] { this.X, this.Y, this.Z };
        }

        #endregion

        #region Equality

        const float nullLimit = 0.0000001f;
        public bool IsNull()
        {

            if (this.X < nullLimit && this.X > -nullLimit &&
                this.Y < nullLimit && this.Y > -nullLimit &&
                this.Z < nullLimit && this.Z > -nullLimit)
                return true;
            else
                return false;
        }

        public static bool operator ==(Vec3f a, Vec3f b)
        {
            return (a - b).IsNull();
        }

        public static bool operator !=(Vec3f a, Vec3f b)
        {
            return !(a == b);
        }

        #endregion
    }
}
