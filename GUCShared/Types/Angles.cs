using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public partial struct Angles
    {
        public static Angles Null
        {
            get { return new Angles(0, 0, 0); }
        }

        public const float PI = 3.1415926535897931f;
        public static float Deg2Rad(float degree)
        {
            return degree * PI / 180f;
        }

        public static float Rad2Deg(float radian)
        {
            return radian * 180f / PI;
        }
        
        public float Pitch;
        public float Yaw;
        public float Roll;

        public float this[int i]
        {
            get
            {
                if (i == 0) return Pitch;
                else if (i == 1) return Yaw;
                else if (i == 2) return Roll;
                else throw new ArgumentOutOfRangeException("Index is out of range (0..2) <> " + i);
            }
            set
            {
                if (i == 0) Pitch = value;
                else if (i == 1) Yaw = value;
                else if (i == 2) Roll = value;
                else throw new ArgumentOutOfRangeException("Index is out of range (0..2) <> " + i);
            }
        }
        public Angles(float[] data)
        {
            if (data != null)
            {
                if (data.Length >= 3)
                {
                    this.Pitch = data[0];
                    this.Yaw = data[1];
                    this.Roll = data[2];
                    return;
                }
                else if (data.Length == 2)
                {
                    this.Pitch = data[0];
                    this.Yaw = data[1];
                    this.Roll = 0;
                    return;
                }
                else if (data.Length == 1)
                {
                    this.Pitch = data[0];
                    this.Yaw = 0;
                    this.Roll = 0;
                    return;
                }
            }
            this.Roll = 0;
            this.Yaw = 0;
            this.Pitch = 0;
        }

        public Angles(double pitch, double yaw, double roll)
        {
            this.Pitch = (float)pitch;
            this.Yaw = (float)yaw;
            this.Roll = (float)roll;
        }

        public Angles(float pitch, float yaw, float roll)
        {
            this.Pitch = pitch;
            this.Yaw = yaw;
            this.Roll = roll;
        }

        public void Reset()
        {
            this.Pitch = 0;
            this.Yaw = 0;
            this.Roll = 0;
        }

        public void Set(float pitch, float yaw, float roll)
        {
            this.Pitch = pitch;
            this.Yaw = yaw;
            this.Roll = roll;
        }

        #region Direction Vectors

        public void SetByAtVector(Vec3f at)
        {
            at = at.Normalise();
            Pitch = -(float)Math.Acos(at.Y);
            Yaw = (float)Math.Atan2(-at.X, at.Z);
            Roll = 0;
        }

        public Vec3f ToRightVector()
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            return new Vec3f(rollCos * yawCos, rollCos * yawSin, -rollSin);
        }

        public Vec3f ToUpVector()
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            float pitchSin = (float)Math.Sin(Pitch);
            float pitchCos = (float)Math.Cos(Pitch);

            return new Vec3f(pitchSin * rollSin * yawCos - pitchCos * yawSin,
                             pitchSin * rollSin * yawSin + pitchCos * yawCos,
                             pitchSin * rollCos);
        }

        public Vec3f ToAtVector()
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            float pitchSin = (float)Math.Sin(Pitch);
            float pitchCos = (float)Math.Cos(Pitch);

            return new Vec3f(pitchCos * rollSin * yawCos + pitchSin * yawSin,
                             pitchCos * rollSin * yawSin - pitchSin * yawCos,
                             pitchCos * rollCos);
        }

        #endregion

        #region Operators

        public static Angles operator +(Angles a, Angles b)
        {
            return new Angles(a.Pitch + b.Pitch, a.Yaw + b.Yaw, a.Roll + b.Roll);
        }

        public static float operator *(Angles a, Angles b)
        {
            return (a.Roll * b.Roll + a.Yaw * b.Yaw + a.Pitch * b.Pitch);
        }

        public static Angles operator *(float factor, Angles a)
        {
            return new Angles(a.Pitch * factor, a.Yaw * factor, a.Roll * factor);
        }

        public static Angles operator *(Angles a, float factor)
        {
            return factor * a;
        }

        public static Angles operator -(Angles a, Angles b)
        {
            return new Angles(a.Pitch - b.Pitch, a.Yaw - b.Yaw, a.Roll - b.Roll);
        }

        public static Angles operator /(Angles a, float factor)
        {
            return new Angles(a.Pitch / factor, a.Yaw / factor, a.Roll / factor);
        }

        #endregion

        #region Conversion

        public static explicit operator Angles(float[] data)
        {
            return new Angles(data);
        }

        public static explicit operator Angles(float value)
        {
            return new Angles(value, value, value);
        }

        public override string ToString()
        {
            return String.Format("Angles({0} / {1} / {2})", this.Pitch, this.Yaw, this.Roll);
        }

        /// <summary>
        /// Use "rad" for radians and "deg" for degrees.
        /// </summary>
        public string ToString(string radordeg)
        {
            if (string.Equals(radordeg, "deg", StringComparison.OrdinalIgnoreCase)
             || string.Equals(radordeg, "degree", StringComparison.OrdinalIgnoreCase)
             || string.Equals(radordeg, "degrees", StringComparison.OrdinalIgnoreCase)
             || string.Equals(radordeg, "d", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format("Angles({0} / {1} / {2})", Rad2Deg(Pitch), Rad2Deg(Yaw), Rad2Deg(Roll));
            }
            return this.ToString();
        }

        public float[] ToArray()
        {
            return new float[3] { this.Pitch, this.Yaw, this.Roll };
        }

        #endregion

        #region Equality

        const float nullLimit = 0.0000001f;
        public bool IsNull()
        {

            if (this.Roll < nullLimit && this.Roll > -nullLimit &&
                this.Yaw < nullLimit && this.Yaw > -nullLimit &&
                this.Pitch < nullLimit && this.Pitch > -nullLimit)
                return true;
            else
                return false;
        }

        public static bool operator ==(Angles a, Angles b)
        {
            return (a - b).IsNull();
        }

        public static bool operator !=(Angles a, Angles b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return obj is Angles && (Angles)obj == this;
        }

        public override int GetHashCode()
        {
            // FIXME
            return 0;
        }

        #endregion
    }
}
