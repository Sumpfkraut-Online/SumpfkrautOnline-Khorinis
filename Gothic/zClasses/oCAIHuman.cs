using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCAiHuman : zClass
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
            Moving = 0x0069B9B0 //Protected
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


    }
}
