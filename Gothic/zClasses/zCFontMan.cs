using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCFontMan : zClass
    {

        #region Offsets

        public enum FuncOffset
        {
            Load = 0x007882D0,
            GetFont = 0x007884B0
        }

        public enum HookSize
        {

        }
        #endregion
        public zCFontMan()
        { }
        public zCFontMan(Process process, int address)
            : base(process, address)
        {

        }

        public static zCFontMan getFontMan(Process process)
        {
            return new zCFontMan(process, 0x00AB39D4);
        }

        #region methods

        public int Load(zString fontname)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffset.Load, new CallValue[] { fontname });
        }

        public zCFont GetFont(int fontid)
        {
            return Process.THISCALL<zCFont>((uint)Address, (uint)FuncOffset.GetFont, new CallValue[] { new IntArg(fontid) });
        }
        #endregion
    }
}
