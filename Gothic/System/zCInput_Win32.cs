using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.System
{
    public static class zCInput_Win32
    {
        #region Gothic Methods

        public const int zinput = 0x008D1650;

        public abstract class FuncAddresses
        {
            /// <summary>
            /// public: virtual void __thiscall zCInput_Win32::ProcessInputEvents(void)
            /// </summary>
            public const int ProcessInputEvent = 0x004D5700; 
        }

        public static void ProcessInputEvents()
        {
            Process.THISCALL<NullReturnCall>(zinput, FuncAddresses.ProcessInputEvent);
        }

        #endregion
    }
}
