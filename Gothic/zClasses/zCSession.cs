using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSession : zCInputCallback
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
        public zCSession(Process process, int address) : base (process, address)
        {
            
        }

        public zCSession()
        {

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
