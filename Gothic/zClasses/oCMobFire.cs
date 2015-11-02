using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCMobFire : oCMobInter
    {
        public oCMobFire()
        {

        }

        public oCMobFire(Process process, int address)
            : base(process, address)
        {
        }

        public new static oCMobFire Create(Process process)
        {
            int address = process.CDECLCALL<IntArg>(0x71A640, null); //_CreateInstance()
            process.THISCALL<NullReturnCall>((uint)address, 0x722460, null); //Konstruktor...
            return new oCMobFire(process, address);
        }
    }
}
