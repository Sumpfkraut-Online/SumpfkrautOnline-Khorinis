using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCObject : zClass
    {
        public enum Offsets
        {
            vtbl = 0x00,
            refCtr = 0x04,
            hashIndex= 0x08,
            hashNext = 0x0C,
            objectName = 0x10//string
        }

        public enum ObjectTypes
        {
            ERROR = 0,
            Item = 8636420,
            Npc = 8640292,
            Mob = 8639700,
            MobFire = 8638876,
            Mover = 8627324,
            MobInter = 8639884,
            MobLockable = 8637628,
            MobContainer = 8637284,
            MobDoor = 8638548,
            VobLight = 8624756,
            zString = 8578800,
            oCMsgConversation = 8642060
        }


        public zCObject(Process process, int address)
            : base(process, address)
        { }

        public zCObject()
        {

        }
        public int VTBL
        {
            get { return Process.ReadInt(Address + (int)Offsets.vtbl); }
            set { Process.Write(value ,Address + (int)Offsets.vtbl); }
        }

        public int refCtr
        {
            get { return Process.ReadInt(Address + (int)Offsets.refCtr); }
            set { Process.Write(value,Address + (int)Offsets.refCtr); }
        }

        public zString ObjectName
        {
            get { return new zString(Process, Address + (int)Offsets.objectName); }
        }
    }
}
