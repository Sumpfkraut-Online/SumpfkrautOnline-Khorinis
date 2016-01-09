using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic
{
    public abstract class zClass : CallValue
    {
        public int Address { get; protected set; }

        public zClass()
        {
        }

        public zClass(int address)
        {
            this.Address = address;
        }

        public override List<byte[]> GetCallParams()
        {
            return new List<byte[]>() { BitConverter.GetBytes(Address) };
        }

        public override void Initialize(int registerAddress)
        {
            this.Address = Process.ReadInt(registerAddress);
        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
