using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zPoint3 : zClass
    {
        public const int ByteSize = 12;

        public zPoint3(int address)
            : base(address)
        {
            
        }

        public zPoint3()
        {

        }

        public float X 
        {
            get { return Process.ReadFloat(Address); }
            set { Process.Write(Address, value); }
        }

        public float Y
        {
            get { return Process.ReadFloat(Address + 4); }
            set { Process.Write(Address + 4, value); }
        }

        public float Z
        {
            get { return Process.ReadFloat(Address + 8); }
            set { Process.Write(Address + 8, value); }
        }
    }
}
