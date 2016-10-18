using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCMobSwitch : oCMobInter
    {
        public oCMobSwitch()
        {
        }

        public oCMobSwitch(int address)
            : base(address)
        {
        }

        new public static oCMobSwitch Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x718F20); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x7234E0); //Konstruktor...
            return new oCMobSwitch(address);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x7235D0, new BoolArg(true));
        }
    }
}
