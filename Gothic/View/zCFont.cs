using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;
using System.Runtime.InteropServices;

namespace Gothic.View
{
    public class zCFont : zClass
    {
        public class FuncAddresses
        {
            public const int GetFontX = 0x007894F0,
            GetFontY = 0x007894E0;
        }

        /*public enum HookSize : uint
        {
            GetFontX = 5
        }*/
        
        public zCFont(int address) : base (address)
        {
        }

        public zCFont()
        {
        }

        public int GetFontX(zString str)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetFontX, str).Value;
        }

        public int GetFontY()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.GetFontY).Value;
        }
    }
}
