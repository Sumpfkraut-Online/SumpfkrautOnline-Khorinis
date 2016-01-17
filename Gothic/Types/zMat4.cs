using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zMat4 : zClass
    {
        public const int ByteSize = 64;

        public zMat4(int address)
            : base(address)
        {
        }

        public zMat4()
        {
        }

        public void SetAtVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x0056B960, vec);
        }


        public static zVec3 operator *(zMat4 a, zVec3 v)
        {
            float x = a.get(0).X * v.X + a.get(0).Y * v.Y + a.get(0).Z * v.Z + a.get(0).W;
            float y = a.get(1).X * v.X + a.get(1).Y * v.Y + a.get(1).Z * v.Z + a.get(1).W;
            float z = a.get(2).X * v.X + a.get(2).Y * v.Y + a.get(2).Z * v.Z + a.get(2).W;

            return zVec3.Create(x, y, z);
        }

        public zVec4 get(int index)
        {
            if (index > 3)
                throw new ArgumentException("the index can not be larger than 3.");
            return new zVec4(this.Address + index * 16);
        }

        public float this[int index]
        {
            get
            {
                if (index >= 0 && index < 16)
                {
                    return Process.ReadFloat(Address + 4 * index);
                }
                throw new ArgumentException("zMat4 get index is outside of range: " + index);
            }
            set
            {
                if (index >= 0 && index < 16)
                {
                    Process.Write(value, Address + 4 * index);
                }
                throw new ArgumentException("zMat4 set index is outside of range: " + index);
            }
        }

        public float[] Position
        {
            get
            {
                float[] pos = new float[3];

                pos[0] = this[3];
                pos[1] = this[7];
                pos[2] = this[11];

                return pos;
            }
            set
            {
                if (value != null && value.Length == 3)
                {
                    this[3] = value[0];
                    this[7] = value[1];
                    this[11] = value[2];
                }
                else
                {
                    throw new ArgumentException("zMat4 set position array is null or has wrong length!");
                }
            }
        }

        public float[] Direction
        {
            get
            {
                float[] pos = new float[3];

                pos[0] = this[2];
                pos[1] = this[6];
                pos[2] = this[10];

                return pos;
            }
            set
            {
                if (value != null && value.Length == 3)
                {
                    this[2] = value[0];
                    this[6] = value[1];
                    this[10] = value[2];
                }
                else
                {
                    throw new ArgumentException("zMat4 set direction array is null or has wrong length!");
                }
            }
        }
    }
}
