using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCMobBed : oCMobInter
    {
        public oCMobBed()
        {
        }

        public oCMobBed(int address)
            : base(address)
        {
        }

        public new static oCMobBed Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x718A50); //_CreateInstance()
            //Process.THISCALL<NullReturnCall>(address, 0x722E50); //Konstruktor...
            return new oCMobBed(address);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x723010, new BoolArg(true));
        }
    }
}
