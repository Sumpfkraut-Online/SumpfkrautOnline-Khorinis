using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class CharArg : CallValue
    {
        public char Value { get; protected set; }

        public CharArg()
        {
        }

        public CharArg(char val)
        {
            Value = val;
        }

        public override List<byte[]> GetCallParams()
        {
            return new List<byte[]>() { new byte[1] { Convert.ToByte(Value) } };
        }

        public override void Initialize(int registerAddress)
        {
            Value = Convert.ToChar(Process.ReadByte(registerAddress));
        }

        public override uint ValueLength()
        {
            return 1;
        }
    }
}
