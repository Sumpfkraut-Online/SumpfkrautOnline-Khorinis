using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCWorldTimer : zClass
    {

        #region Offsets

        public abstract class FuncAddresses
        {
            public const int SetDay = 0x00780DE0,
            SetTime = 0x00780E40,
            Timer = 0x00780D80;
        }

        #endregion

        public oCWorldTimer()
        { }
        public oCWorldTimer(int address)
            : base(address)
        {

        }

        public void SetDay(int day)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetDay, new IntArg(day));
        }

        public void SetTime(int hour, int minute)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetTime, new IntArg(hour), new IntArg(minute));
        }

        public void Timer()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Timer);
        }
    }
}
