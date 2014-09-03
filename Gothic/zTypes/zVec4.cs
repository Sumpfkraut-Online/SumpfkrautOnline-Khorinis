using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace Gothic.zTypes
{
    public class zVec4 : zClass, IDisposable
    {
        public zVec4(Process process, int address)
            : base(process, address)
        {
            
        }

        public zVec4()
        {

        }

        public static zVec4 Create(Process process)
        {
            zVec4 rV = new zVec4();
            IntPtr addr = process.Alloc((uint)rV.SizeOf());
            rV.Initialize(process, addr.ToInt32());
            return rV;
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
                Process.Free(new IntPtr(Address), (uint)this.SizeOf());
                disposed = true;
            }
        }

        public float get(int index)
        {
            if (index > 3)
                throw new ArgumentException("Index can not be larger that 3");


            return Process.ReadFloat(Address + index * 4);
        }

        public float X 
        {
            get { return Process.ReadFloat(Address); }
            set { Process.Write(value, Address); }
        }

        public float Y
        {
            get { return Process.ReadFloat(Address + 4); }
            set { Process.Write(value, Address + 4); }
        }

        public float Z
        {
            get { return Process.ReadFloat(Address + 8); }
            set { Process.Write(value, Address + 8); }
        }

        public float W
        {
            get { return Process.ReadFloat(Address + 12); }
            set { Process.Write(value, Address + 12); }
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public override int SizeOf()
        {
            return 16;
        }
    }
}
