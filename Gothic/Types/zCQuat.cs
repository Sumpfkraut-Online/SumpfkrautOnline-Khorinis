using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zCQuat : zClass, IDisposable
    {
        public const int ByteSize = 12;

        public zCQuat()
        {
        }

        public zCQuat(int address) : base(address)
        {
        }

        public static zCQuat Create()
        {
            int ptr = Process.Alloc(ByteSize).ToInt32();
            return new zCQuat(ptr);
        }

        public void Dispose()
        {
            Process.Free(Address, ByteSize);
        }

        public void Matrix4ToQuat(zMat4 matrix)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x518560, matrix);
        }

        public void QuatToMatrix4(zMat4 matrix)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x518360, matrix);
        }

        public void QuatToEuler(zVec3 zVec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x518AC0, zVec);
        }

        public void EulerToQuat(zVec3 zVec)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x518BE0, zVec);
        }
    }
}
