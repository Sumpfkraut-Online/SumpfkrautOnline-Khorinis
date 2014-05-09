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
            SpellID = 0x54
        }
        public enum FuncOffsets : uint
        {
            GetSpellID = 0x00486480,
            GetSpellInstanceName = 0x00484150,
            GetName = 0x004864B0,
            Cast = 0x00485360,
            CastSpecificSpell = 0x00486960,
            CreateEffect = 0x004842E0,
            Invest = 0x004850D0
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

        public int GetSpellID()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetSpellID, new CallValue[] {  }).Address;
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
    }
}
