using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCMobLadder : oCMobInter
    {
        public oCMobLadder()
        {
        }

        public oCMobLadder(int address)
            : base(address)
        {
        }

        public new static oCMobLadder Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x719EE0); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x7274C0); //Konstruktor...
            return new oCMobLadder(address);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x7275D0, new BoolArg(true));
        }
    }
}
