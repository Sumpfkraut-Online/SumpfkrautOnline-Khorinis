using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using Gothic.Objects;

namespace GUC.Types
{
    public partial struct Angles
    {
        /// <summary> Sets the trafo of the given vob </summary>
        public void SetMatrix(zCVob vob)
        {
            SetMatrix(vob.TrafoObjToWorld);
            /*bool movement = vob.IsInMovement;
            if (!movement) vob.BeginMovement();

            var obj = vob.CollObj;
            SetMatrix(obj.NewTrafo);
            obj.TrafoHintRotation = true;

            if (!movement) vob.EndMovement();*/
        }

        public void SetMatrix(zMat4 matrix)
        {
            float rollSin = (float)Math.Sin(Roll);
            float rollCos = (float)Math.Cos(Roll);

            float yawSin = (float)Math.Sin(Yaw);
            float yawCos = (float)Math.Cos(Yaw);

            float pitchSin = -(float)Math.Cos(Pitch);// (float)Math.Sin(Pitch - PI/2); // -PI/2 so the default value can be 0 and not -90
            float pitchCos = (float)Math.Sin(Pitch); // (float)Math.Cos(Pitch - PI/2);

            matrix[0] = rollCos * yawCos;
            matrix[8] = rollCos * yawSin;
            matrix[4] = -rollSin;

            matrix[1] = -pitchSin * rollSin * yawCos + pitchCos * yawSin;
            matrix[9] = -pitchSin * rollSin * yawSin - pitchCos * yawCos;
            matrix[5] = -pitchSin * rollCos;

            matrix[2] = pitchCos * rollSin * yawCos + pitchSin * yawSin;
            matrix[10] = pitchCos * rollSin * yawSin - pitchSin * yawCos;
            matrix[6] = pitchCos * rollCos;
        }

        public static explicit operator Angles(zMat4 matrix)
        {
            return new Angles(matrix);
        }

        public static Angles FromMatrix(zMat4 matrix)
        {
            return new Angles(matrix);
        }

        public Angles(zMat4 matrix)
        {
            Vec3f right = new Vec3f(matrix[0], matrix[8], matrix[4]).Normalise();
            Vec3f up = new Vec3f(matrix[1], matrix[9], matrix[5]).Normalise();
            Vec3f at = new Vec3f(matrix[2], matrix[10], matrix[6]).Normalise();

            Pitch = -(float)Math.Atan2(up.Z, at.Z) + PI / 2; // +PI/2 so the default value is 0 and not -90
            Pitch = ClampToPI(Pitch);

            Yaw = (float)Math.Atan2(right.Y, right.X);
            Roll = -(float)Math.Asin(right.Z);

            /*const float oneminusepsilon = 0.99999f;
            if (right.Z < -oneminusepsilon)
            { // singularity
                Roll = PI / 2f;
                Yaw = 0;
                Pitch = (float)Math.Atan2(up.X, at.X);
            }
            else if (right.Z > oneminusepsilon)
            { // singularity
                Roll = -PI / 2f;
                Yaw = 0;
                Pitch = (float)Math.Atan2(-up.X, -at.X);
            }
            else
            {
                Roll = -(float)Math.Asin(right.Z);
                float c = (float)Math.Cos(Roll);

                Yaw = (float)Math.Atan2(right.Y / c, right.X / c);
                Pitch = -(float)Math.Atan2(up.Z / c, at.Z / c) + PI/2; // +PI/2 so the default is 0 and not 90
            }*/
        }

        /*            
            // right vector
            double R11 = matrix[0];
            double R21 = matrix[8];
            double R31 = matrix[4];
            Log.Logger.Log("Right: " + R11 + " " + R21 + " " + R31);

            // up vector
            double R12 = matrix[1];
            double R22 = matrix[9];
            double R32 = matrix[5];
            Log.Logger.Log("Up: " + R12 + " " + R22 + " " + R32);

            // at vector
            double R13 = matrix[2];
            double R23 = matrix[10];
            double R33 = matrix[6];
            Log.Logger.Log("At: " + R13 + " " + R23 + " " + R33);

            if (Math.Abs(R31 - 1) < 0.001f)
            {
                Log.Logger.LogWarning("singularity");
                //return;
            }

            double theta = -Math.Asin(R31); // pitch
            double c = Math.Cos(theta);

            double phi = Math.Atan2(R21 / c, R11 / c); // yaw

            double psi = Math.Atan2(R32 / c, R33 / c); // roll

            using (var vec = Gothic.Types.zVec3.Create())
            {
                matrix.GetEulerAngles(vec);
                Log.Logger.Log(vec.X + " " + vec.Y + " " + vec.Z);
            }
            Log.Logger.Log(theta + " " + phi + " " + psi);
            
            phi += x * Math.PI / 180;
            theta += y * Math.PI / 180;
            psi += z * Math.PI / 180;

            R11 = Math.Cos(theta) * Math.Cos(phi);
            R21 = Math.Cos(theta) * Math.Sin(phi);
            R31 = -Math.Sin(theta);
            Log.Logger.Log("Right: " + R11 + " " + R21 + " " + R31);

            R12 = Math.Sin(psi) * Math.Sin(theta) * Math.Cos(phi) - Math.Cos(psi) * Math.Sin(phi);
            R22 = Math.Sin(psi) * Math.Sin(theta) * Math.Sin(phi) + Math.Cos(psi) * Math.Cos(phi);
            R32 = Math.Sin(psi) * Math.Cos(theta);
            Log.Logger.Log("Up: " + R12 + " " + R22 + " " + R32);

            R13 = Math.Cos(psi) * Math.Sin(theta) * Math.Cos(phi) + Math.Sin(psi) * Math.Sin(phi);
            R23 = Math.Cos(psi) * Math.Sin(theta) * Math.Sin(phi) - Math.Sin(psi) * Math.Cos(phi);
            R33 = Math.Cos(psi) * Math.Cos(theta);
            Log.Logger.Log("At: " + R13 + " " + R23 + " " + R33);

            var a = arrow.TrafoObjToWorld;
            a[0] = (float)R11;
            a[8] = (float)R21;
            a[4] = (float)R31;

            a[1] = (float)R12;
            a[9] = (float)R22;
            a[5] = (float)R32;

            a[2] = (float)R13;
            a[10] = (float)R23;
            a[6] = (float)R33;
        */
    }
}
