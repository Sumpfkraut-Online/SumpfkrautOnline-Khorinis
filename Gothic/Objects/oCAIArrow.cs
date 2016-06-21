using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCAIArrow : oCAIArrowBase
    {
        public oCAIArrow()
        {

        }

        public oCAIArrow(int address)
            : base(address)
        {
        }

        public static oCAIArrow Create()
        {
            return new oCAIArrow(Process.CDECLCALL<IntArg>(0x6A2DC0)); // _CreateInstance()
        }

        public int GetSoundMaterial(zCVob vob, int arg)
        {
            return Process.THISCALL<IntArg>(Address, 0x69EF30, vob, (IntArg)arg);
        }
    }
}
