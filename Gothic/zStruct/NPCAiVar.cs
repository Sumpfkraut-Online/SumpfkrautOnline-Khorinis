using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zStruct
{
    public class NPCAiVar :zClass
    {
        public NPCAiVar() { }
        public NPCAiVar(Process process, int address)
            : base(process, address)
        {

        }

        public enum Monster_AiVar : uint
        {

        }

        public enum Human_AiVar : uint
        {
            AIV_PARTYMEMBER = 15,
            AIV_VictoryXPGiven = 16
        }

    }
}
