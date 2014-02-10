using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Args;

namespace Gothic.zClasses
{
    public class zCInput : zClass
    {
        public zCInput()
        {

        }

        public zCInput(Process process, int address)
            : base(process, address)
        {

        }

        public enum FuncOffsets : uint
        {
            GetMousePosition = 0x004CBCF0
        }

        public float[] GetMousePosition()
        {
            float[] fl = new float[3];
            FloatRefArg f1 = new FloatRefArg(Process, 0); FloatRefArg f2 = new FloatRefArg(Process, 0); FloatRefArg f3 = new FloatRefArg(Process, 0);
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.GetMousePosition, new CallValue[] { f1, f2, f3 });
            fl[0] = f1.GetFloatValue();
            fl[1] = f2.GetFloatValue();
            fl[2] = f3.GetFloatValue();

            return fl;
        }

        public static zCInput GetInput(Process process)
        {
            return new zCInput(process, process.ReadInt(0x008D1650));
        }
    }
}
