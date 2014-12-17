using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCVobSpot : zCVob
    {
        public zCVobSpot(Process process, int address)
            : base(process, address)
        {

        }

        public static zCClassDef GetClassDef(Process Process)
        {
            return new zCClassDef(Process, 0x00AB6648);
        }
    }
}
