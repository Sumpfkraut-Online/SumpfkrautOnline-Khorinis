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

        public static implicit operator FloatArg(float value)
        {
            return new FloatArg(value);
        }

        public static implicit operator float(FloatArg value)
        {
            return value.Address;
        }
    }
}
