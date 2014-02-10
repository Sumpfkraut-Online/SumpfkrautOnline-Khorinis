using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCOption : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {
            SectionList = 8
        }

        public enum FuncOffsets : uint
        {
            Save = 0x004616C0,
        }

        public enum HookSize : uint
        {
            
        }
        #endregion

        #region Standard
        public zCOption(Process process, int address) : base (process, address)
        {
            
        }

        public zCOption()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }


        #endregion

        #region statics
        public static zCOption GetOption(Process process)
        {
            return new zCOption(process, process.ReadInt(0x008CD988));
        }

        #endregion

        #region Fields

        public zCArray<zCOptionSection> SectionList
        {
            get { return new zCArray<zCOptionSection>(Process, Address + (int)Offsets.SectionList); }
        }


        #endregion

        #region ownmethods

        public String getEntryValue(String section, String Entry)
        {
            zCOptionSection sec = getSection(section);
            if (section == null)
                return "";

            zCOptionEntry entr = sec.getEntry(Entry);
            if (entr == null)
                return "";

            return entr.VarValue.Value;
        }

        public zCOptionSection getSection(String section)
        {
            zCArray<zCOptionSection> list = SectionList;
            int size = list.Size;

            for (int i = 0; i < size; i++)
            {
                if (list.get(i).SectionName.Value.Trim().ToLower() == section.Trim().ToLower())
                    return list.get(i);
            }

            return null;
        }

        #endregion

        #region methods
        public void Save(string file)
        {
            zString str = zString.Create(Process, file);
            //Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Save, new CallValue[] { (IntArg)str.Res, (IntArg)str.Length, (IntArg)str.PTR, (IntArg)str.ALLOCATER, (IntArg)str.VTBL });
            Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Save, new CallValue[] { (IntArg)str.VTBL, (IntArg)str.ALLOCATER, (IntArg)str.PTR, (IntArg)str.Length, (IntArg)str.Res });

            Process.Free(new IntPtr(str.Address), 20);
            //str.Dispose();
        }
        #endregion
    }
}
