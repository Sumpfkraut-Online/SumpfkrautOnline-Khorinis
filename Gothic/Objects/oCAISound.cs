using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCAISound : zCAIBase
    {
        public oCAISound()
        {

        }

        public oCAISound(int address)
            : base(address)
        {
        }
        
        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x69F510, (BoolArg)true);
        }
    }
}
