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

        public const float PI    = 3.1415926535897931f;
        public const float TwoPI = 6.2831853071795865f;

        public static float Deg2Rad(float degrees)
        {
            return degrees * PI / 180f;
        }

        public static float Rad2Deg(float radians)
        {
            return radians * 180f / PI;
        }

        /// <summary> Clamps to 0, 360 </summary>
        public static float ClampTo360(float degrees)
        {
            degrees %= 360;
            if (degrees < 0)
                degrees += 360;
            return degrees;
        }

        /// <summary> Clamps to 0, pi </summary>
        public static float ClampTo2PI(float radians)
        {
            radians %= TwoPI;
            if (radians < 0)
                radians += TwoPI;
            return radians;
        }

        /// <summary> Clamps to -180, +180 </summary>
        public static float ClampTo180(float degrees)
        {
            degrees = (degrees + 180) % 360;
            if (degrees < 0)
                degrees += 360;
            return degrees - 180;
        }

        /// <summary> Clamps to [-pi, +pi] </summary>
        public static float ClampToPI(float radians)
        {
            radians = (radians + PI) % (2 * PI);
            if (radians < 0)
                radians += 2 * PI;
            return radians - PI;
        }

        /// <summary> Clamps all three angles to [-pi, +pi]</summary>
        public Angles Clamp()
        {
            return new Angles(ClampToPI(Pitch), ClampToPI(Yaw), ClampToPI(Roll));
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

        public static float GetYawFromAtVector(Vec3f at)
        {
            at = at.Normalise();
            return (float)Math.Atan2(-at.X, at.Z);
        }

        public static Angles FromAtVector(Vec3f at)
        {
            var angles = new Angles();
            angles.SetByAtVector(at);
            return angles;
        }

        /// <summary> Sets the Pitch and Yaw angles by a heading-at vector. Roll is 0. </summary>
        public void SetByAtVector(Vec3f at)
        {
            at = at.Normalise();
            Pitch = -(float)Math.Acos(at.Y) + PI / 2; // +PI/2 so the default value is 0 and not -90
            Pitch = ClampToPI(Pitch);

            Yaw = (float)Math.Atan2(-at.X, at.Z);
            Roll = 0;
        }

        public Vec3f ToRightVector()
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            return new Vec3f(rollCos * yawCos, -rollSin, rollCos * yawSin);
        }

        public Vec3f ToUpVector()
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            float pitchSin = -(float)Math.Cos(Pitch);// (float)Math.Sin(Pitch - PI/2); // -PI/2 so the default value can be 0 and not -90
            float pitchCos = (float)Math.Sin(Pitch); // (float)Math.Cos(Pitch - PI/2);

            return new Vec3f(pitchSin * rollSin * yawCos - pitchCos * yawSin,
                             pitchSin * rollCos,
                             pitchSin * rollSin * yawSin + pitchCos * yawCos);
        }

        public Vec3f ToAtVector()
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            float pitchSin = -(float)Math.Cos(Pitch);// (float)Math.Sin(Pitch - PI/2); // -PI/2 so the default value can be 0 and not -90
            float pitchCos = (float)Math.Sin(Pitch); // (float)Math.Cos(Pitch - PI/2);

            return new Vec3f(pitchCos * rollSin * yawCos + pitchSin * yawSin,
                             pitchCos * rollCos,
                             pitchCos * rollSin * yawSin - pitchSin * yawCos);
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

        /// <summary>
        /// Converts signed two byte integer to [-pi, pi] float.
        /// </summary>
        public static float Short2Angle(short value)
        {
            return value * PI / short.MaxValue;
        }

        /// <summary>
        /// Converts [-pi, pi] float to signed two byte integer.
        /// </summary>
        public static short Angle2Short(float angle)
        {
            return (short)(angle * short.MaxValue / PI);
        }

        // for npc updates
        public bool DifferenceIsBigger(Angles angles2, float maxDiff)
        {
            if (Math.Abs(Difference(Yaw, angles2.Yaw)) > maxDiff)
                return true;
            else if (Math.Abs(Difference(Pitch, angles2.Pitch)) > maxDiff)
                return true;
            else if (Math.Abs(Difference(Roll, angles2.Roll)) > maxDiff)
                return true;
            return false;
        }

        /// <summary> Returns the signed difference on a [-pi, pi] scale between two angles. </summary>
        public static float Difference(float angle1, float angle2)
        {
            float a = (angle1 - angle2) % TwoPI;
            if (a < 0) a += TwoPI;
            if (a > PI) a -= TwoPI;
            return a;

        }

        /// <summary> Returns the signed difference on a [-pi, pi] scale between two angles. </summary>
        public static Angles Difference(Angles angles1, Angles angles2)
        {
            return new Angles()
            {
                Pitch = Difference(angles1.Pitch, angles2.Pitch),
                Yaw = Difference(angles1.Yaw, angles2.Yaw),
                Roll = Difference(angles1.Roll, angles2.Roll)
            };
        }
    }
}
