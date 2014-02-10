using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCFont : zClass
    {
        #region OffsetLists
        public enum Offsets : uint
        {
        }

        public enum FuncOffsets : uint
        {
            GetFontX = 0x007894F0,
            GetFontY = 0x007894E0
        }

        public enum HookSize : uint
        {
            GetFontX = 5
            

        }
        #endregion

        #region Standard
        public zCFont(Process process, int address) : base (process, address)
        {
            
        }

        public zCFont()
        {

        }

        public override uint ValueLength()
        {
            return 4;
        }
        #endregion

        #region statics
        

        #endregion

        #region Fields
        
        #endregion

        #region methods
        public int GetFontX(zString str)
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetFontX, new CallValue[] { str }).Address ;
        }

        public int GetFontY()
        {
            return Process.THISCALL<IntArg>((uint)Address, (uint)FuncOffsets.GetFontY, new CallValue[] { }).Address;
        }
        #endregion
    }
}
