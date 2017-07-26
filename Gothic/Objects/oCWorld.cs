using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCWorld : zCWorld
    {
        public oCWorld()
        {
        }

        public oCWorld(int address) : base(address)
        {
        }
        new public abstract class FuncAddresses : zCWorld.FuncAddresses
        {
            new public const int RemoveVob = 0x007800C0;
            public const int InsertVobInWorld = 0x00780330,
            DisableVob = 0x00780460;
        }

        new public const int ByteSize = 0x628C;

        new public static oCWorld Create()
        {
            return new oCWorld(Process.CDECLCALL<IntArg>(0x77ED20));
        }

        public void InsertVobInWorld(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.InsertVobInWorld, vob);
        }

        public override void RemoveVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.RemoveVob, vob);
        }

        public void DisableVob(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.DisableVob, vob);
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x77EFB0, (BoolArg)true);
        }
    }
}
