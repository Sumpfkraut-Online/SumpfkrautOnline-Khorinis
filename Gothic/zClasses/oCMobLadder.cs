using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCMobLadder : oCMobInter
    {
        public oCMobLadder()
        {

        }

        public oCMobLadder(Process process, int address)
            : base(process, address)
        {
        }

        public new static oCMobLadder Create(Process process)
        {
            int address = process.CDECLCALL<IntArg>(0x719EE0, null); //_CreateInstance()
            process.THISCALL<NullReturnCall>((uint)address, 0x7274C0, null); //Konstruktor...
            return new oCMobLadder(process, address);
        }
    }
}
