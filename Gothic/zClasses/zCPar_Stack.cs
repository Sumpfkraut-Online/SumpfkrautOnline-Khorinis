using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCPar_Stack : zClass
    {
        public zCPar_Stack()
        { }
        public zCPar_Stack(Process process, int address)
            : base(process, address)
        {

        }

        public enum Offsets
        {
            begin = 0,
            StackPos = 4,
            Size = 12
        }
        public enum FuncOffsets
        {
            PushString = 0x007A4BB0
        }

        #region Fields
        public int Begin
        {
            get { return Process.ReadInt(Address + (int)Offsets.begin); }
        }
        public int StackPos
        {
            get { return Process.ReadInt(Address + (int)Offsets.StackPos); }
        }

        public int Size
        {
            get { return Process.ReadInt(Address + (int)Offsets.Size); }
        }
        #endregion

        #region methods

        public void PushString(zString str)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PushString, new CallValue[] { str });
        }

        #endregion
    }
}
