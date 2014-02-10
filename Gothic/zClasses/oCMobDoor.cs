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
    }
}
