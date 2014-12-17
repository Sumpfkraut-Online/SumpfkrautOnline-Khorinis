using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class Matrix4 : zClass
    {
        public float[] i;

        public Matrix4(Matrix4 matrix) : base(matrix.Process, matrix.Address)
        {
            i = matrix.i;
        }

        private Matrix4(Process process, int pos) : base(process, pos)
        {

        }

        public float[] getPosition()
        {
            float[] pos = new float[3];

            pos[0] = get(3);
            pos[1] = get(7);
            pos[2] = get(11);

            return pos;
        }

        public float[] getDirection()
        {
            float[] pos = new float[3];

            pos[0] = get(2);
            pos[1] = get(6);
            pos[2] = get(10);

            return pos;
        }

        public void setDirection(float[] dir)
        {
            set(2, dir[0]);
            set(6, dir[1]);
            set(10, dir[2]);
        }

        public void setPosition(float[] dir)
        {
            set(3, dir[0]);
            set(7, dir[1]);
            set(11, dir[2]);
        }

        public float get(int pos)
        {
            return Process.ReadFloat(Address + pos * 4);
        }

        public void set(int pos, float value)
        {
            i[pos] = value;
            Process.Write(value, Address + pos * 4);
        }

        public static Matrix4 read(Process process, int pos)
        {
            float[] ix = new float[16];
            for (int i = 0; i < 16; i++)
            {
                ix[i] = process.ReadInt(pos + i*4);
            }

            Matrix4 mat = new Matrix4(process, pos);
            mat.i = ix;
            return mat;
        }
    }
}
