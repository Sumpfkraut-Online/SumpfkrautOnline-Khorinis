using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCRtnManager : zClass
    {

        #region Offsets

        public enum FuncOffset
        {
            SetDailyRoutinePos = 0x007764D0,
            UpdateSingleRoutine = 0x00775080
        }

        public enum HookSize
        {

        }
        #endregion
        public oCRtnManager()
        { }
        public oCRtnManager(Process process, int address)
            : base(process, address)
        {

        }

        public static oCRtnManager  GetRtnManager(Process process)
        {
            return new oCRtnManager(process, 0x00AB31C8);
        }

        #region methods

        public void SetDailyRoutinePos(int day)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffset.SetDailyRoutinePos, new CallValue[] { new IntArg(day) });
        }

        public void UpdateSingleRoutine(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffset.UpdateSingleRoutine, new CallValue[] { npc });
        }
        #endregion
    }
}
