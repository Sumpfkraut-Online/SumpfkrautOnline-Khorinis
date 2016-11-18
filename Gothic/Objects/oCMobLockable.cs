using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public abstract class oCMobLockable : oCMobInter
    {
        new public abstract class VarOffsets : oCMobInter.VarOffsets
        {
            public const int IsLocked = 0x234,
            keyInstance = 0x238,
            PickLockStr = 0x24C;
        }
        new public abstract class FuncAddresses : oCMobInter.FuncAddresses
        {
            public const int SetLocked = 0x00719620,
            SetPickLockStr = 0x00719780,
            Unlock = 0x00724A70,
            PickLock = 0x00724800,
            Lock = 0x007251C0,
            SetKeyInstance = 0x00719640;
        }

        /*public enum HookSize : uint
        {
            SetLocked = 6,
            SetPickLockStr = 8,
            Unlock = 7,
            PickLock = 6,
            Lock = 6,
            SetKeyInstance = 8
        }*/

        public oCMobLockable()
        {
        }

        public oCMobLockable(int address)
            : base(address)
        {
        }

        public void PickLock(oCNpc npc, char ch)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PickLock, npc, new CharArg(ch));
        }

        public void SetLocked(int locked)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetLocked, new IntArg(locked));
        }

        public void SetPickLockStr(String lockString)
        {
            using (zString zS = zString.Create(lockString))
                SetPickLockStr(zS);
        }

        public void SetPickLockStr(zString lockString)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetPickLockStr, lockString);
        }

        public void SetKeyInstance(String lockString)
        {
            using (zString zS = zString.Create(lockString))
                SetKeyInstance(zS);
        }

        public void SetKeyInstance(zString lockString)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetKeyInstance, lockString);
        }

        public bool IsLocked
        {
            get { return Process.ReadBool(Address + VarOffsets.IsLocked); }
            set { Process.Write(Address + VarOffsets.IsLocked, value); }
        }

        public zString keyInstance
        {
            get { return new zString(Address + VarOffsets.keyInstance); }
        }

        public zString PickLockStr
        {
            get { return new zString(Address + VarOffsets.PickLockStr); }
        }
    }
}
