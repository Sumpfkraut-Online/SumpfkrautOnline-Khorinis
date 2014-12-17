using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCInputCallback : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {
        }

        public enum FuncOffsets : uint
        {
            SetEnableHandleEvent = 0x007A54E0
        }

        public enum HookSize : uint
        {
            SetEnableHandleEvent = 6
        }
        #endregion

        #region Standard
        public zCInputCallback(Process process, int address) : base (process, address)
        {
            
        }

        public zCInputCallback()
        {

        }
        #endregion

        #region statics
        

        #endregion

        #region Fields
        
        #endregion

        #region methods

        public void SetEnableHandleEvent(int b)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetEnableHandleEvent, new CallValue[] { new IntArg(b) });
        }

        #endregion
    }
}
