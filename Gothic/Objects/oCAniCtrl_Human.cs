using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects
{
    public class oCAniCtrl_Human : zCAIPlayer
    {
        new public abstract class VarOffsets : zCObject.VarOffsets
        {
            public const int ComboNr = 0x1B4,
            HitTarget = 0x1C4,
            Bitfield = 0x1B0,

            _t_walk_2_aim = 0x10E8,
            _s_aim = 0x10EC,
            _t_aim_2_walk = 0x10F0,

            _t_hitl = 0x10F4,
            _t_hitr = 0x10F8,
            _t_hitf = 0x1100,
            _t_hitfrun = 0x1124,

            _s_walk = 0x1008,
            _s_walkl = 0x1014,
            _t_turnl = 0x1028,
            _t_turnr = 0x102C,

            _s_dive = 0x109C,
            _t_diveturnl = 0x10D8,
            _t_diveturnr = 0x10DC,

            _s_swim = 0x10B4,
            _t_swimturnl = 0x10D0,
            _t_swimturnr = 0x10D4,

            _t_strafel = 0x1030,
            _t_strafer = 0x1034,

            _t_runl_2_jump = 0x118C,
            _t_runr_2_jump = 0x1190,
            _t_jump_2_runl = 0x1194,
            _t_jump_2_stand = 0x106C,

            actionMode = 0x14C,

            wmode_last = 0x154;
        }

        public abstract class FuncAddresses //: zCObject.FuncAddresses
        {
            public const int GetWalkModeString = 0x006AAE40,
            _Forward = 0x006B7900,
            _Stand = 0x006B7490,
            _Backward = 0x006B7BC0,
            SetWalkMode = 0x006A9820,
            SetAlwaysWalk = 0x006ABDB0,
            ToggleWalkMode = 0x006AD500,
            IsStanding = 0x6ADEE0,
            IsRunning = 0x6AE200,
            JumpForward = 0x6B21E0,
            PC_JumpForward = 0x6B1E00,
            PC_GoForward = 0x6B1D70,
            PC_GoBackward = 0x6B1DC0,

            CreateHit = 0x6B0830,

            Reset = 0x006A5080,

            RemoveWeapon2 = 0x006B33B0,
            SearchStandAni = 0x006A4D20,
            CheckMeleeWeaponHitsLevel = 0x6B0CD0,
            GetFightLimbs = 0x6AF1E0,

            HitCombo = 0x6B0260,
            SetFightAnis = 0x6AAA40;
        }

        public abstract class BitFlag
        {
            public const int canEnableNextCombo = 1;
            public const int endCombo = 2;
            public const int comboCanHit = 4;
            public const int hitPosUsed = 8;
            public const int hitGraphical = 16;
            public const int canDoCollisionFX = 32;
        }

        public oCAniCtrl_Human()
        {

        }

        public oCAniCtrl_Human(int address)
            : base(address)
        {
        }

        public override void Dispose()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6A3C50, (BoolArg)true);
        }

        public int _t_hitfrun { get { return Process.ReadInt(Address + VarOffsets._t_hitfrun); } }
        public int _t_hitf { get { return Process.ReadInt(Address + VarOffsets._t_hitf); } }
        public int _t_hitr { get { return Process.ReadInt(Address + VarOffsets._t_hitr); } }
        public int _t_hitl { get { return Process.ReadInt(Address + VarOffsets._t_hitl); } }

        public int _s_walkl { get { return Process.ReadInt(Address + VarOffsets._s_walkl); } }
        public int _s_walk { get { return Process.ReadInt(Address + VarOffsets._s_walk); } }
        public int _t_turnr { get { return Process.ReadInt(Address + VarOffsets._t_turnr); } }
        public int _t_turnl { get { return Process.ReadInt(Address + VarOffsets._t_turnl); } }

        public int _s_dive { get { return Process.ReadInt(Address + VarOffsets._s_dive); } }
        public int _t_diveturnr { get { return Process.ReadInt(Address + VarOffsets._t_diveturnr); } }
        public int _t_diveturnl { get { return Process.ReadInt(Address + VarOffsets._t_diveturnl); } }

        public int _s_swim { get { return Process.ReadInt(Address + VarOffsets._s_swim); } }
        public int _t_swimturnr { get { return Process.ReadInt(Address + VarOffsets._t_swimturnr); } }
        public int _t_swimturnl { get { return Process.ReadInt(Address + VarOffsets._t_swimturnl); } }

        public int _t_strafer { get { return Process.ReadInt(Address + VarOffsets._t_strafer); } }
        public int _t_strafel { get { return Process.ReadInt(Address + VarOffsets._t_strafel); } }

        public int _t_runr_2_jump { get { return Process.ReadInt(Address + VarOffsets._t_runr_2_jump); } }
        public int _t_runl_2_jump { get { return Process.ReadInt(Address + VarOffsets._t_runl_2_jump); } }
        public int _t_jump_2_runl { get { return Process.ReadInt(Address + VarOffsets._t_jump_2_runl); } }
        public int _t_jump_2_stand { get { return Process.ReadInt(Address + VarOffsets._t_jump_2_stand); } }

        public int wmode_last { get { return Process.ReadInt(Address + VarOffsets.wmode_last); } }
        public int actionMode { get { return Process.ReadInt(Address + VarOffsets.actionMode); } }

        public int HitTarget
        {
            get { return Process.ReadInt(Address + VarOffsets.HitTarget); }
            set { Process.Write(Address + VarOffsets.HitTarget, value); }
        }

        public int AniCtrlBitfield
        {
            get { return Process.ReadInt(Address + VarOffsets.Bitfield); }
            set { Process.Write(Address + VarOffsets.Bitfield, value); }
        }

        public float LookTargetX
        {
            get { return Process.ReadFloat(Address + 0x17C); }
            set { Process.Write(Address + 0x17C, value); }
        }

        public float LookTargetY
        {
            get { return Process.ReadFloat(Address + 0x180); }
            set { Process.Write(Address + 0x180, value); }
        }

        public void LookAtTarget()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6B62F0);
        }

        public void SetLookAtTarget(float x, float y)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6B6310, (FloatArg)x, (FloatArg)y);
        }

        public int RemoveWeapon2()
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.RemoveWeapon2);
        }

        public void ShowWeaponTrail()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6AFE70);
        }

        public void SearchStandAni(bool force = false)
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.SearchStandAni, new BoolArg(force));
        }

        public void StopTurnAnis()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6AE530);
        }

        public void Reset()
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.Reset);
        }

        public void SetAlwaysWalk(int x)
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.SetAlwaysWalk, new IntArg(x));
        }

        public void ToggleWalkMode(int x)
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.ToggleWalkMode, new IntArg(x));
        }

        public void ToggleWalkMode(char x)
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.ToggleWalkMode, new CharArg(x));
        }

        public void SetWalkMode(int x)
        {
            Process.THISCALL<IntArg>(Address, FuncAddresses.SetWalkMode, new IntArg(x));
        }

        public void _Forward()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses._Forward);
        }

        public void _Stand()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses._Stand);
        }

        public void _Backward()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses._Backward);
        }

        public void JumpForward()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.JumpForward);
        }

        public void PC_JumpForward()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PC_JumpForward);
        }

        public void PC_GoForward()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PC_GoForward);
        }

        public void PC_GoBackward()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.PC_GoBackward);
        }

        public void CreateHit(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.CreateHit, vob);
        }

        public int HitCombo(int DoCombo)
        {
            return Process.THISCALL<IntArg>(Address, FuncAddresses.HitCombo, (IntArg)DoCombo);
        }

        public bool IsStanding()
        {
            return Process.THISCALL<BoolArg>(Address, FuncAddresses.IsStanding);
        }

        public bool IsRunning()
        {
            return Process.THISCALL<BoolArg>(Address, FuncAddresses.IsRunning);
        }

        public bool CanParade(oCNpc npc)
        {
            return Process.THISCALL<IntArg>(Address, 0x6B15B0, npc) > 0;
        }

        public void StartParadeEffects(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6B16F0, npc);
        }

        public void StartFallDownAni()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6B5220);
        }

        public void StopLookAtTarget()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6B6640);
        }

        public void StartStandAni()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6A5060);
        }

        public void StartAni(int ani, int nextAni)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6A3DC0);
        }

        public bool CheckMeleeWeaponHitsLevel(oCItem item)
        {
            return Process.THISCALL<BoolArg>(Address, FuncAddresses.CheckMeleeWeaponHitsLevel, item);
        }

        public void GetFightLimbs()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.GetFightLimbs);
        }

        public void CorrectFightPosition()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6AAB80);
        }

        public void Turn(float amount, bool playAni)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6AE540, new FloatArg(amount), new BoolArg(playAni));
        }

        public void SetFightAnis(int fmode)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetFightAnis, new IntArg(fmode));
        }

        /*public zString GetWalkModeZString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>(Address, FuncAddresses.GetWalkModeString, new CallValue[] { new IntArg(str) });
            return new zString(Process, arg.Address);
        }

        public String GetWalkModeString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>(Address, FuncAddresses.GetWalkModeString, new CallValue[] { new IntArg(str) });
            zString zString = new zString(Process, arg.Address);
            String v = null;
            if (zString.Length < 500)
                v = zString.Value.Trim();
            zString.Dispose();

            return v;
        }*/

    }
}
