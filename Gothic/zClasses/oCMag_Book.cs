using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class oCMag_Book : zClass
    {
        #region OffsetLists
        public enum Offsets
        {
            Spells = 0x00,
            SpellItems = 12
        }
        public enum FuncOffsets : uint
        {
            DoOpen = 0x00478460,
            DoClose = 0x004785E0,
            DoTurn = 0x00478D50,
            SpellCast = 0x004767A0,
            GetSpellByKey = 0x00479C60,
            GetSpellItem = 0x00479BE0,
            CreateNewSpell = 0x00475E50,
            Spell_Setup = 0x004763A0,
            SetShowHandSymbol = 0x00478FD0,
            Open = 0x00476F60,
            Close = 0x00477270,
            StartCastEffect = 0x00476E60,
            Spell_Invest = 0x00476530,
            StartInvestEffect = 0x004766B0,

            SetOwner = 0x00475A50,
            oCMag_Book = 0x004753F0
        }

        public enum HookSize : uint
        {
            SpellCast = 7,
            Spell_Setup = 7,
            Open = 7,
            Close = 7,
            StartCastEffect = 7,
            Spell_Invest = 7,
            StartInvestEffect = 7
        }

        #endregion

        public oCMag_Book()
        {

        }

        public oCMag_Book(Process process, int address)
            : base(process, address)
        {
        }


        public static oCMag_Book Create(Process process)
        {
            IntPtr magBook = process.Alloc(0x7C);

            process.THISCALL<NullReturnCall>((uint)magBook.ToInt32(), (uint)FuncOffsets.oCMag_Book, null);

            oCMag_Book mb = new oCMag_Book(process, magBook.ToInt32());
            
            return mb;
        }

        public oCSpell GetSpellByID(int arg)
        {
            for (int i = 0; i < Spells.Size; i++)
            {
                if (Spells.get(i).GetSpellID() == arg)
                    return Spells.get(i);
            }

            return null;
        }

        public int GetKeyByID(int arg)
        {
            for (int i = 0; i < Spells.Size; i++)
            {
                if (Spells.get(i).GetSpellID() == arg)
                    return i;
            }

            return -1;
        }



        public zCArray<oCSpell> Spells
        {
            get { return new zCArray<oCSpell>(Process, Address + (int)Offsets.Spells); }
        }

        public zCArray<oCItem> SpellItems
        {
            get { return new zCArray<oCItem>(Process, Address + (int)Offsets.SpellItems); }
        }

        public void SetOwner(zCVob vob)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetOwner, new CallValue[] { vob });
        }

        public void DoOpen()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.DoOpen, new CallValue[] { });
        }

        public void Open(int arg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Open, new CallValue[] { new IntArg(arg) });
        }

        public void Close(int arg)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Close, new CallValue[] { new IntArg(arg) });
        }

        public void SpellCast()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SpellCast, new CallValue[] { });
        }

        public void Spell_Invest()
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Spell_Invest, new CallValue[] { });
        }

        public void Spell_Setup(oCNpc npc, zCVob vob, int i)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.Spell_Setup, new CallValue[] { npc, vob, new IntArg(i) });
        }

        public void StartCastEffect(zCVob vob, zVec3 pos)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartCastEffect, new CallValue[] { vob, pos });
        }

        public void StartInvestEffect(zCVob vob, int x, int y, int z)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.StartInvestEffect, new CallValue[] { vob, new IntArg(x), new IntArg(y), new IntArg(z) });
        }


        public oCSpell GetSpellByKey(int arg)
        {
            return Process.THISCALL<oCSpell>((uint)Address, (uint)FuncOffsets.GetSpellByKey, new CallValue[] { new IntArg(arg) });
        }

        public oCSpell CreateNewSpell(int arg)
        {
            return Process.THISCALL<oCSpell>((uint)Address, (uint)FuncOffsets.CreateNewSpell, new CallValue[] { new IntArg(arg) });
        }

        public oCSpell SetShowHandSymbol(int arg)
        {
            return Process.THISCALL<oCSpell>((uint)Address, (uint)FuncOffsets.SetShowHandSymbol, new CallValue[] { new IntArg(arg) });
        }
    }
}
