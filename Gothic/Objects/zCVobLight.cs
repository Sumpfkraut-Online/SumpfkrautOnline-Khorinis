using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class zCVobLight : zCVob
    {
        public zCVobLight(int address) : base(address)
        {
        }
        
        new public static zCVobLight Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x607C60); //_CreateInstance()
            return new zCVobLight(address);
        }

        public void SetRange(float range, bool idk)
        {
            Process.THISCALL<NullReturnCall>(this.Address, 0x608320, new FloatArg(range), new BoolArg(idk));
        }
    }
}
