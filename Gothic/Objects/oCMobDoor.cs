using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCMobDoor : oCMobLockable
    {
        new public abstract class FuncAddresses : oCMobLockable.FuncAddresses
        {
            public const int Open = 0x0071A430,
            Close = 0x0071A440;
        }

        public oCMobDoor()
        {
        }

        public oCMobDoor(int address)
            : base(address)
        {
        }

        new public static oCMobDoor Create()
        {
            int address = Process.CDECLCALL<IntArg>(0x71A250); //_CreateInstance()
            Process.THISCALL<NullReturnCall>(address, 0x7269B0); //Konstruktor...
            return new oCMobDoor(address);
        }
    }
}
