using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zStruct;

namespace Gothic.zClasses
{
    public class oCCSManager : zClass
    {
        public oCCSManager()
        {

        }

        public zCEventMessage CreateMessage(int type)
        {
            return Process.THISCALL<zCEventMessage>((uint)Address, (uint)0x402420, new CallValue[] { (IntArg)type });
        }

        public oCCSManager(Process process, int address)
            : base(process, address)
        {
            
        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
