using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class zCAICamera : zCAIBase
    {
        public const int current = 0x8CEAB8;
        public static int CurrentAddress { get { return Process.ReadInt(current); } }
        public static zCAICamera CurrentCam { get { return new zCAICamera(CurrentAddress); } }

        public zCAICamera()
        {
        }

        public zCAICamera(int address) : base(address)
        {
        }
        
        public void CreateInstance(zString instance)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x4A3670, instance);
        }

        public zString CurrentMode
        {
            get { return new zString(Address + 0x26C); }
        }

        public float BestRotX
        {
            get { return Process.ReadFloat(Address + 0x38); }
            set { Process.Write(Address + 0x38, value); }
        }

        public float BestRotY
        {
            get { return Process.ReadFloat(Address + 0x44); }
            set { Process.Write(Address + 0x44, value); }
        }
    }
}
