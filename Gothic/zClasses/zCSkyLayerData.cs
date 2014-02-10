using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCSkyLayerData : zClass
    {
        public zCSkyLayerData() { }
        public zCSkyLayerData(Process process, int address)
            : base(process, address)
        {

        }

        #region Offsets
        public enum Offsets
        {
            skyMode = 0,
            texture = 4,
            texName = 8,
            texAlpha = 28,
            texScale = 32,
            texSpeed = 36
        }

        public enum FuncOffsets
        {
            
        }

        public enum HookSize
        {
            
        }
        #endregion

        #region Fields
        public zString TexName
        {
            get { return new zString(Process, Address + (int)Offsets.texName); }
        }
        #endregion
    }
}
