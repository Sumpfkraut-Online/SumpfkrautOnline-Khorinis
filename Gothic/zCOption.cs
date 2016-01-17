using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic
{
    public class zCOption : zClass
    {
        public abstract class VarOffsets
        {
            public const int SectionList = 8;
        }

        public abstract class FuncAddresses
        {
            public const int Save = 0x004616C0;
        }
        
        public zCOption(int address) : base (address)
        {
            
        }

        public zCOption()
        {

        }
        
        public static zCOption GetOption()
        {
            return new zCOption(Process.ReadInt(0x008CD988));
        }

        public zCArray<zCOptionSection> SectionList
        {
            get { return new zCArray<zCOptionSection>(Address + VarOffsets.SectionList); }
        }

        #region ownmethods

        public String getEntryValue(String section, String Entry)
        {
            zCOptionSection sec = getSection(section);
            if (section == null)
                return "";

            zCOptionEntry entr = sec.getEntry(Entry);
            if (entr == null)
                return "";

            return entr.VarValue.ToString();
        }

        public zCOptionSection getSection(String section)
        {
            zCArray<zCOptionSection> list = SectionList;
            int size = list.GetCount();

            for (int i = 0; i < size; i++)
            {
                if (list.get(i).SectionName.ToString().Trim().ToLower() == section.Trim().ToLower())
                    return list.get(i);
            }

            return null;
        }

        #endregion
        
        public int Save(string file)
        {
            int rval;
            using (zString str = zString.Create(file))
                //Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.Save, new CallValue[] { (IntArg)str.Res, (IntArg)str.Length, (IntArg)str.PTR, (IntArg)str.ALLOCATER, (IntArg)str.VTBL });
                rval = Process.THISCALL<IntArg>(Address, FuncAddresses.Save, (IntArg)str.VTBL, (IntArg)str.ALLOCATER, (IntArg)str.PTR, (IntArg)str.Length, (IntArg)str.Res);

            return rval;
        }

        public float ReadReal(string section, string entry, float def)
        {
            using (zString sec = zString.Create(section))
            using (zString ent = zString.Create(entry))
            {
                return Process.THISCALL<FloatArg>(Address, 0x463A60, sec, ent, (FloatArg)def);
            }
        }
    }
}
