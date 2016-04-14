using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class zCAIPlayer : zCAIBase
    {
        public zCAIPlayer()
        {

        }

        public zCAIPlayer(int address)
            : base(address)
        {
        }

        public bool CheckEnoughSpaceMoveForward(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x511700, (BoolArg)arg);
        }
        
        public bool CheckEnoughSpaceMoveBackward(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x511740, (BoolArg)arg);
        }

        public bool CheckEnoughSpaceMoveLeft(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x5117D0, (BoolArg)arg);
        }

        public bool CheckEnoughSpaceMoveRight(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x511790, (BoolArg)arg);
        }
    }
}
