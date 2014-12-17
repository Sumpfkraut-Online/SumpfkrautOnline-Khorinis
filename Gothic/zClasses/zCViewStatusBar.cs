using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCViewStatusBar : zCView
    {
        public zCViewStatusBar(Process process, int address)
            : base(process, address)
        {
        }
    }
}
