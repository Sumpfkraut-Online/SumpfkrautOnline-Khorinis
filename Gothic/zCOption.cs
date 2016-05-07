using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic
{
    public static class zCOption
    {
        public const int zoptions = 0x008CD988;
        public const int zgameoptions = 0x008CD98C;

        public abstract class VarOffsets
        {
            public const int SectionList = 8;
        }

        public abstract class FuncAddresses
        {
            public const int Save = 0x004616C0,
            GetSectionByName = 0x463000, ///<summary> zCOptionSection * __thiscall zCOption::GetSectionByName(class zSTRING const &, int) </summary>
            GetEntryByName = 0x462D10, ///<summary> zCOptionEntry * __thiscall zCOption::GetEntryByName(class zCOptionSection *, class zSTRING const &, int) </summary>
            AddParameters = 0x463B00, ///<summary> int __thiscall zCOption::AddParameters(class zSTRING) </summary>
            WriteBool_char = 0x461DE0; ///<summary> int __thiscall zCOption::WriteBool(class zSTRING const &, char const *, int, int) </summary>
        }

        public static zCOptionSection GetSectionByName(String name)
        {
            zCOptionSection ret;
            using (zString str = zString.Create(name))
                ret = GetSectionByName(str);
            return ret;
        }

        public static zCOptionSection GetSectionByName(zString name)
        {
            return Process.THISCALL<zCOptionSection>(Process.ReadInt(zoptions), FuncAddresses.GetSectionByName, name, new IntArg(1));
        }

        public static int Save(string file)
        {
            int ret;
            using (zString str = zString.Create(file))
                ret = Save(str);
            return ret;
        }

        public static int Save(zString file)
        {
            return Process.THISCALL<IntArg>(Process.ReadInt(zoptions), FuncAddresses.Save, (IntArg)file.VTBL, (IntArg)file.ALLOCATER, (IntArg)file.PTR, (IntArg)file.Length, (IntArg)file.Res);
        }


        public static int AddParameters(string parms)
        {
            int ret;
            using (zString str = zString.Create(parms))
                ret = AddParameters(str);
            return ret;
        }

        public static int AddParameters(zString parms)
        {
            return Process.THISCALL<IntArg>(Process.ReadInt(zoptions), FuncAddresses.AddParameters, (IntArg)parms.VTBL, (IntArg)parms.ALLOCATER, (IntArg)parms.PTR, (IntArg)parms.Length, (IntArg)parms.Res);
        }
    }
}
