using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCStealContainer : oCItemContainer
    {

        public oCStealContainer(Process process, int address)
            : base(process, address)
        {

        }

        public enum FuncOffsets
        {
            SetOwner = 0x0070ADB0
        }

        public enum HookSizes
        {
            SetOwner = 6
        }

        public static oCStealContainer GetStealContainer(Process process)
        {
            return new oCStealContainer(process, process.ReadInt(0x00AB27DC));
        }

        public void SetOwner(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (int)FuncOffsets.SetOwner, new CallValue[] { npc });
        }

        public void CreateList()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, 0x0070ADE0, new CallValue[] { });
        }
    }
}
