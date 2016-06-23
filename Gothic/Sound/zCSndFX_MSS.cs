using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Objects;

namespace Gothic.Sound
{
    public class zCSndFX_MSS : zCObject
    {
        public zCSndFX_MSS()
        {

        }

        public zCSndFX_MSS(int address)
            : base(address)
        {
        }

        public bool isFixed
        {
            get { return Process.ReadBool(Address + 104); }
            set { Process.Write(value, Address + 104); }
        }

        public float Volume
        {
            get { return Process.ReadFloat(Address + 84); }
            set { Process.Write(value, Address + 84); }
        }
    }
}
