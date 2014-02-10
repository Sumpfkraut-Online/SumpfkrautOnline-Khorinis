using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class IntArg : CallValue
    {
        public IntArg(int i)
        {
            Address = i;
        }

        public IntArg()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }


        public static implicit operator IntArg(int value)
        {
            return new IntArg(value);
        }

        public static implicit operator int(IntArg value)
        {
            return value.Address;
        }
    }
}
