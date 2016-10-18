using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCWorld : zCWorld
    {
        public oCWorld()
        {
        }

        public oCWorld(int address) : base(address)
        {
        }

        new public const int ByteSize = 0x628C;

        new public static oCWorld Create()
        {
            return new oCWorld(Process.CDECLCALL<IntArg>(0x77ED20));
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x77EFB0, (BoolArg)true);
        }
    }
}
