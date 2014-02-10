using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCClassDef : zClass
    {
        public static void ObjectCreated(Process process, int ptrObj, int ptrClassDef)
        {
            process.CDECLCALL<NullReturnCall>(0x005AAEB0, new CallValue[] { new IntArg(ptrObj), new IntArg(ptrClassDef) });
        }

        public enum Offsets
        {
            ObjectList = 92,
            HashMap = 88
        }


        public zCClassDef(Process process, int address)
            : base(process, address)
        {

        }

        public zTypes.zCArray<zCObject> ObjectList
        {
            get { return new zTypes.zCArray<zCObject>(Process, Address + (int)Offsets.ObjectList); }
        }

        public int HashMap
        {
            get { return Process.ReadInt(Address + (int)Offsets.HashMap); }
        }

    }
}
