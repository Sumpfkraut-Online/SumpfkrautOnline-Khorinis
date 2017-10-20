using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class FloatArg : CallValue
    {
        public float Value { get; protected set; }

        public FloatArg()
        {
        }

        public FloatArg(float val)
        {
            Value = val;
        }

        public override List<byte[]> GetCallParams()
        {
            return new List<byte[]>() { BitConverter.GetBytes(Value) };
        }

        public override void Initialize(int registerAddress)
        {
            Value = Process.ReadFloat(registerAddress);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public static implicit operator FloatArg(float value)
        {
            return new FloatArg(value);
        }

        public static implicit operator float(FloatArg value)
        {
            return value.Value;
        }
    }
}
