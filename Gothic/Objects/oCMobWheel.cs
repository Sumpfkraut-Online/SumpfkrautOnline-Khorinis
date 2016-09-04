using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCMobWheel : oCMobInter
    {
        public oCMobWheel()
        {
        }

        public oCMobWheel(int address)
            : base(address)
        {
        }

        public new static oCMobWheel Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x719B90, null); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x726FA0, null); //Konstruktor...
            return new oCMobWheel(address);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x7270A0, new BoolArg(true));
        }
    }
}
