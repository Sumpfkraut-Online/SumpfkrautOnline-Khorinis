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

        public zCArray<zCOptionEntry> EntryList
        {
            get { return new zCArray<zCOptionEntry>(Address + VarOffsets.EntryList); }
        }


        public zCOptionEntry getEntry(String entryname)
        {
            zCArray<zCOptionEntry> list = EntryList;
            int size = list.GetCount();

            for (int i = 0; i < size; i++)
            {
                if (list.get(i).VarName.ToString().Trim().ToLower() == entryname.Trim().ToLower())
                    return list.get(i);
            }

            return null;
        }
    }
}
