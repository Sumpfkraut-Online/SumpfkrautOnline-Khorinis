using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCModelPrototype : zClass
    {
        #region OffsetLists
        public enum Offsets
        {
            
        }
        public enum FuncOffsets : uint
        {
            SearchAni = 0x0058A090,
        }

        public enum HookSize : uint
        {
            
        }

        #endregion




        public zCModelPrototype(Process process, int address)
            : base(process, address)
        {
            
        }


        public zCModelAni SearchAni(String str)
        {
            zString zStr = zString.Create(Process, str);
            zCModelAni ma = SearchAni(zStr);
            zStr.Dispose();

            return ma;
        }

        public zCModelAni SearchAni(zString str)
        {
            return Process.THISCALL<zCModelAni>((uint)Address, (uint)FuncOffsets.SearchAni, new CallValue[] { str });
        }

        public override uint ValueLength()
        {
            return 4;
        }
    }
}
