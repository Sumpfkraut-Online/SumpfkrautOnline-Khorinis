using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCAiHuman : zCObject
    {
        #region OffsetLists
        public enum Offsets
        {
            NPC = 0x12C
        }
        public enum FuncOffsets : uint
        {
            StopTurnAnis = 0x006AE530,
            DoSimpleAi = 0x006953B0,
            DoAi = 0x0069BB63,
            Moving = 0x0069B9B0, //Protected
            Init = 0x00695390,
            InitAnimations = 0x006A4010,
            InitAllAnis = 0x006A5BF0
        }

        public enum HookSize : uint
        {
            DoSimpleAi = 6,
            DoAi = 5
        }

        #endregion

        public oCAiHuman()
        {

        }

        public oCAiHuman(Process process, int address)
            : base(process, address)
        {
        }

        public oCNpc NPC
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.NPC)); }
            set { Process.Write(value.Address, Address + (int)Offsets.NPC); }
        }

        public void Init(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Init, new CallValue[] { npc });
        }

        public void InitAnimations()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.InitAnimations, new CallValue[] { });
        }

        public void InitAllAnis()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.InitAllAnis, new CallValue[] { });
        }


        public void Destroy()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x00695290, new CallValue[] { new IntArg(1) });
        }
    }
}
