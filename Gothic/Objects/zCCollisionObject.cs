using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class zCCollisionObject : zCObject
    {
        public zCCollisionObject(int address)
            : base(address)
        {
        }

        public zCCollisionObject()
        {
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x54D230, new BoolArg(true));
        }

        public float GroundLevel
        {
            get { return Process.ReadFloat(Address + 192); }
        }

        public float WaterLevel
        {
            get { return Process.ReadFloat(Address + 196); }
        }
    }
}
