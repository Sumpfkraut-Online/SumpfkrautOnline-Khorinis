using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCPlayerInfo : zClass
    {
        public zCPlayerInfo()
        {

        }

        public zCPlayerInfo(Process process, int address)
            : base(process, address)
        {

        }



        public override uint ValueLength()
        {
            return 4;
        }
    }
}
