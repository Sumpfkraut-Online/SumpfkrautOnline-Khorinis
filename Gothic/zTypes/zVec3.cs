using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace Gothic.zTypes
{
    public class zVec3 : zClass, IDisposable
    {
        public zVec3(Process process, int address)
            : base(process, address)
        {
            
        }

        public zVec3()
        {

        }

        public static zVec3 Create(Process process)
        {
            zVec3 rV = new zVec3();
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
            if (index > 2)
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

        public override uint ValueLength()
        {
            return 4;
        }

        public override int SizeOf()
        {
            return 12;
        }
    }
}
