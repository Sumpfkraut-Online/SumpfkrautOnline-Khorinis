using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCMobWheel : oCMobInter
    {
        public oCMobWheel()
        {

        }

        public oCMobWheel(Process process, int address)
            : base(process, address)
        {
        }

        public new static oCMobWheel Create(Process process)
        {
            int address = process.CDECLCALL<IntArg>(0x719B90, null); //_CreateInstance()
            process.THISCALL<NullReturnCall>((uint)address, 0x726FA0, null); //Konstruktor...
            return new oCMobWheel(process, address);
        }
    }
}
