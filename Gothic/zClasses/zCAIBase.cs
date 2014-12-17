using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCAIBase : zCObject
    {
        public zCAIBase(Process process, int address)
            : base(process, address)
        { 
        }

        public zCAIBase()
        {

        }
    }
}
