using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Args;

namespace Gothic.zClasses
{
    public class zCInput_Win32 : zCInput
    {
        public zCInput_Win32()
        {

        }

        public zCInput_Win32(Process process, int address)
            : base(process, address)
        {

        }


        #region OffsetLists
        public enum Offsets : uint
        {
        }

        public enum FuncOffsets : uint
        {
            SetDeviceEnabled = 0x004D5100,
            GetMousePosition = 0x004D5730
        }

        public enum HookSize : uint
        {

        }
        #endregion



        #region methods
        //2=maus
        public void SetDeviceEnabled(int inputdevice, int b)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetDeviceEnabled, new CallValue[] { new IntArg(inputdevice), new IntArg(b) });
        }

        public float[] GetMousePosition()
        {
            float[] fl = new float[3];
            FloatRefArg f1 = new FloatRefArg(Process, 0); FloatRefArg f2 = new FloatRefArg(Process, 0); FloatRefArg f3 = new FloatRefArg(Process, 0);
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.GetMousePosition, new CallValue[] { f1,f2,f3 });
            fl[0] = f1.GetFloatValue();
            fl[1] = f2.GetFloatValue();
            fl[2] = f3.GetFloatValue();

            return fl;
        }

        #endregion

        public static zCInput_Win32 GetInput(Process process)
        {
            return new zCInput_Win32(process, process.ReadInt(0x008D1650));
        }
    }
}
