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
            set { Process.Write(Address + 36, value); }
        }
    }

    public class zCAIPlayer : zCAIBase
    {
        new public abstract class VarOffsets : zCAIBase.VarOffsets
        {
            public const int stepHeight = 0x2C,
                jumpUpMinCeil = 0x30,
                waterDepthWade = 0x34,
                waterDepthSwim = 0x38,
                yMaxSwimClimbOut = 0x3C,
                forceJumpUp = 0x40,
                yMaxJumpLow = 0x44,
                yMaxJumpMid = 0x48,
                centerPos = 70,
                vob = 0x64,
                feetY = 0x7C,
                headY = 0x80,
                waterLevel = 0x88,
                bitfield = 0xB8;
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

        public void Begin(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x50E750, vob);
        }

        public void End()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x50E8F0);
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
            set { Process.Write(Address + VarOffsets.waterDepthSwim, value); }
        }

        public float WaterDepthWade
        {
            get { return Process.ReadFloat(Address + VarOffsets.waterDepthWade); }
            set { Process.Write(Address + VarOffsets.waterDepthWade, value); }
        }
        
        public float StepHeight
        {
            get { return Process.ReadFloat(Address + VarOffsets.stepHeight); }
            set { Process.Write(Address + VarOffsets.stepHeight, value); }
        }

        public float ForceJumpUp
        {
            get { return Process.ReadFloat(Address + VarOffsets.forceJumpUp); }
            set { Process.Write(Address + VarOffsets.forceJumpUp, value); }
        }

        public float JumpUpMinCeil
        {
            get { return Process.ReadFloat(Address + VarOffsets.jumpUpMinCeil); }
            set { Process.Write(Address + VarOffsets.jumpUpMinCeil, value); }
        }

        public float YMaxJumpLow
        {
            get { return Process.ReadFloat(Address + VarOffsets.yMaxJumpLow); }
            set { Process.Write(Address + VarOffsets.yMaxJumpLow, value); }
        }

        public float YMaxJumpMid
        {
            get { return Process.ReadFloat(Address + VarOffsets.yMaxJumpMid); }
            set { Process.Write(Address + VarOffsets.yMaxJumpMid, value); }
        }

        public float MaxGroundAngleWalk
        {
            get { return Process.ReadFloat(Address + 0x4C); }
            set { Process.Write(Address + 0x4C, value); }
        }

        public float MaxGroundAngleSlide2
        {
            get { return Process.ReadFloat(Address + 0x54); }
            set { Process.Write(Address + 0x54, value); }
        }

        [Flags]
        public enum Flags
        {
            DetectWalkChasm = 1 << 2,
            WallSliding = 1 << 3,
            CorrectHeight = 1 << 4,
            CDStatOriginal = 1 << 5,
            CDStatCurrent = 1 << 6,
            LandDust = 1 << 7,
            ForceModelHalt = 1 << 8,
        }

        public Flags Bitfield0
        {
            get { return (Flags)Process.ReadInt(Address + VarOffsets.bitfield); }
            set { Process.Write(Address + VarOffsets.bitfield, (int)value); }
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x50C740, new BoolArg(true));
        }

        public zVec3 Velocity
        {
            get { return new zVec3(Address + 0x90); }
        }

        public void CalcForceModelHalt()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x50E390);
        }
    }
}
