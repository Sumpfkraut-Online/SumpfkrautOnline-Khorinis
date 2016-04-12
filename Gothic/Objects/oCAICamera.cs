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
            int address = Process.CDECLCALL<IntArg>(0x69E760); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x69DD00); //Konstruktor...
            return new oCAICamera(address);
        }
    }
}
