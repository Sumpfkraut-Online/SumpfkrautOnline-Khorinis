using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public static class oCNpcFocus
    {
        public static void SetFocusMode(int mode)
        {
            Process.CDECLCALL<NullReturnCall>(0x6BEC20, (IntArg)mode);
        }

        public static void StartHighlightingFX(oCNpc target)
        {
            Process.CDECLCALL<NullReturnCall>(0x6BF390, target);
        }

        public static void StopHighlightingFX()
        {
            Process.CDECLCALL<NullReturnCall>(0x6BF4C0);
        }
    }
}
