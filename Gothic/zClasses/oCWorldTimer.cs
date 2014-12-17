using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class oCWorldTimer : zClass
    {

        #region Offsets

        public enum FuncOffset
        {
            SetDay  = 0x00780DE0, 
            SetTime = 0x00780E40,
            Timer = 0x00780D80
        }

        public enum HookSize
        {

        }
        #endregion
        public oCWorldTimer()
        { }
        public oCWorldTimer(Process process, int address)
            : base(process, address)
        {

        }

        #region methods

        public void SetDay(int day)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffset.SetDay, new CallValue[] { new IntArg(day) });
        }

        public void SetTime(int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffset.SetTime, new CallValue[] { new IntArg(hour), new IntArg(minute) });
        }

        public void Timer()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffset.Timer, new CallValue[] { });
        }
        #endregion
    }
}
