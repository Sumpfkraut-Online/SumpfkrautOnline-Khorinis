using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public static class zCCamera
    {
        public const int activeCam = 0x8D7F94;

        public static int GetCamAddr()
        {
            return Process.ReadInt(activeCam);
        }

        public static void Project(zVec3 pos, out int x, out int y)
        {
            int xAddr = Process.Alloc(8).ToInt32();
            int yAddr = xAddr + 4;

            Process.THISCALL<NullReturnCall>(GetCamAddr(), 0x57A440, pos, (IntArg)xAddr, (IntArg)yAddr);
            x = Process.ReadInt(xAddr);
            y = Process.ReadInt(yAddr);

            Process.Free(new IntPtr(xAddr), 8);
        }

        public static void Project(zVec3 pos, out float x, out float y)
        {
            int xAddr = Process.Alloc(8).ToInt32();
            int yAddr = xAddr + 4;

            Process.THISCALL<NullReturnCall>(GetCamAddr(), 0x530030, pos, (IntArg)xAddr, (IntArg)yAddr);
            x = Process.ReadFloat(xAddr);
            y = Process.ReadFloat(yAddr);

            Process.Free(new IntPtr(xAddr), 8);
        }

        public static zMat4 CamMatrix { get { return new zMat4(GetCamAddr() + 164); } }
    }
}
