using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic
{
    public class zClass : CallValue
    {
        public zClass(int address)
        {
            this.Initialize(address);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public virtual int SizeOf()
        {
            return 0;
        }
    }
}
