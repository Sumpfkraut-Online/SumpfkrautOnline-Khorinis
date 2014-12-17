using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCEventManager : zClass
    {
        public zCEventManager()
            : base()
        {

        }

        public zCEventManager(Process process, int address)
            : base(process, address)
        {

        }

        #region OffsetLists

        public enum FuncOffsets
        {
            OnMessage = 0x00786380
        }

        #endregion



        #region methods

        public void OnMessage(int eventMessage, zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnMessage, new CallValue[] { (IntArg)eventMessage, vob });
        }

        #endregion

    }
}
