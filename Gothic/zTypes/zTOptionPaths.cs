using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zTypes
{
    public class zTOptionPaths : zClass
    {
        public byte value;
        public int PTR;

        public zTOptionPaths(Process process, String val, byte bval)
        {
            PTR = process.Alloc((uint)val.Length + 1).ToInt32();
            value = bval;
        }

        public override List<byte[]> getCallParam()
        {
            List<byte[]> r = new List<byte[]>();
            r.Add(new byte[]{value});
            r.Add(BitConverter.GetBytes(PTR));
            return r;
        }

        public override uint ValueLength()
        {
            return 8;
        }
        
    }
}
