using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic
{
    public class zCOptionEntry : zClass
    {
        public zCOptionEntry(int address)
            : base(address)
        {

        }

        public zCOptionEntry()
        {

        }

       
        public abstract class VarOffsets
        {
            public const int VarName = 16,
            VarValue = 36;
        }
        
        public zString VarName
        {
            get { return new zString(Address + VarOffsets.VarName); }
        }

        public zString VarValue
        {
            get { return new zString(Address + VarOffsets.VarValue); }
        }
    }
}
