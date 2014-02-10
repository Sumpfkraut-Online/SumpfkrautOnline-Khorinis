using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSkyState : zClass
    {
        public zCSkyState() { }
        public zCSkyState(Process process, int address)
            : base(process, address)
        {

        }

        #region Offsets
        public enum Offsets
        {
            Layer0 = 64,
            Layer1 = 108
        }

        public enum FuncOffsets
        {
            
        }

        public enum HookSize
        {
            
        }
        #endregion

        #region Fields
        public zCSkyLayerData Layer0
        {
            get { return new zCSkyLayerData(Process, Address + (int)Offsets.Layer0); }
        }
        public zCSkyLayerData Layer1
        {
            get { return new zCSkyLayerData(Process, Address + (int)Offsets.Layer1); }
        }
        #endregion


    }
}
