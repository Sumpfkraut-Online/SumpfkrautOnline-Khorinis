using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApiNew
{
    /*public enum FlagRegister
    {
        CF = 0,
        PF = 2,
        AF = 4,
        ZF = 6,
        SF = 7,
        OF = 11,
    }

    public enum Register
    {
        // reverse pushad order
        EAX,
        ECX,
        EDX,
        EBX,
        ESP,
        EBP,
        ESI,
        EDI,
    }*/

    [StructLayout(LayoutKind.Sequential)]
    public struct RegisterMemory
    {
        // reversed pushad order!
        int stackedi;
        int stackesi;
        int stackebp;
        // esp value from after flags but before registers were pushed
        int stackesp;
        //int stackebx;
        //int stackedx;
        //int stackecx;
        //int stackeax;
        //int stackflags;

        public int ESP { get { return stackesp; } }
        public int EAX { get { return Process.ReadInt(stackesp - 4); } set { Process.WriteInt(stackesp - 4, value); } }
        public int ECX { get { return Process.ReadInt(stackesp - 8); } set { Process.WriteInt(stackesp - 8, value); } }
        public int EDX { get { return Process.ReadInt(stackesp - 12); } set { Process.WriteInt(stackesp - 12, value); } }
        public int EBX { get { return Process.ReadInt(stackesp - 16); } set { Process.WriteInt(stackesp - 16, value); } }
        //public int ESP { get { return Process.ReadInt(stackesp - 20); } set { Process.WriteInt(stackesp - 20, value); } }
        public int EBP { get { return Process.ReadInt(stackesp - 24); } set { Process.WriteInt(stackesp - 24, value); } }
        public int ESI { get { return Process.ReadInt(stackesp - 28); } set { Process.WriteInt(stackesp - 28, value); } }
        public int EDI { get { return Process.ReadInt(stackesp - 32); } set { Process.WriteInt(stackesp - 32, value); } }

        public int GetFlags()
        {
            return Process.ReadInt(stackesp);
        }

        public int GetArg(byte argumentIndex)
        {
            return Process.ReadInt(stackesp + 4 * (argumentIndex + 2)); // + 4 flags + 4 to get arg0
        }

        public void SetArg(byte argumentIndex, int value)
        {
            Process.WriteInt(stackesp + 4 * (argumentIndex + 2), value);
        }
    }
}
