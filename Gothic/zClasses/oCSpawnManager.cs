using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCSpawnManager : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {

        }

        public enum FuncOffsets : uint
        {
            DeleteNPC = 0x00779690,
            SpawnNPCInt = 0x00778B20,
            SpawnNPC = 0x00778E70,
            SpawnNPC_Str = 0x00778BA0,
            SummonNPC = 0x00778A20
        }

        public enum HookSize : uint
        {
            DeleteNPC = 7,
            SpawnNPCInt = 10,
            SpawnNPC = 6,
            SpawnNPC_Str = 7,
            SummonNPC = 6
        }
        #endregion


        #region methods

        public void DeleteNPC(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DeleteNPC, new CallValue[] { npc });
        }

        public void SpawnNPC(oCNpc npc, zString pos, int value)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SpawnNPC_Str, new CallValue[] { npc, pos, new IntArg(value) });
        }

        public void SpawnNPC(oCNpc npc, zVec3 pos, int value)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SpawnNPC, new CallValue[] { npc, pos, new IntArg(value) });
        }

        public void SummonNPC(int npc, zVec3 pos, int value)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SummonNPC, new CallValue[] { new IntArg(npc), pos, new IntArg(value) });
        }

        #endregion

        #region Standard
        public oCSpawnManager(Process process, int address) : base (process, address)
        {
            
        }

        public oCSpawnManager()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion
    }
}
