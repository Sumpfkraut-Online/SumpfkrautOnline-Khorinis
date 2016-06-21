using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCAIArrowBase : oCAISound
    {
        public oCAIArrowBase()
        {

        }

        public oCAIArrowBase(int address)
            : base(address)
        {
        }

        public void CreateTrail(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6A0420, vob);
        }
    }
}
