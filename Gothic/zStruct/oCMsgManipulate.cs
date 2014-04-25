using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zStruct
{
    public class oCMsgManipulate : Gothic.zClasses.zClass
    {
        public enum Offsets
        {
            InstanceName = 68, //ItemName zString
            SlotName = 88, //ZS_LEFTHAND SlotName zString

        }
        public oCMsgManipulate()
        { }
        public oCMsgManipulate(Process process, int address)
            : base(process, address)
        {

        }


        public zString InstanceName { get { return new zString(Process, Address + (int)Offsets.InstanceName); } }
        public zString SlotName { get { return new zString(Process, Address + (int)Offsets.SlotName); } }

    }
}
