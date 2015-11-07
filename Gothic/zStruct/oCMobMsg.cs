using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace Gothic.zStruct
{
    public class oCMobMsg : zCEventMessage
    {
        public oCMobMsg()
            : base()
        {

        }

        public oCMobMsg(Process process, int address)
            : base(process, address)
        {

        }


        public enum SubTypes
        {
            EV_StartInteraction,
            EV_StartStateChange,
            EV_EndInteraction,
            EV_Unlock,
            EV_Lock,
            EV_CallScript
        }

        public static oCMobMsg Create(Process process, SubTypes subType, oCNpc npc, int arg1)
        {
            int address = process.CDECLCALL<IntArg>(0x71B030, null);
            process.THISCALL<NullReturnCall>((uint)address, 0x0071B4D0, new CallValue[] { (IntArg)(int)subType, npc, (IntArg)arg1 });
            return new oCMobMsg(process, address);
        }

        public static oCMobMsg Create(Process process, SubTypes subType, oCNpc npc)
        {
            int address = process.CDECLCALL<IntArg>(0x71B030, null);
            process.THISCALL<NullReturnCall>((uint)address, 0x0071B220, new CallValue[] { (IntArg)(int)subType, npc });
            return new oCMobMsg(process, address);
        }

        public SubTypes SubType
        {
            get { return (SubTypes)Process.ReadUShort(Address + 0x24); }
        }

        public int UserAddress
        {
            get { return Process.ReadInt(Address + 0x2C); }
        }

        //no idea what this address really is, but it's an indicator
        public bool StateChangeLeaving
        {
            get { return Process.ReadInt(Address + 0x30) != 0; }
            set { Process.Write(value ? 1 : 0, Address + 0x30); }
        }

    }
}
