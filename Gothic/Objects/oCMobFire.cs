using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCMobFire : oCMobInter
    {
        public oCMobFire()
        {
        }

        public oCMobFire(int address)
            : base(address)
        {
        }

        public new static oCMobFire Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x71A640); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x722460); //Konstruktor...
            return new oCMobFire(address);
        }
    }
}
