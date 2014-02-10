using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class FloatArg : CallValue
    {
        float val;
        public FloatArg(float i)
        {
            val = i;
        }

        public FloatArg()
        {

        }

        public override List<byte[]> getCallParam()
        {
            List<byte[]> bL = new List<byte[]>();
            bL.Add(BitConverter.GetBytes(val));
            return bL;
        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
