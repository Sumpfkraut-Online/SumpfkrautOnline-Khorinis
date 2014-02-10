using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCAniCtrl_Human : oCAiHuman
    {
        #region OffsetLists
        public enum Offsets
        {
        }
        public enum FuncOffsets : uint
        {
            StopTurnAnis = 0x006AE530,
            GetWalkModeString = 0x006AAE40,
            _Forward = 0x006B7900,
            _Stand = 0x006B7490,
            SetWalkMode = 0x006A9820,
            SetAlwaysWalk = 0x006ABDB0,
            ToggleWalkMode = 0x006AD500
        }

        public enum HookSize : uint
        {

        }

        #endregion

        public oCAniCtrl_Human()
        {

        }

        public oCAniCtrl_Human(Process process, int address)
            : base(process, address)
        {
        }

        public void SetAlwaysWalk(int x)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SetAlwaysWalk, new CallValue[] { new IntArg(x) });
        }

        public void ToggleWalkMode(int x)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.ToggleWalkMode, new CallValue[] { new IntArg(x) });
        }

        public void ToggleWalkMode(char x)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.ToggleWalkMode, new CallValue[] { new CharArg(x) });
        }

        public void SetWalkMode( int x )
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SetWalkMode, new CallValue[] { new IntArg(x) });
        }

        public void _Forward()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets._Forward, new CallValue[] { });
        }

        public void _Stand()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets._Stand, new CallValue[] { });
        }




        public zString GetWalkModeString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetWalkModeString, new CallValue[]{new IntArg(str)});
            return new zString(Process, arg.Address);
        }

    }
}
