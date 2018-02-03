using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

namespace Gothic.Objects
{
    public class oCAIVobMove : oCAISound
    {
        public oCAIVobMove()
        {
        }

        public oCAIVobMove(int address) : base(address)
        {
        }

        public void Init(zCVob self, zCVob owner, zVec3 start, float angle, float force, zMat4 trafo)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x69F540, self, owner, start, (FloatArg)angle, (FloatArg)force, trafo);
        }
    }
}
