using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCNpcFocus
    {
        public static void SetFocusMode(Process process, int mode)
        {
            process.CDECLCALL<NullReturnCall>((uint)0x6BEC20, new CallValue[] { (IntArg)mode });
        }
    }
}
