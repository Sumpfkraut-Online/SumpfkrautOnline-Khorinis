using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zStruct
{
    public class TNpcAIState : zClass
    {

        public enum Offsets
        {
            index = 0,
            loop = 4,
            end = 8,
            timeBehavior = 12,
            restTime = 16,
            phase = 20,
            valid = 24,
            name = 28,
            stateTime = 48,
            prgIndex = 52,
            RtnStatus = 56
        }

        public TNpcAIState()
        {

        }

        public TNpcAIState(Process process, int address)
            : base(process, address)
        {

        }


        public int Index
        {
            get { return Process.ReadInt(Address + (int)Offsets.index); }
            set { Process.Write(value, Address + (int)Offsets.index); }
        }
        public int Valid
        {
            get { return Process.ReadInt(Address + (int)Offsets.valid); }
            set { Process.Write(value, Address + (int)Offsets.valid); }
        }
    }
}
