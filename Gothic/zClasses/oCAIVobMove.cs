using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCAIVobMove : zCAIBase
    {
        public oCAIVobMove(Process process, int address)
            : base(process, address)
        { 
        }

        public oCAIVobMove()
        {

        }


        public static oCAIVobMove Create(Process Process)
        {
            IntPtr addr = Process.Alloc(0x3C);
            zCClassDef.ObjectCreated(Process, addr.ToInt32(), 0x00AAD720);
            Process.THISCALL<NullReturnCall>((uint)addr.ToInt32(), 0x0069F220, null);
            return new oCAIVobMove(Process, addr.ToInt32());
        }

    }
}
