using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zStruct;

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

        public void OnMessage(zCEventMessage eventMessage, zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnMessage, new CallValue[] { eventMessage, vob });
        }

        #endregion


        public override uint ValueLength()
        {
            return 4;
        }
    }
}
