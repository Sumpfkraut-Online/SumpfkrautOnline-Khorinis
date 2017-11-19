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

        public bool LoadDat(zString name)
        {
            return Process.THISCALL<IntArg>(Address, 0x78E900, name) == 0;
        }

        public bool SaveDat(string name)
        {
            bool result;
            using (zString z = zString.Create(name))
                result = SaveDat(z);
            return result;
        }

        public bool SaveDat(zString name)
        {
            return Process.THISCALL<IntArg>(Address, 0x78E740, name) == 0;
        }

        public bool ParseSource(string name)
        {
            bool result;
            using (zString z = zString.Create(name))
                result = ParseSource(z);
            return result;
        }

        public bool ParseSource(zString name)
        {
            return Process.THISCALL<IntArg>(Address, 0x78EE20, name) == 0;
        }

        public bool ParseFile(string name)
        {
            bool result;
            using (zString z = zString.Create(name))
                result = ParseFile(z);
            return result;
        }

        public bool ParseFile(zString name)
        {
            return Process.THISCALL<IntArg>(Address, 0x78F660, name) == 0;
        }

        public void Reparse(string name)
        {
            using (zString z = zString.Create(name))
                Reparse(z);
        }

        public void Reparse(zString name)
        {
            Process.THISCALL<IntArg>(Address, 0x794C30, name);
        }

        public void Reset()
        {
            Process.THISCALL<IntArg>(Address, 0x793100);
        }

        public int GetNumSymbols()
        {
            return Process.ReadInt(Address + 0x10 + 0x10);
        }
    }
}
