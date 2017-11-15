using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zMat4 : zClass, IDisposable
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

        public void SetUpVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x004B9D90, vec);
        }

        public void SetRightVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x004B9DB0, vec);
        }

        public void GetAtVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x431D20, vec);
        }

        public void GetUpVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x490DD0, vec);
        }

        public void GetRightVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x490DF0, vec);
        }

        public void PostRotateY(float angle)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x00517780, (FloatArg)angle);
        }

        public static zVec3 operator *(zMat4 a, zVec3 v)
        {
            float x = a.Get(0).X * v.X + a.Get(0).Y * v.Y + a.Get(0).Z * v.Z + a.Get(0).W;
            float y = a.Get(1).X * v.X + a.Get(1).Y * v.Y + a.Get(1).Z * v.Z + a.Get(1).W;
            float z = a.Get(2).X * v.X + a.Get(2).Y * v.Y + a.Get(2).Z * v.Z + a.Get(2).W;

            return zVec3.Create(x, y, z);
        }

        public zVec4 Get(int index)
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
                    Process.Write(Address + 4 * index, value);
                }
                else
                {
                    throw new ArgumentException("zMat4 set index is outside of range: " + index);
                }
            }
        }

        // improve me
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

       
        public void GetEulerAngles(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x516390, vec);
        }

        public void SetByEulerAngles(float x, float y, float z)
        {
            using (zVec3 zVec = zVec3.Create(x, y, z))
                SetByEulerAngles(zVec);
        }

        public void SetByEulerAngles(zVec3 angles)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x5163D0, angles);
        }

        public static zMat4 Create()
        {
            int ptr = Process.Alloc(ByteSize).ToInt32();
            return new zMat4(ptr);//Process.THISCALL<zMat4>(ptr, 0x514C10, (FloatArg)0);
        }

        public void Dispose()
        {
            Process.Free(new IntPtr(Address), zMat4.ByteSize);
        }
    }
}
