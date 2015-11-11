using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCAniCtrl_Human : oCAiHuman
    {
        #region OffsetLists
        public enum Offsets
        {
            NPC = 0x12C,
            ComboNr = 0x1B4,
            HitTarget = 0x1C4,
            Bitfield = 0x1B0,

            _t_hitl = 0x10F4,
            _t_hitr = 0x10F8,
            _t_hitf = 0x1100,
            _t_hitfrun = 0x1124,

            _s_walk = 0x1008,
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

            _t_runr_2_jump = 0x1190,

            wmode_last = 0x154


        }
        public enum FuncOffsets : uint
        {
            StopTurnAnis = 0x006AE530,
            GetWalkModeString = 0x006AAE40,
            _Forward = 0x006B7900,
            _Stand = 0x006B7490,
            _Backward = 0x006B7BC0,
            SetWalkMode = 0x006A9820,
            SetAlwaysWalk = 0x006ABDB0,
            ToggleWalkMode = 0x006AD500,
            IsStanding = 0x6ADEE0,
            JumpForward = 0x6B21E0,
            PC_JumpForward = 0x6B1E00,
            PC_GoForward = 0x6B1D70,
            PC_GoBackward = 0x6B1DC0,

            CreateHit = 0x6B0830,

            InitAnimations = 0x006A4010,
            Reset = 0x006A5080,

            RemoveWeapon2 = 0x006B33B0,
            SearchStandAni = 0x006A4D20,

            HitCombo = 0x6B0260
        }

        public enum HookSize : uint
        {
            InitAnimations = 6,
            JumpForward = 6,
            HitCombo = 6
        }

        public struct BitFlag
        {
            public const int canEnableNextCombo = 1;
            public const int endCombo = 2;
            public const int comboCanHit = 4;
            public const int hitPosUsed = 8;
            public const int hitGraphical = 16;
            public const int canDoCollisionFX = 32;
        }

        #endregion

        public oCAniCtrl_Human()
        {

        }

        public oCAniCtrl_Human(Process process, int address)
            : base(process, address)
        {
        }

        public int _t_hitfrun { get { return Process.ReadInt(Address + (int)Offsets._t_hitfrun); } }
        public int _t_hitf { get { return Process.ReadInt(Address + (int)Offsets._t_hitf); } }
        public int _t_hitr { get { return Process.ReadInt(Address + (int)Offsets._t_hitr); } }
        public int _t_hitl { get { return Process.ReadInt(Address + (int)Offsets._t_hitl); } }

        public int _s_walk { get { return Process.ReadInt(Address + (int)Offsets._s_walk); } }
        public int _t_turnr { get { return Process.ReadInt(Address + (int)Offsets._t_turnr); } }
        public int _t_turnl { get { return Process.ReadInt(Address + (int)Offsets._t_turnl); } }

        public int _s_dive { get { return Process.ReadInt(Address + (int)Offsets._s_dive); } }
        public int _t_diveturnr { get { return Process.ReadInt(Address + (int)Offsets._t_diveturnr); } }
        public int _t_diveturnl { get { return Process.ReadInt(Address + (int)Offsets._t_diveturnl); } }

        public int _s_swim { get { return Process.ReadInt(Address + (int)Offsets._s_swim); } }
        public int _t_swimturnr { get { return Process.ReadInt(Address + (int)Offsets._t_swimturnr); } }
        public int _t_swimturnl { get { return Process.ReadInt(Address + (int)Offsets._t_swimturnl); } }

        public int _t_strafer { get { return Process.ReadInt(Address + (int)Offsets._t_strafer); } }
        public int _t_strafel { get { return Process.ReadInt(Address + (int)Offsets._t_strafel); } }

        public int _t_runr_2_jump { get { return Process.ReadInt(Address + (int)Offsets._t_runr_2_jump); } }

        public int wmode_last { get { return Process.ReadInt(Address + (int)Offsets.wmode_last); } }

        public int HitTarget
        {
            get { return Process.ReadInt(Address + (int)Offsets.HitTarget); }
            set { Process.Write(value, Address + (int)Offsets.HitTarget); }
        }

        public int BitField
        {
            get { return Process.ReadInt(Address + (int)Offsets.Bitfield); }
            set { Process.Write(value, Address + (int)Offsets.Bitfield); }
        }

        public int RemoveWeapon2()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.RemoveWeapon2, new CallValue[] { });
        }

        public void SearchStandAni()
        {
            SearchStandAni(false);
        }

        public void SearchStandAni(bool force)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SearchStandAni, new CallValue[] { new BoolArg(force) });
        }

        public void StopTurnAnis()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x6AE530, null);
        }

        public void Reset()
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Reset, new CallValue[] { });
        }

        public void SetAlwaysWalk(int x)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SetAlwaysWalk, new CallValue[] { new IntArg(x) });
        }

        public void ToggleWalkMode(int x)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.ToggleWalkMode, new CallValue[] { new IntArg(x) });
        }

        public void ToggleWalkMode(char x)
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.ToggleWalkMode, new CallValue[] { new CharArg(x) });
        }

        public void SetWalkMode( int x )
        {
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.SetWalkMode, new CallValue[] { new IntArg(x) });
        }

        public void _Forward()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets._Forward, new CallValue[] { });
        }

        public void _Stand()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets._Stand, new CallValue[] { });
        }

        public void _Backward()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets._Backward, new CallValue[] { });
        }

        public void JumpForward()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.JumpForward, new CallValue[] { });
        }

        public void PC_JumpForward()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PC_JumpForward, new CallValue[] { });
        }

        public void PC_GoForward()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PC_GoForward, new CallValue[] { });
        }

        public void PC_GoBackward()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.PC_GoBackward, new CallValue[] { });
        }

        public void CreateHit(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.CreateHit, new CallValue[] { vob });
        }

        public int HitCombo(int DoCombo)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.HitCombo, new CallValue[] { (IntArg)DoCombo });
        }

        public bool IsStanding()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.IsStanding, null) > 0;
        }

        public bool CanParade(oCNpc npc)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)0x6B15B0, new CallValue[] { npc }) > 0;
        }

        public void StartParadeEffects(oCNpc npc)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x6B16F0, new CallValue[] { npc });
        }

        public void StartFallDownAni()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x6B5220, null);
        }

        public zString GetWalkModeZString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetWalkModeString, new CallValue[]{new IntArg(str)});
            return new zString(Process, arg.Address);
        }

        public String GetWalkModeString()
        {
            int str = Process.Alloc(20).ToInt32();
            IntArg arg = Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetWalkModeString, new CallValue[] { new IntArg(str) });
            zString zString = new zString(Process, arg.Address);
            String v = null;
            if(zString.Length < 500)
                v = zString.Value.Trim();
            zString.Dispose();

            return v;
        }

    }
}
