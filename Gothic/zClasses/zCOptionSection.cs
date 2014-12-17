using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCOptionSection : zClass
    {
        public zCOptionSection(Process process, int address)
            : base(process, address)
        {

        }

        public zCOptionSection()
        {

        }


        #region OffsetLists
        public enum Offsets : uint
        {
            SectionName = 0,
            EntryList = 20
        }

        public enum FuncOffsets : uint
        {
        }

        public enum HookSize : uint
        {

        }
        #endregion


        #region statics


        #endregion

        #region Fields

        public zString SectionName
        {
            get { return new zString(Process, Address + (int)Offsets.SectionName); }
        }

        public zCArray<zCOptionEntry> EntryList
        {
            get { return new zCArray<zCOptionEntry>(Process, Address + (int)Offsets.EntryList); }
        }

        #endregion

        #region methods

        #endregion

        #region ownmethods

        public zCOptionEntry getEntry(String entryname)
        {
            zCArray<zCOptionEntry> list = EntryList;
            int size = list.Size;

            for (int i = 0; i < size; i++)
            {
                if (list.get(i).VarName.Value.Trim().ToLower() == entryname.Trim().ToLower())
                    return list.get(i);
            }

            return null;
        }

        #endregion
    }
}
