using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects.Meshes;

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
        new public abstract class VarOffsets : zCAIBase.VarOffsets
        {
            public const int stepHeight = 0x2C,
                waterDepthWade = 0x34,
                waterDepthSwim = 0x38,
                centerPos = 70,
                vob = 0x64,
                feetY = 0x7C,
                headY = 0x80,
                waterLevel = 0x88;
        }

        public zCAIPlayer()
        {

        }

        public zCAIPlayer(int address)
            : base(address)
        {
        }

        public void LandAndStartAni(string aniName)
        {
            using (zString z = zString.Create(aniName))
                this.LandAndStartAni(z);
        }

        public void LandAndStartAni(zString aniName)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x511B10, aniName);
        }

        public void LandAndStartAni(zCModelAni ani)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x511BC0, ani);
        }

        public bool DetectClimbUpLedge(zVec3 resultPos, bool arg)
        {
            return Process.THISCALL<BoolArg>(Address, 0x50FD90, resultPos, (BoolArg)arg);
        }

        public zTLedgeInfo GetLedgeInfo()
        {
            return Process.THISCALL<zTLedgeInfo>(Address, 0x50FCC0);
        }

        public void ClearFoundLedge()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x50FD20);
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


        public int WaterLevel
        {
            get { return Process.ReadInt(Address + VarOffsets.waterLevel); }
        }

        public float FeetY
        {
            get { return Process.ReadFloat(Address + VarOffsets.feetY); }
        }

        public float HeadY
        {
            get { return Process.ReadFloat(Address + VarOffsets.headY); }
        }

        public float WaterDepthSwim
        {
            get { return Process.ReadFloat(Address + VarOffsets.waterDepthSwim); }
            set { Process.Write(value, Address + VarOffsets.waterDepthSwim); }
        }

        public float WaterDepthWade
        {
            get { return Process.ReadFloat(Address + VarOffsets.waterDepthWade); }
            set { Process.Write(value, Address + VarOffsets.waterDepthWade); }
        }

        public float StepHeight
        {
            get { return Process.ReadFloat(Address + VarOffsets.stepHeight); }
            set { Process.Write(value, Address + VarOffsets.stepHeight); }
        }
    }
}
