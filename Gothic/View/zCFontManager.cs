using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.View
{
    class zCFontManager : zClass
    {
        public const int zfontman = 0xAB39D4;
        
        public static int ManagerAddress { get { return Process.ReadInt(zfontman); } }

        public zCFontManager(int address) : base(address)
        {
        }

        public static zCFontManager Manager { get { return new zCFontManager(ManagerAddress); } }

        public int Load(string fontName)
        {
            int ret;
            using (zString z = zString.Create(fontName))
                ret = Load(z);
            return ret;
        }

        public int Load(zString fontName)
        {
            return Process.THISCALL<IntArg>(ManagerAddress, 0x7882D0, fontName);
        }

        public zCFont GetFont(int num)
        {
            return Process.THISCALL<zCFont>(ManagerAddress, 0x7884B0, (IntArg)num);
        }
    }
}
