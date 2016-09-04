using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public abstract class oCVob : zCVob
    {
        public oCVob()
        {
        }

        public oCVob(int address)
            : base(address)
        {
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x77B6E0, (BoolArg)true);
        }
    }
}
