using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCSpell : zCObject
    {
        #region OffsetLists
        public enum Offsets
        {
            TimePerMana = 128,
            DamagePerLevel = 132,
            DamageType = 136,
            SpellType = 140,
            CanTurnDuringInvest = 144,
            CanChangeTargetDuringInvest = 148,
            IsMultiEffect = 152,
            TargetCollectionAlgo = 156,
            TargetCollectType = 160,
            TargetCollectRange = 164,
            TargetCollectAzi = 168,
            TargetCollectElev = 172,
            SpellID = 0x54,//84
            SpellInfo = 0x58,//88

            NPC = 0x30,
            Caster = 0x34,
            Target = 0x3C,

            Visual = 0x28,
            
        }
        public enum FuncOffsets : uint
        {
            GetSpellID = 0x00486480,
            GetSpellInstanceName = 0x00484150,
            GetName = 0x004864B0,
            Cast = 0x00485360,
            CastSpecificSpell = 0x00486960,
            CreateEffect = 0x004842E0,
            Invest = 0x004850D0,
            SetSpellInfo = 0x00486940,
            Stop = 0x00485510,
            Reset = 0x00485B00
        }

        public enum HookSize : uint
        {
            Cast = 7,
            CastSpecificSpell = 7,
            CreateEffect = 7,
            Invest = 7
        }

        #endregion

        public oCSpell()
        {

        }

        public oCSpell(Process process, int address)
            : base(process, address)
        {
        }


        #region Fields

        public oCNpc NPC
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.NPC) ); }
            set { Process.Write(value.Address, Address + (int)Offsets.NPC); }
        }

        public oCNpc Caster
        {
            get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.Caster)); }
            set { Process.Write(value.Address, Address + (int)Offsets.Caster); }
        }

        public zCVob Target
        {
            get { return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.Target)); }
            set { Process.Write(value.Address, Address + (int)Offsets.Target); }
        }

        public float TimePerMana
        {
            get { return Process.ReadFloat(Address + (int)Offsets.TimePerMana); }
            set { Process.Write(value, Address + (int)Offsets.TimePerMana); }
        }

        public int DamagePerLevel
        {
            get { return Process.ReadInt(Address + (int)Offsets.DamagePerLevel); }
            set { Process.Write(value, Address + (int)Offsets.DamagePerLevel); }
        }

        public int DamageType
        {
            get { return Process.ReadInt(Address + (int)Offsets.DamageType); }
            set { Process.Write(value, Address + (int)Offsets.DamageType); }
        }

        public int CanTurnDuringInvest
        {
            get { return Process.ReadInt(Address + (int)Offsets.CanTurnDuringInvest); }
            set { Process.Write(value, Address + (int)Offsets.CanTurnDuringInvest); }
        }

        public int CanChangeTargetDuringInvest
        {
            get { return Process.ReadInt(Address + (int)Offsets.CanChangeTargetDuringInvest); }
            set { Process.Write(value, Address + (int)Offsets.CanChangeTargetDuringInvest); }
        }

        public int IsMultiEffect
        {
            get { return Process.ReadInt(Address + (int)Offsets.IsMultiEffect); }
            set { Process.Write(value, Address + (int)Offsets.IsMultiEffect); }
        }

        public int TargetCollectionAlgo
        {
            get { return Process.ReadInt(Address + (int)Offsets.TargetCollectionAlgo); }
            set { Process.Write(value, Address + (int)Offsets.TargetCollectionAlgo); }
        }

        public int TargetCollectType
        {
            get { return Process.ReadInt(Address + (int)Offsets.TargetCollectType); }
            set { Process.Write(value, Address + (int)Offsets.TargetCollectType); }
        }

        public int TargetCollectRange
        {
            get { return Process.ReadInt(Address + (int)Offsets.TargetCollectRange); }
            set { Process.Write(value, Address + (int)Offsets.TargetCollectRange); }
        }

        public int TargetCollectAzi
        {
            get { return Process.ReadInt(Address + (int)Offsets.TargetCollectAzi); }
            set { Process.Write(value, Address + (int)Offsets.TargetCollectAzi); }
        }

        public int TargetCollectElev
        {
            get { return Process.ReadInt(Address + (int)Offsets.TargetCollectElev); }
            set { Process.Write(value, Address + (int)Offsets.TargetCollectElev); }
        }

        public int SpellType
        {
            get { return Process.ReadInt(Address + (int)Offsets.SpellType); }
            set { Process.Write(value, Address + (int)Offsets.SpellType); }
        }

        public int SpellID
        {
            get { return Process.ReadInt(Address + (int)Offsets.SpellID); }
        }

        public oCVisualFX VisualFX
        {
            get { return new oCVisualFX(Process, Process.ReadInt(Address + (int)Offsets.Visual)); }
        }

        //public oCNpc Caster
        //{
        //    get { return new oCNpc(Process, Process.ReadInt(Address + (int)Offsets.Caster)); }
        //}

        //public zCVob Target
        //{
        //    get { return new zCVob(Process, Process.ReadInt(Address + (int)Offsets.Target)); }
        //}
        #endregion


        public void Reset()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Reset, new CallValue[] { });
        }

        public int GetSpellID()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetSpellID, new CallValue[] {  }).Address;
        }

        public int Stop()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Stop, new CallValue[] { }).Address;
        }

        /// <summary>
        /// Call Dispose after Usage!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public zString GetSpellInstanceName(int id)
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetSpellInstanceName, new CallValue[] { new IntArg(id) });
        }

        /// <summary>
        /// Needs Dispose() after use!
        /// </summary>
        /// <returns></returns>
        public zString GetName()
        {
            return Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.GetName, new CallValue[] { });
        }

        public void Cast()
        {
            Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.Cast, new CallValue[] { });
        }

        public void CastSpecificSpell()
        {
            Process.THISCALL<zString>((uint)Address, (uint)FuncOffsets.CastSpecificSpell, new CallValue[] { });
        }


        public override uint ValueLength()
        {
            return 4;
        }
    }
}
