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
            OnMessage = 0x00786380,
            KillMessages = 0x00786320,
            ProcessList = 0x787000,
            InsertInList = 0x787300,
            DoFrameActivity = 0x785F70,
            GetActiveMsg = 0x787810
        }

        #endregion

        public int OwnerAddress
        {
            get { return Process.ReadInt(Address + 0x3C); }
        }

        #region methods

        public void OnMessage(zCEventMessage eventMessage, zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.OnMessage, new CallValue[] { eventMessage, vob });
        }

        public void KillMessages()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.KillMessages, new CallValue[] { });
        }

        public void ProcessMessageList()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.ProcessList, new CallValue[] { });
        }

        public void DoFrameActivity()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DoFrameActivity, new CallValue[] { });
        }

        public void InsertInList(zCEventMessage msg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.InsertInList, new CallValue[] { msg });
        }


        public zCEventMessage GetActiveMsg()
        {
            int addr = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetActiveMsg, null);
            return new zCEventMessage(Process, addr);
        }

        #endregion


        public override uint ValueLength()
        {
            return 4;
        }
    }
}
