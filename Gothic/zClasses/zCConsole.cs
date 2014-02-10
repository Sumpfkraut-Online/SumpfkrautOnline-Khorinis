using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCConsole : zCInputCallback
    {
        public zCConsole()
        {}

        public zCConsole(Process process, int address)
            : base(process, address)
        {

        }
        public static zCConsole Console(Process process)
        {
            return new zCConsole(process, 0xAB3860);
        }

        public void Show()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, 0x00783460, new CallValue[] { });
        }

        public int IsVisible()
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x00783890, new CallValue[] { }).Address;
        }
    }
}
