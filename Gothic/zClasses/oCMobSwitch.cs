using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMobSwitch : oCMobInter
    {
        public oCMobSwitch()
        {

        }

        public oCMobSwitch(Process process, int address)
            : base(process, address)
        {
        }

        public static oCMobSwitch Create(Process process)
        {
            int address = process.CDECLCALL<IntArg>(0x718F20, null); //_CreateInstance()
            process.THISCALL<NullReturnCall>((uint)address, 0x7234E0, null); //Konstruktor...
            return new oCMobSwitch(process, address);
        }
    }
}
