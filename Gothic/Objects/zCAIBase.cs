using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class zCAIBase : zCObject
    {
        public zCAIBase()
        {

        }

        public zCAIBase(int address)
            : base(address)
        {
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x48BE20, new BoolArg(true));
        }
    }
}
