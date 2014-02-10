using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zTypes
{
    public class zColor : zClass
    {
        public zColor()
        {

        }

        public zColor(Process process, int address)
            : base(process, address)
        {

        }

        public static zColor Create(Process process, byte r, byte g, byte b, byte a)
        {
            IntPtr colorAddr = process.Alloc(4);
            int arr = BitConverter.ToInt32(new byte[] { b, g, r, a }, 0);
            process.THISCALL<NullReturnCall>((uint)colorAddr.ToInt32(), (uint)0x00401F20, new CallValue[] { new IntArg(arr) });

            return new zColor(process, colorAddr.ToInt32());
        }

        public byte B
        {
            get { return Process.ReadByte(Address + 0); }
            set { Process.Write(new byte[]{value}, Address + 0); }
        }

        public byte G
        {
            get { return Process.ReadByte(Address + 1); }
            set { Process.Write(new byte[] { value }, Address + 1); }
        }

        public byte R
        {
            get { return Process.ReadByte(Address + 2); }
            set { Process.Write(new byte[] { value }, Address + 2); }
        }

        public byte A
        {
            get { return Process.ReadByte(Address + 3); }
            set { Process.Write(new byte[] { value }, Address + 3); }
        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
