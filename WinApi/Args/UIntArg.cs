using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class UIntArg : CallValue
    {
        public UIntArg(uint i)
        {
            Address = (int)i;
        }

        public UIntArg()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }


        public static implicit operator UIntArg(uint value)
        {
            return new UIntArg(value);
        }

        public static implicit operator uint(UIntArg value)
        {
            return (uint)value.Address;
        }
    }
}
