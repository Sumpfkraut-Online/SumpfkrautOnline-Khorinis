using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.View;

namespace Gothic.Objects
{
    public class zCCamera : zClass, IDisposable
    {
        public const int activeCam = 0x8D7F94;

        public static zCCamera ActiveCamera
        {
            get { return new zCCamera(Process.ReadInt(activeCam)); }
            set { Process.Write(activeCam, value.Address); }
        }

        public void Project(zVec3 pos, out int x, out int y)
        {
            int xAddr = Process.Alloc(8).ToInt32();
            int yAddr = xAddr + 4;

            Process.THISCALL<NullReturnCall>(Address, 0x57A440, pos, (IntArg)xAddr, (IntArg)yAddr);
            x = Process.ReadInt(xAddr);
            y = Process.ReadInt(yAddr);

            Process.Free(new IntPtr(xAddr), 8);
        }

        public void Project(zVec3 pos, out float x, out float y)
        {
            int xAddr = Process.Alloc(8).ToInt32();
            int yAddr = xAddr + 4;

            Process.THISCALL<NullReturnCall>(Address, 0x530030, pos, (IntArg)xAddr, (IntArg)yAddr);
            x = Process.ReadFloat(xAddr);
            y = Process.ReadFloat(yAddr);

            Process.Free(new IntPtr(xAddr), 8);
        }

        public zMat4 CamMatrix { get { return new zMat4(Address + 164); } }

        public float FarClipZ { get { return Process.ReadFloat(Address + 2300); } }
        public void SetFarClipZ(float value)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x54B200, (FloatArg)value);
        }

        public void SetRenderTarget(zCView viewBase)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x54ABD0, viewBase);
        }

        public void SetFOV(float x, float y)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x54A960, (FloatArg)x, (FloatArg)y);
        }

        public zCCamera()
        {

        }

        public zCCamera(int address)
            : base(address)
        {

        }
        
        public const int ByteSize = 0x934;
        public static zCCamera Create()
        {
            int ptr = Process.Alloc(ByteSize).ToInt32();
            Process.THISCALL<NullReturnCall>(ptr, 0x549E60);
            return new zCCamera(ptr);
        }
        
        bool disposed = false;
        public void Dispose()
        {
            if (!this.disposed)
            {
                Process.THISCALL<NullReturnCall>(Address, 0x0054A290);
                Process.Free(new IntPtr(Address), ByteSize); //Wird von destruktor aufgerufen!?
                disposed = true;
            }
        }

        public zCVob CamVob
        {
            get { return new zCVob(Address + 0x920); }
            set { Process.Write(Address + 0x920, value.Address); }
        }

        public float FOVx
        {
            get { return Process.ReadFloat(Address + 0x918); }
            set { Process.Write(Address + 0x918, value); }
        }

        public float FOVy
        {
            get { return Process.ReadFloat(Address + 0x91C); }
            set { Process.Write(Address + 0x91C, value); }
        }
    }
}
