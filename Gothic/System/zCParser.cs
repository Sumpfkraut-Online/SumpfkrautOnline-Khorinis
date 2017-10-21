using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.System
{
    public class zCParser : zClass
    {
        public const int camParser = 0x8CEAC8;

        public zCParser()
        {
        }

        public zCParser(int address) : base(address)
        {
        }

        public static zCParser GetCameraParser()
        {
            return new zCParser(Process.ReadInt(camParser));
        }

        public bool LoadDat(string name)
        {
            bool result;
            using (zString z = zString.Create(name))
                result = LoadDat(z);
            return result;
        }

        public void Reset()
        {
            Process.THISCALL<IntArg>(Address, 0x793100);
        }

        public bool LoadDat(zString name)
        {
            return Process.THISCALL<IntArg>(Address, 0x78E900, name) == 0;
        }

        public int GetNumSymbols()
        {
            return Process.ReadInt(Address + 0x10 + 0x10);
        }
    }
}
