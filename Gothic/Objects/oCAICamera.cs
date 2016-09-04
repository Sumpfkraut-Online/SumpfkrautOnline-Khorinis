using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCAICamera : zCAIBase
    {
        public const int Size = 0x4C;

        public oCAICamera()
        {

        }

        public oCAICamera(int address)
            : base(address)
        {
        }

        public static oCAICamera Create()
        {
            return new oCAICamera(Process.CDECLCALL<IntArg>(0x69E760));
        }
        
        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x69DE60, (BoolArg)true);
        }
    }
}
