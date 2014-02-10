using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCSkyControler_Barrier : zCSkyControler_Outdoor
    {

        public enum FuncOffsets
        {
            RenderSkyPre = 0x006BB8D0
        }

        public enum HookSize
        {
            RenderSkyPre = 5
        }


        public oCSkyControler_Barrier(Process process, int address)
            : base(process, address)
        {
        }

        public oCSkyControler_Barrier() { }


        public static oCSkyControler_Barrier Create(Process process)
        {
            IntPtr address = process.Alloc(0x6c4);//0x6c4

            zCClassDef.ObjectCreated(process, address.ToInt32(), 0x0099ACD8);
            process.THISCALL<NullReturnCall>((uint)address.ToInt32(), 0x006BB7B0, new CallValue[] { });
            return new oCSkyControler_Barrier(process, address.ToInt32());
        }


        public void RenderSkyPre()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.RenderSkyPre, new CallValue[] { });
        }
        
    }
}
