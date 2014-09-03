using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zTypes
{
    public class zMat4 : zClass
    {
        public zMat4(Process process, int address)
            : base(process, address)
        {
            
        }

        public void SetAtVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x0056B960, new CallValue[] { vec });
        }


        public static zVec3 operator *(zMat4 a, zVec3 v)
        {
            float x = a.get(0).get(0) * v.get(0) + a.get(0).get(1) * v.get(1) + a.get(0).get(2) * v.get(2) + a.get(0).get(3);
            float y = a.get(1).get(0) * v.get(0) + a.get(1).get(1) * v.get(1) + a.get(1).get(2) * v.get(2) + a.get(1).get(3);
            float z = a.get(2).get(0) * v.get(0) + a.get(2).get(1) * v.get(1) + a.get(2).get(2) * v.get(2) + a.get(2).get(3);

            zVec3 nV = zVec3.Create(a.Process);
            nV.X = x;
            nV.Y = y;
            nV.Z = z;

            return nV;
        }

        public zVec4 get(int index)
        {
            if (index > 3)
                throw new ArgumentException("the index can not be larger than 3.");
            return new zVec4(Process, this.Address + index * 16);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public override int SizeOf()
        {
            return 64;
        }
    }
}
