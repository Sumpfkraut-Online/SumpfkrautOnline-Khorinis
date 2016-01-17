using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zVec4 : zClass, IDisposable
    {
        public const int ByteSize = 16;

        public zVec4(int address)
            : base(address)
        {
            
        }

        public zVec4()
        {

        }

        public static zVec4 Create()
        {
            int ptr = Process.Alloc(zVec4.ByteSize).ToInt32();
            return new zVec4(ptr);
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
                Process.Free(new IntPtr(Address), zVec4.ByteSize);
                disposed = true;
            }
        }

        public float this[int index]
        {
            get
            {
                if (index >= 0 && index < 4)
                {
                    return Process.ReadFloat(Address + 4 * index);
                }
                throw new ArgumentException("zVec4 get index is outside of range: " + index);
            }
            set
            {
                if (index >= 0 && index < 4)
                {
                    Process.Write(value, Address + 4 * index);
                }
                throw new ArgumentException("zVec4 set index is outside of range: " + index);
            }
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
    }
}
