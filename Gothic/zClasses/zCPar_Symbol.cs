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

        public enum FuncOffsets
        {
            SetClassOffset = 0x007A2F40,
            LoadFull = 0x007A2BA0,
            SetValue_Str = 0x007A1E90,
        }


        public zCPar_Symbol(Process process, int address)
            : base(process, address)
        {

        }

        public zCPar_Symbol()
        {

        }

        public void SetValue(zString str, int id)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetValue_Str, new CallValue[] { str, new IntArg(id) });
        }

        public void LoadFull(zFile file)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.LoadFull, new CallValue[] { file });
        }

        public void SetClassOffset(int offset)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetClassOffset, new CallValue[] { new IntArg(offset) });
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
