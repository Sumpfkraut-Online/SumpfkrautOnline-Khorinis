using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMobLockable : oCMobInter
    {
         #region OffsetLists
        public enum Offsets
        {
            IsLocked = 0x234,
            keyInstance = 0x568,
            PickLockStr = 0x588
        }
        public enum FuncOffsets : uint
        {
            SetLocked = 0x00719620,
            SetPickLockStr = 0x00719780,
            Unlock = 0x00724A70,
            PickLock = 0x00724800,
            Lock = 0x007251C0,
            SetKeyInstance = 0x00719640
        }

        public enum HookSize : uint
        {
            SetLocked = 6,
            SetPickLockStr = 8,
            Unlock = 7,
            PickLock = 6,
            Lock = 6,
            SetKeyInstance = 8
        }

        #endregion

        public oCMobLockable()
        {

        }

        public oCMobLockable(Process process, int address)
            : base(process, address)
        {
        }

        public void PickLock(oCNpc npc, char ch)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PickLock, new CallValue[] { npc, new CharArg(ch) });
        }

        public void SetLocked(int locked)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetLocked, new CallValue[] { new IntArg(locked) });
        }

        public zString keyInstance
        {
            get { return new zString(Process, Address + (int)Offsets.keyInstance); }
        }

        public zString PickLockStr
        {
            get { return new zString(Process, Address + (int)Offsets.PickLockStr); }
        }
    }
}
