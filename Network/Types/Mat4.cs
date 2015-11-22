using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public class Mat4
    {
        protected double[][] mData = new double[4][];


        public Mat4()
        {
            for (int i = 0; i < 4; i++)
            {
                mData[i] = new double[4];
            }
        }

        public Mat4(Mat4 mat)
            : this()
        {
            set(mat.mData);
        }

        public Mat4(double[][] values)
            : this()
        {
            
            set(values);
        }

        public Mat4(double[] values)
            : this()
        {
            set(values);
        }

        public Mat4(float[] values)
            : this()
        {
            set(values);
        }

        public void set(double[][] data)
        {
            if (data == null)
                throw new ArgumentNullException("data can't be null!");
            if (data.Length != 4)
                throw new ArgumentException("data has not a size of 4");

            for (int i = 0; i < 4; i++)
            {
                for(int b = 0; b < 4; b++){
                    mData[i][b] = data[i][b];
                }
            }
        }

        public void set(double[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data can't be null!");
            if (data.Length != 16)
                throw new ArgumentException("data has not a size of 16");

            for (int i = 0; i < 4; i++)
            {
                for(int b = 0; b < 4; b++){
                    mData[i][b] = data[i*4+b];
                }
            }
        }

        public void set(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data can't be null!");
            if (data.Length != 16)
                throw new ArgumentException("data has not a size of 16");

            for (int i = 0; i < 4; i++)
            {
                for (int b = 0; b < 4; b++)
                {
                    mData[i][b] = (float)data[i * 4 + b];
                }
            }
        }


        public void setIdentity()
        {
            mData[0][0] = 1.0;
            mData[0][1] = 0.0;
            mData[0][2] = 0.0;
            mData[0][3] = 0.0;

            mData[1][0] = 0.0;
            mData[1][1] = 1.0;
            mData[1][2] = 0.0;
            mData[1][3] = 0.0;

            mData[2][0] = 0.0;
            mData[2][1] = 0.0;
            mData[2][2] = 1.0;
            mData[2][3] = 0.0;

            mData[3][0] = 0.0;
            mData[3][1] = 0.0;
            mData[3][2] = 0.0;
            mData[3][3] = 1.0;
        }

        public static Mat4 Identity
        {
            get { Mat4 mat = new Mat4(); mat.setIdentity(); return mat; }
        }


        public void setOnlyForward(Vec3f forward)
        {
            Vec3f fw = new Vec3f(forward);
            fw.Normalize();

            Vec3f zAxis = forward;
            Vec3f up = new Vec3f(0.0f, 0.0f, 0.0f);

            if (Math.Abs(zAxis.Y) > 0.5)
            {
                if (zAxis.Y > 0)
                    up.Z = -1.0f;
                else
                    up.Z = 1.0f;
            }
            else if (Math.Abs(zAxis.X) < 0.0001 && Math.Abs(zAxis.Y) < 0.0001)
            {
                if (zAxis.Y > -0.0001)
                {
                    up.Y = 1.0f;
                }
                else
                {
                    up.Y = -1.0f;
                }
            }
            else
            {
                up.Y = 1.0f;
            }



            Vec3f xAxis = up.Cross(zAxis).Normalize();
            Vec3f yAxis = zAxis.Cross(xAxis).Normalize();

            mData[2][0] = zAxis.X;
            mData[2][1] = zAxis.Y;
            mData[2][2] = zAxis.Z;

            mData[0][0] = xAxis.X;
            mData[0][1] = xAxis.Y;
            mData[0][2] = xAxis.Z;


            mData[1][0] = yAxis.X;
            mData[1][1] = yAxis.Y;
            mData[1][2] = yAxis.Z;
        }
        

        public Vec3f Forward { get { return new Vec3f((float)mData[2][0], (float)mData[2][1], (float)mData[2][2]); } }
        public Vec3f Right { get { return new Vec3f((float)mData[0][0], (float)mData[0][1], (float)mData[0][2]); } }
        public Vec3f Up { get { return new Vec3f((float)mData[1][0], (float)mData[1][1], (float)mData[1][2]); } }
        public Vec3f Position { get { return new Vec3f((float)mData[3][0], (float)mData[3][1], (float)mData[3][2]); } }


        public double this[int key]
        {
            get { 
                int k = key / 4;
                int k2 = key - (k*4);
                return mData[k][k2]; 
            }
            set
            {
                int k = key / 4;
                int k2 = key - (k*4);

                mData[k][k2] = value;
            }
        }

        

        public static Mat4 operator *(Mat4 a, Mat4 b)
        {
            if (a == null)
                return null;
            if (b == null)
                return null;

            Mat4 newMat = new Mat4();

            newMat[0] = b.mData[0][0] * a.mData[0][0] +
                b.mData[0][1] * a.mData[1][0] +
                b.mData[0][2] * a.mData[2][0] +
                b.mData[0][3] * a.mData[3][0];

            newMat[1] = b.mData[0][0] * a.mData[0][1] +
                b.mData[0][1] * a.mData[1][1] +
                b.mData[0][2] * a.mData[2][1] +
                b.mData[0][3] * a.mData[3][1];

            newMat[2] = b.mData[0][0] * a.mData[0][2] +
                b.mData[0][1] * a.mData[1][2] +
                b.mData[0][2] * a.mData[2][2] +
                b.mData[0][3] * a.mData[3][2];

            newMat[3] = b.mData[0][0] * a.mData[0][3] +
                b.mData[0][1] * a.mData[1][3] +
                b.mData[0][2] * a.mData[2][3] +
                b.mData[0][3] * a.mData[3][3];


            newMat[4] = b.mData[1][0] * a.mData[0][0] +
                b.mData[1][1] * a.mData[1][0] +
                b.mData[1][2] * a.mData[2][0] +
                b.mData[1][3] * a.mData[3][0];

            newMat[5] = b.mData[1][0] * a.mData[0][1] +
                b.mData[1][1] * a.mData[1][1] +
                b.mData[1][2] * a.mData[2][1] +
                b.mData[1][3] * a.mData[3][1];

            newMat[6] = b.mData[1][0] * a.mData[0][2] +
                b.mData[1][1] * a.mData[1][2] +
                b.mData[1][2] * a.mData[2][2] +
                b.mData[1][3] * a.mData[3][2];

            newMat[7] = b.mData[1][0] * a.mData[0][3] +
                b.mData[1][1] * a.mData[1][3] +
                b.mData[1][2] * a.mData[2][3] +
                b.mData[1][3] * a.mData[3][3];


            newMat[8] = b.mData[2][0] * a.mData[0][0] +
                b.mData[2][1] * a.mData[1][0] +
                b.mData[2][2] * a.mData[2][0] +
                b.mData[2][3] * a.mData[3][0];

            newMat[9] = b.mData[2][0] * a.mData[0][1] +
                b.mData[2][1] * a.mData[1][1] +
                b.mData[2][2] * a.mData[2][1] +
                b.mData[2][3] * a.mData[3][1];

            newMat[10] = b.mData[2][0] * a.mData[0][2] +
                b.mData[2][1] * a.mData[1][2] +
                b.mData[2][2] * a.mData[2][2] +
                b.mData[2][3] * a.mData[3][2];

            newMat[11] = b.mData[2][0] * a.mData[0][3] +
                b.mData[2][1] * a.mData[1][3] +
                b.mData[2][2] * a.mData[2][3] +
                b.mData[2][3] * a.mData[3][3];


            newMat[12] = b.mData[3][0] * a.mData[0][0] +
                b.mData[3][1] * a.mData[1][0] +
                b.mData[3][2] * a.mData[2][0] +
                b.mData[3][3] * a.mData[3][0];

            newMat[13] = b.mData[3][0] * a.mData[0][1] +
                b.mData[3][1] * a.mData[1][1] +
                b.mData[3][2] * a.mData[2][1] +
                b.mData[3][3] * a.mData[3][1];

            newMat[14] = b.mData[3][0] * a.mData[0][2] +
                b.mData[3][1] * a.mData[1][2] +
                b.mData[3][2] * a.mData[2][2] +
                b.mData[3][3] * a.mData[3][2];

            newMat[15] = b.mData[3][0] * a.mData[0][3] +
                b.mData[3][1] * a.mData[1][3] +
                b.mData[3][2] * a.mData[2][3] +
                b.mData[3][3] * a.mData[3][3];

            return newMat;
        }

        public static Vec4f operator *(Mat4 a, Vec4f b)
        {
            if (a == null)
                return null;
            if (b == null)
                return null;

            Vec4f newVec = new Vec4f();

            newVec.Data[0] = (float)(a.mData[0][0] * b.Data[0] +
                a.mData[0][1] * b.Data[1]+
                a.mData[0][2] * b.Data[2]+
                a.mData[0][3] * b.Data[3]);

            newVec.Data[1] = (float)(a.mData[1][0] * b.Data[0] +
                a.mData[1][1] * b.Data[1] +
                a.mData[1][2] * b.Data[2] +
                a.mData[1][3] * b.Data[3]);

            newVec.Data[2] = (float)(a.mData[2][0] * b.Data[0] +
                a.mData[2][1] * b.Data[1] +
                a.mData[2][2] * b.Data[2] +
                a.mData[2][3] * b.Data[3]);

            newVec.Data[3] = (float)(a.mData[3][0] * b.Data[0] +
                a.mData[3][1] * b.Data[1] +
                a.mData[3][2] * b.Data[2] +
                a.mData[3][3] * b.Data[3]);

            return newVec;
        }


    public void RotateX(double angle)
    {
        double s = Math.Sin(angle);
        double c = Math.Cos(angle);

        double a10 = mData[1][0], a11 = mData[1][1], a12 = mData[1][2], a13 = mData[1][3];
        double a20 = mData[2][0], a21 = mData[2][1], a22 = mData[2][2], a23 = mData[2][3];

        mData[1][0] = a10 * c + a20 * s;
        mData[1][1] = a11 * c + a21 * s;
        mData[1][2] = a12 * c + a22 * s;
        mData[1][3] = a13 * c + a23 * s;
        
        mData[2][0] = a10 * -s + a20 * c;
        mData[2][1] = a11 * -s + a21 * c;
        mData[2][2] = a12 * -s + a22 * c;
        mData[2][3] = a13 * -s + a23 * c;
    }

    public void RotateY(double angle)
    {
        double s = Math.Sin(angle);
        double c = Math.Cos(angle);
        
        double a00 = mData[0][0], a01 = mData[0][1], a02 = mData[0][2], a03 = mData[0][3];
        double a20 = mData[2][0], a21 = mData[2][1], a22 = mData[2][2], a23 = mData[2][3];
        
        mData[0][0] = a00 * c + a20 * -s;
        mData[0][1] = a01 * c + a21 * -s;
        mData[0][2] = a02 * c + a22 * -s;
        mData[0][3] = a03 * c + a23 * -s;
        
        mData[2][0] = a00 * s + a20 * c;
        mData[2][1] = a01 * s + a21 * c;
        mData[2][2] = a02 * s + a22 * c;
        mData[2][3] = a03 * s + a23 * c;
    }
    
    public void RotateZ(double angle)
    {
        double s = Math.Sin(angle);
        double c = Math.Cos(angle);
        
        double a00 = mData[0][0], a01 = mData[0][1], a02 = mData[0][2], a03 = mData[0][3];
        double a10 = mData[1][0], a11 = mData[1][1], a12 = mData[1][2], a13 = mData[1][3];
        
        mData[0][0] = a00 * c + a10 * s;
        mData[0][1] = a01 * c + a11 * s;
        mData[0][2] = a02 * c + a12 * s;
        mData[0][3] = a03 * c + a13 * s;
        
        mData[1][0] = a00 * -s + a10 * c;
        mData[1][1] = a01 * -s + a11 * c;
        mData[1][2] = a02 * -s + a12 * c;
        mData[1][3] = a03 * -s + a13 * c;
    }

    }
}
    