using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class CharArg : CallValue
    {
        public CharArg(char i)
        {
            Address = Convert.ToInt32(i);
        }

        public CharArg()
        {

        }

        public override List<byte[]> getCallParam()
        {
            List<byte[]> b = new List<byte[]>();
            b.Add(new byte[] { (byte)Address });
            return b;
        }

        public override uint ValueLength()
        {
            return 1;
        }
    }
}
