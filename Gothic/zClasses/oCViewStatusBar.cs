using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCViewStatusBar : zCViewStatusBar
    {
        public oCViewStatusBar(Process process, int address)
            : base(process, address)
        {
        }
    }
}
