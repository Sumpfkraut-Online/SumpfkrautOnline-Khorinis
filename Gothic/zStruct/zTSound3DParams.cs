using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zStruct
{
    public class zTSound3DParams : Gothic.zClasses.zClass, IDisposable
    {
        #region OffsetLists
        public enum Offsets
        {
            obstruction = 0x0, //float
            volume = 0x4, //float
            radius = 8, //float
            loopType = 12, //zTLoopType
            angle = 16,//float
            level = 20,//float
            ambient3d = 24,//bool
            pitch = 28//float
        }
        public enum FuncOffsets : uint
        {
            
        }

        public enum HookSize : uint
        {

        }

        #endregion


        public zTSound3DParams()
        { }
        public zTSound3DParams(Process process, int address)
            : base(process, address)
        {

        }

        public static zTSound3DParams Create(Process process)
        {
            IntPtr pointer = process.Alloc(0x20);
            process.Write(0, pointer.ToInt32());
            process.Write(1f, pointer.ToInt32()+4);
            process.Write(-1f, pointer.ToInt32() + 8);//0x451C40
            process.Write(0, pointer.ToInt32()+12);
            process.Write(0, pointer.ToInt32()+16);
            process.Write(1f, pointer.ToInt32() + 20);//3F80
            process.Write(0, pointer.ToInt32() + 24);//Ambient? 0 == No, 1 == Call Sound3DAmbient!
            process.Write(0x0, pointer.ToInt32() + 28);

            return new zTSound3DParams(process, pointer.ToInt32());
        }

        public float Obstruction
        {
            get { return Process.ReadFloat(this.Address +  (int)Offsets.obstruction); }
            set { Process.Write(value, this.Address + (int)Offsets.obstruction); }
        }

        public float Volume
        {
            get { return Process.ReadFloat(this.Address + (int)Offsets.volume); }
            set { Process.Write(value, this.Address + (int)Offsets.volume); }
        }

        public float Radius
        {
            get { return Process.ReadFloat(this.Address + (int)Offsets.radius); }
            set { Process.Write(value, this.Address + (int)Offsets.radius); }
        }

        public int LoopType
        {
            get { return Process.ReadInt(this.Address + (int)Offsets.loopType); }
            set { Process.Write(value, this.Address + (int)Offsets.loopType); }
        }

        public float Angle
        {
            get { return Process.ReadFloat(this.Address + (int)Offsets.angle); }
            set { Process.Write(value, this.Address + (int)Offsets.angle); }
        }

        public float Pitch
        {
            get { return Process.ReadFloat(this.Address + (int)Offsets.pitch); }
            set { Process.Write(value, this.Address + (int)Offsets.pitch); }
        }

        public float Level
        {
            get { return Process.ReadFloat(this.Address + (int)Offsets.level); }
            set { Process.Write(value, this.Address + (int)Offsets.level); }
        }

        public bool Ambient3D
        {
            get { return Process.ReadInt(this.Address + (int)Offsets.ambient3d) >= 1; }
            set { Process.Write(value ? 1 : 0, this.Address + (int)Offsets.ambient3d); }
        }
        

        public override uint ValueLength()
        {
            return 4;
        }


        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.Free(new IntPtr(Address), 0x20);
                disposed = true;
            }
        }
    }
}
