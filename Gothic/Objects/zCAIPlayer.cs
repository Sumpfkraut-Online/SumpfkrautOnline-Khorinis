using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class zTLedgeInfo : zClass
    {
        public const int ByteSize = 40;

        public zTLedgeInfo()
        {
        }

        public zTLedgeInfo(int address)
            : base(address)
        {
        }

        public zVec3 Position
        {
            get { return new zVec3(Address); }
        }

        public zVec3 Normal
        {
            get { return new zVec3(Address + 12); }
        }

        public zVec3 Cont
        {
            get { return new zVec3(Address + 24); }
        }

        public float MaxMoveForward
        {
            get { return Process.ReadFloat(Address + 36); }
            set { Process.Write(value, Address + 36); }
        }
    }

    public class zCAIPlayer : zCAIBase
    {
        public zCAIPlayer()
        {

        }

        public zCAIPlayer(int address)
            : base(address)
        {
        }

        public bool CheckEnoughSpaceMoveForward(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x511700, (BoolArg)arg);
        }
        
        public bool CheckEnoughSpaceMoveBackward(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x511740, (BoolArg)arg);
        }

        public bool CheckEnoughSpaceMoveLeft(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x5117D0, (BoolArg)arg);
        }

        public bool CheckEnoughSpaceMoveRight(bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x511790, (BoolArg)arg);
        }

        public zTLedgeInfo GetLedgeInfo()
        {
            return Process.THISCALL<zTLedgeInfo>(Address, 0x50FCC0);
        }

        public int WaterLevel
        {
            get { return Process.ReadInt(Address + 0x88); }
        }

        public float FeetY
        {
            get { return Process.ReadFloat(Address + 0x7C); }
        }
    }
}
