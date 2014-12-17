using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace Gothic.zTypes
{
    public class zPoint3 : zClass
    {
        public zPoint3(Process process, int address)
            : base(process, address)
        {
            
        }

        public zPoint3()
        {

        }

        public float X 
        {
            get { return Process.ReadFloat(Address); }
            set { Process.Write(value, Address); }
        }

        public float Y
        {
            get { return Process.ReadFloat(Address + 4); }
            set { Process.Write(value, Address + 4); }
        }

        public float Z
        {
            get { return Process.ReadFloat(Address + 8); }
            set { Process.Write(value, Address + 8); }
        }
    }
}
