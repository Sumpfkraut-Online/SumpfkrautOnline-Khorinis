using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class BoolArg : CallValue
    {
        public BoolArg(bool i)
        {
            Address = Convert.ToInt32(i);
        }

        public BoolArg()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
