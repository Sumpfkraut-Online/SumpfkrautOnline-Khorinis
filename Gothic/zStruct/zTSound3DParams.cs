using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zStruct
{
    public class zTSound3DParams : Gothic.zClasses.zClass, IDisposable
    {
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
            process.Write(0x3F800000, pointer.ToInt32()+4);
            process.Write(0x0BF800000, pointer.ToInt32() + 8);//0x451C40
            process.Write(0, pointer.ToInt32()+12);
            process.Write(0, pointer.ToInt32()+16);
            process.Write(0x3F800000, pointer.ToInt32() + 20);//3F80
            process.Write(0, pointer.ToInt32() + 24);//Ambient? 0 == No, 1 == Call Sound3DAmbient!
            process.Write(0x0, pointer.ToInt32() + 28);

            return new zTSound3DParams(process, pointer.ToInt32());
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
