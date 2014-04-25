using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCPar_SymbolTable : zClass
    {
        public enum Offsets
        {
        }

        public enum FuncOffsets
        {
            Load = 0x007A3460
        }


        public zCPar_SymbolTable(Process process, int address)
            : base(process, address)
        {

        }

        public zCPar_SymbolTable()
        {

        }

        public void LoadFull(zFile file)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Load, new CallValue[] { file });
        }




        public override uint ValueLength()
        {
            return 4;
        }
    }
}
