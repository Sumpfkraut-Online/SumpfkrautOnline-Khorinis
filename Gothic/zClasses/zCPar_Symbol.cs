using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCPar_Symbol : zClass
    {
        public enum Offsets
        {
            name = 0,
            offset = 0x1C,
            content = 0x18
        }
        public zCPar_Symbol(Process process, int address)
            : base(process, address)
        {

        }

        public zCPar_Symbol()
        {

        }

        public zString Name
        {
            get{ return new zString(Process, Address);}
        }

        public int Offset
        {
            get { return Process.ReadInt(Address + (int)Offsets.offset); }
            set { Process.Write(value, Address + (int)Offsets.offset); }
        }

        public int Content
        {
            get { return Process.ReadInt(Address + (int)Offsets.content); }
            set { Process.Write(value, Address + (int)Offsets.content); }
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public int GetOffset()
        {
            return Process.THISCALL<IntArg>((uint)Address, 0x007A2F30, new CallValue[] { }).Address;
        }

    }
}
