using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.View
{
    public class zCViewDraw : zClass
    {
        public class FuncAddresses
        {
            public const int DrawChildren = 0x00690300;
        }

        /*public enum HookSizes
        {
            DrawChildren = 6
        }*/

        public zCViewDraw()
        {
        }

        public zCViewDraw(int address)
            : base(address)
        {
        }

        public static zCViewDraw GetScreen()
        {
            return new zCViewDraw(Process.ReadInt(0x00AAD298));
        }
    }
}
