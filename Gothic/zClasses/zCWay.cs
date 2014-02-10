using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCWay : zClass
    {
                #region OffsetLists
        public enum Offsets : uint
        {
            
        }

        public enum FuncOffsets : uint
        {
            
        }

        public enum HookSize : uint
        {
            
        }
        #endregion

        #region Standard
        public zCWay(Process process, int address) : base (process, address)
        {
            
        }

        public zCWay()
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

        #endregion
    }
}
