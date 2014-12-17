using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Args
{ 
    public class FloatRefArg : CallValue
    {
        
        public FloatRefArg(Process process, float value)
        {
            Process = process;
            byte[] arr = BitConverter.GetBytes(value);
            Address = process.Alloc(4).ToInt32();

            process.Write(arr, Address);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public float GetFloatValue()
        {
            float val = Process.ReadFloat(Address);
            Process.Free(new IntPtr(Address), 4);
            return val;
        }
    }
}
