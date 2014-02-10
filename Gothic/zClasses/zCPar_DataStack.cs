using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCPar_DataStack : zClass
    {
        public zCPar_DataStack(Process process, int address)
            : base(process, address)
        {
        }

        public zCPar_DataStack() { }



        #region OffsetList

        public enum FuncOffsets
        {
            Push = 0x007A4F80
        }

        #endregion


        public void Push(int x)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Push, new CallValue[]{(IntArg)x});
        }
    }
}
