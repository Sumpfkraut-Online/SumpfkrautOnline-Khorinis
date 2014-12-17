using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMobBed : oCMobInter
    {

        public oCMobBed()
        {

        }

        public oCMobBed(Process process, int address)
            : base(process, address)
        {
        }

        public static oCMobBed Create(Process process)
        {
            IntPtr ptr = process.Alloc(0x248);
            zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0x00AB14A0);
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x0071D010, new CallValue[] { });

            return new oCMobBed(process, ptr.ToInt32());
        }
    }
}
