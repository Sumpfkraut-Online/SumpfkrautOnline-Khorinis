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
            int address = process.CDECLCALL<IntArg>(0x71A250, null); //_CreateInstance()
            process.THISCALL<NullReturnCall>((uint)address, 0x7269B0, null); //Konstruktor...
            return new oCMobDoor(process, address);
        }
    }
}
