using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zStruct;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCNpc_States : zClass
    {
        #region OffsetLists
        public enum Offsets
        {
            name = 4,
            npc = 0x18,
            current_state = 0x1C,
            next_state = 0x58,
            
        }
        public enum FuncOffsets : uint
        {
            DoAIState = 0x0076D1A0,
            StartRtnState = 0x0076C2E0,
            InitAIStateDriven = 0x0076E8E0,
            ActivateRtnState = 0x0076C330,
            StartAIState = 0x0076C840,
            IsInState = 0x0076C040
        }

        public enum HookSize : uint
        {
            DoAIState = 6,
            ActivateRtnState = 6
        }

        #endregion

        public oCNpc_States()
        {

        }

        public oCNpc_States(Process process, int address)
            : base(process, address)
        {
        }

        public oCNpc Owner
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.npc)); }
        }
        public TNpcAIState CurrentState
        {
            get { return new TNpcAIState(Process, Address + (int)Offsets.current_state); }
        }

        public TNpcAIState NextState
        {
            get { return new TNpcAIState(Process, Address + (int)Offsets.next_state); }
        }

        public int StartAIState(uint type, int x, int y, float f, int z)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.StartAIState, new CallValue[] { new UIntArg(type), new IntArg(x), new IntArg(y), new FloatArg(f), new IntArg(z) });
        }

        public int IsInState(uint type)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsInState, new CallValue[] { new UIntArg(type) });
        }

        public void StartRtnState(int day)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartRtnState, new CallValue[] { new IntArg(day) });
        }

        public void ActivateRtnState(int day)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ActivateRtnState, new CallValue[] { new IntArg(day) });
        }

        public void InitAIStateDriven(zVec3 day)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.InitAIStateDriven, new CallValue[] { day });
        }
    }
}
