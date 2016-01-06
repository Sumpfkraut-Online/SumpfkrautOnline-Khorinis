using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class IntArg : CallValue
    {
        public int Value { get; protected set; }

        public IntArg()
        {
        }

        public IntArg(int val)
        {
            Value = val;
        }

        public override List<byte[]> GetCallParams()
        {
            return new List<byte[]>() { BitConverter.GetBytes(Value) };
        }

        public override void Initialize(int registerAddress)
        {
            Value = Process.ReadInt(registerAddress);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public static implicit operator IntArg(int value)
        {
            return new IntArg(value);
        }

        public static implicit operator int(IntArg value)
        {
            return value.Value;
        }
    }
}
