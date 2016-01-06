using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class BoolArg : CallValue
    {
        public bool Value { get; protected set; }

        public BoolArg()
        {
        }

        public BoolArg(bool val)
        {
            Value = val;
        }

        public override List<byte[]> GetCallParams()
        {
            return new List<byte[]>() { new byte[4] { Convert.ToByte(Value), 0, 0, 0 } };
        }

        public override void Initialize(int registerAddress)
        {
            Value = Process.ReadBool(registerAddress);
        }

        public override uint ValueLength()
        {
            return 4;
        }

        public static implicit operator BoolArg(bool value)
        {
            return new BoolArg(value);
        }

        public static implicit operator bool(BoolArg value)
        {
            return value.Value;
        }
    }
}
