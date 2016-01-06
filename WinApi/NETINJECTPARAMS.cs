using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class NETINJECTPARAMS : IDisposable
    {
        public int Address;

        public static NETINJECTPARAMS Create(String dllname, String typename, String methodname, String ptrAddress)
        {
            NETINJECTPARAMS rValue = new NETINJECTPARAMS();

            IntPtr structPtr = Process.Alloc(16);
            IntPtr dllPtr = Process.Alloc((uint)dllname.Length + 1);
            IntPtr typePtr = Process.Alloc((uint)typename.Length + 1);
            IntPtr methodPtr = Process.Alloc((uint)methodname.Length + 1);
            IntPtr adressPtr = Process.Alloc((uint)ptrAddress.Length + 1);

            Process.Write(dllPtr.ToInt32(), structPtr.ToInt32());
            Process.Write(typePtr.ToInt32(), structPtr.ToInt32() + 4);
            Process.Write(methodPtr.ToInt32(), structPtr.ToInt32() + 8);
            Process.Write(adressPtr.ToInt32(), structPtr.ToInt32() + 12);

            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            Process.Write(enc.GetBytes(dllname), dllPtr.ToInt32());
            Process.Write(enc.GetBytes(typename), typePtr.ToInt32());
            Process.Write(enc.GetBytes(methodname), methodPtr.ToInt32());
            Process.Write(enc.GetBytes(ptrAddress), adressPtr.ToInt32());

            rValue.Address = structPtr.ToInt32();
            return rValue;
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                int dllPtr = Process.ReadInt(Address);
                int typePtr = Process.ReadInt(Address + 4);
                int methodPtr = Process.ReadInt(Address + 8);
                int adressPtr = Process.ReadInt(Address + 12);

                uint i = 0;

                while (Process.ReadByte(dllPtr + (int)i) != 0x00)
                    i++;
                Process.Free(new IntPtr(dllPtr), i);

                i = 0;
                while (Process.ReadByte(typePtr + (int)i) != 0x00)
                    i++;
                Process.Free(new IntPtr(typePtr), i);

                i = 0;
                while (Process.ReadByte(methodPtr + (int)i) != 0x00)
                    i++;
                Process.Free(new IntPtr(methodPtr), i);

                i = 0;
                while (Process.ReadByte(adressPtr + (int)i) != 0x00)
                    i++;
                Process.Free(new IntPtr(adressPtr), i);

                Process.Free(new IntPtr(Address),16);
                disposed = true;
            }
        }
    }
}
