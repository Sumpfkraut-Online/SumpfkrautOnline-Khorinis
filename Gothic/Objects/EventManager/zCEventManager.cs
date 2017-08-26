using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects.EventManager
{
    public class zCEventManager : zClass
    {
        public zCEventManager()
            : base()
        {

        }

        public zCEventManager(int address)
            : base(address)
        {

        }

        public abstract class FuncAddresses
        {
            public const int OnMessage = 0x00786380,
            KillMessages = 0x00786320,
            ProcessList = 0x787000,
            InsertInList = 0x787300,
            DoFrameActivity = 0x785F70,
            GetActiveMsg = 0x787810;
        }
        
        public static bool DisableEventManagers
        {
            get { return Process.ReadBool(0xAB39C8); }
            set { Process.Write(0xAB39C8, value); }
        }

        public int OwnerAddress
        {
            get { return Process.ReadInt(Address + 0x3C); }
        }

        public void OnMessage(zCEventMessage eventMessage, zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.OnMessage, eventMessage, vob);
        }

        public void KillMessages()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.KillMessages);
        }

        public void ProcessMessageList()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.ProcessList);
        }

        public void DoFrameActivity()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DoFrameActivity);
        }

        public void InsertInList(zCEventMessage msg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.InsertInList, msg);
        }

        public zCEventMessage GetActiveMsg()
        {
            return Process.THISCALL<zCEventMessage>(Address, FuncAddresses.GetActiveMsg);
        }
    }
}
