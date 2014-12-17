using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMobSwitch : oCMobInter
    {

        public oCMobSwitch()
        {

        }

        public oCMobSwitch(Process process, int address)
            : base(process, address)
        {
        }

        public static oCMobSwitch Create(Process process)
        {
            IntPtr ptr = process.Alloc(0x234);
            zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0x00AB17C0);//0x00AB1518) => MobDoor;
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x0071D010, new CallValue[] { });

            return new oCMobSwitch(process, ptr.ToInt32());
        }
    }
}
