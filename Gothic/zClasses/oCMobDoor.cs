using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCMobDoor : oCMobLockable
    {
        #region OffsetLists
        public enum Offsets
        {
            
        }
        public enum FuncOffsets : uint
        {
            Open = 0x0071A430,
            Close = 0x0071A440
        }

        public enum HookSize : uint
        {
        }

        #endregion

        public oCMobDoor()
        {

        }

        public oCMobDoor(Process process, int address)
            : base(process, address)
        {
        }

        public static oCMobDoor Create(Process process)
        {
            IntPtr ptr = process.Alloc(0x274);
            zCClassDef.ObjectCreated(process, ptr.ToInt32(), 0x00AB1518);
            process.THISCALL<NullReturnCall>((uint)ptr.ToInt32(), (uint)0x0071D010, new CallValue[] { });

            return new oCMobDoor(process, ptr.ToInt32());
        }
    }
}
