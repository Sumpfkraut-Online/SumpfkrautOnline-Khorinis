using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic
{
    public class zCOptionSection : zClass
    {
        public zCOptionSection(int address)
            : base(address)
        {

        }

        public zCOptionSection()
        {

        }

        public abstract class VarOffsets
        {
            public const int SectionName = 0,
            EntryList = 20;
        }
        
        public zString SectionName
        {
            get { return new zString(Address + VarOffsets.SectionName); }
        }

        public zCOptionEntry GetEntryByName(String name)
        {
            zCOptionEntry ret;
            using (zString str = zString.Create(name))
                ret = GetEntryByName(str);
            return ret;
        }

        public zCOptionEntry GetEntryByName(zString name)
        {
            return Process.THISCALL<zCOptionEntry>(zCOption.zoptions, zCOption.FuncAddresses.GetEntryByName, this, name, new IntArg(1));
        }
    }
}
