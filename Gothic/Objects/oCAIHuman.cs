using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class oCAIHuman : oCAniCtrl_Human
    {
        new public abstract class VarOffsets : oCAniCtrl_Human.VarOffsets
        {
            public const int NPC = 0x12C,
            fallDownDistanceY = 156,
            waterLevel = 136,
            aboveFloor = 0x84,
            wmode = 336;
        }
        new public abstract class FuncAddresses : oCAniCtrl_Human.FuncAddresses
        {
            public const int StopTurnAnis = 0x006AE530,
            DoSimpleAi = 0x006953B0,
            DoAi = 0x0069BB63,
            Moving = 0x0069B9B0, //Protected
            Init = 0x00695390,
            InitAnimations = 0x006A4010,
            InitAllAnis = 0x006A5BF0,
            PC_Turnings = 0x0069A940,
            StartFlyDamage = 0x69D940,
            CheckFocusVob = 0x0069B7A0;
        }

        public oCAIHuman()
        {

        }

        public oCAIHuman(int address)
            : base(address)
        {
        }
        
        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x695290, (BoolArg)true);
        }

        public oCNpc NPC
        {
            get { return new oCNpc(Process.ReadInt(Address + VarOffsets.NPC)); }
            set { Process.Write(Address + VarOffsets.NPC, value.Address); }
        }

        public int WMode
        {
            get { return Process.ReadInt(Address + VarOffsets.wmode); }
            set { Process.Write(Address + VarOffsets.wmode, value); }
        }

        public float FallDownDistanceY
        {
            get { return Process.ReadFloat(Address + VarOffsets.fallDownDistanceY); }
            set { Process.Write(Address + VarOffsets.fallDownDistanceY, value); }
        }

        public float AboveFloor
        {
            get { return Process.ReadFloat(Address + VarOffsets.aboveFloor); }
        }

        public void Init(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Init, npc);
        }

        public void InitAnimations()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.InitAnimations);
        }

        public void InitAllAnis()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.InitAllAnis);
        }

        public void PC_WeaponMove(int arg)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x69A0E0, (IntArg)arg);
        }

        public void PC_Turnings(int arg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PC_Turnings, (IntArg)arg);
        }

        public void Destroy()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x00695290, new IntArg(1));
        }

        public void StartFlyDamage(float arg1, zVec3 arg2)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.StartFlyDamage, new FloatArg(arg1), arg2);
        }

        public void Moving()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Moving);
        }

        public void CheckFocusVob(int arg)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CheckFocusVob, (IntArg)arg);
        }
    }
}
