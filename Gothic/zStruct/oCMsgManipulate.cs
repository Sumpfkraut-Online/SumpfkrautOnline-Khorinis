using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;
using Gothic.zClasses;

namespace Gothic.zStruct
{
    public class oCMsgManipulate : Gothic.zClasses.zClass
    {
        //VBTL => 8641948



        public enum Offsets
        {
            //60 => ?
            InstanceName = 68, //ItemName zString
            SlotName = 88, //ZS_LEFTHAND SlotName zString
            Item = 108
        }
        public oCMsgManipulate()
        { }
        public oCMsgManipulate(Process process, int address)
            : base(process, address)
        {

        }

        public oCItem Item { get { return new oCItem(Process, Process.ReadInt(Address + (int)Offsets.Item)); } }
        public zString InstanceName { get { return new zString(Process, Address + (int)Offsets.InstanceName); } }
        public zString SlotName { get { return new zString(Process, Address + (int)Offsets.SlotName); } }

    }
}
