using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

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

        public zMat4 NewTrafo
        {
            get { return new zMat4(Address + 0x44); }
        }

        public bool TrafoHintLocation
        {
            get { return Process.ReadBit(Address + 0x88, 0); }
            set { Process.WriteBit(Address + 0x88, 0, value); }
        }

        public bool TrafoHintRotation
        {
            get { return Process.ReadBit(Address + 0x88, 1); }
            set { Process.WriteBit(Address + 0x88, 1, value); }
        }
    }
}
