using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCCSManager : zClass
    {
        public zCCSManager()
        {

        }

        public zCCSManager(Process process, int address)
            : base(process, address)
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
