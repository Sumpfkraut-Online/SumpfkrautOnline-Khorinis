using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSoundFX : zClass
    {
        #region OffsetLists
        public enum Offsets
        {
        }

        public enum FuncOffsets : uint
        {
        }

        public enum HookSize : uint
        {

        }

        #endregion

        public zCSoundFX()
        {

        }

        public zCSoundFX(Process process, int address)
            : base(process, address)
        {
        }


        public override uint ValueLength()
        {
            return 4;
        }    
    }
}
