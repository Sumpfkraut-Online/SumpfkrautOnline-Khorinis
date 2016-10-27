using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    /// <summary>
    /// A 3-dimensional float Vector. Y is the up-axis.
    /// </summary>
    public partial struct Vec3f
    {
        public static Vec3f Null
        {
            get { return new Vec3f(0, 0, 0); }
        }

        public float X;
        public float Y;
        public float Z;

        public float this[int i]
        {
            get
            {
                if (i == 0) return X;
                else if (i == 1) return Y;
                else if (i == 2) return Z;
                else throw new ArgumentOutOfRangeException("Vec3f index is out of range (0..2) <> " + i);
            }
            set
            {
                if (i == 0) X = value;
                else if (i == 1) Y = value;
                else if (i == 2) Z = value;
                else throw new ArgumentOutOfRangeException("Vec3f index is out of Vec3f range (0..2) <> " + i);
            }
        }
        public Vec3f(float[] data)
        {
            if (data != null)
            {
                if (data.Length >= 3)
                {
                    this.X = data[0];
                    this.Y = data[1];
                    this.Z = data[2];
                    return;
                }
                else if (data.Length == 2)
                {
                    this.X = data[0];
                    this.Y = data[1];
                    this.Z = 0;
                    return;
                }
                else if (data.Length == 1)
                {
                    this.X = data[0];
                    this.Y = 0;
                    this.Z = 0;
                    return;
                }
            }
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public Vec3f(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vec3f(Vec3f vec)
        {
            this.X = vec.X;
            this.Y = vec.Y;
            this.Z = vec.Z;
        }

        public void Reset()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }

        public void Set(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        #region Math Methods

        public float GetLength()
        {
            //return (float)Math.Sqrt((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z);
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public Vec3f Normalise()
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

        public float GetDistancePlanar(Vec3f value)
        {
            return (float)Math.Sqrt((this.X - value.X) * (this.X - value.X) + (this.Z - value.Z)*(this.Z - value.Z));
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

        public static Vec3f operator /(Vec3f a, float factor)
        {
            return new Vec3f(a.X / factor, a.Y / factor, a.Z / factor);
        }

        #endregion

        #region Conversion

        public static explicit operator Vec3f(float[] data)
        {
            return new Vec3f(data);
        }

        public static explicit operator Vec3f(float value)
        {
            return new Vec3f(value, value, value);
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

        public static bool operator ==(Vec3f a, float b)
        {
            return a.GetLength() == b;
        }

        public static bool operator !=(Vec3f a, float b)
        {
            return a.GetLength() != b;
        }

        public static bool operator ==(Vec3f a, Vec3f b)
        {
            return (a - b).IsNull();
        }

        public static bool operator !=(Vec3f a, Vec3f b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vec3f)
            {
                return this == (Vec3f)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            // FIXME
            return 0;
        }

        #endregion
        
        internal Vec3f CorrectPosition()
        {
            Vec3f ret;
            if (this.X < -838860.8f) ret.X = -838860.8f;
            else if (this.X > 838860.7f) ret.X = 838860.7f;
            else ret.X = this.X;

            if (this.Y < -838860.8f) ret.Y = -838860.8f;
            else if (this.Y > 838860.7f) ret.Y = 838860.7f;
            else ret.Y = this.Y;

            if (this.Z < -838860.8f) ret.Z = -838860.8f;
            else if (this.Z > 838860.7f) ret.Z = 838860.7f;
            else ret.Z = this.Z;

            return ret;
        }

        internal Vec3f CorrectDirection()
        {
            if (this.IsNull())
            {
                return new Vec3f(0, 0, 1);
            }
            return this.Normalise();
        }
    }
}
