using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects.Sky
{
    public class zCSkyLayerData : zClass
    {
        public zCSkyLayerData()
        {
        }

        public zCSkyLayerData(int address)
            : base(address)
        {

        }
        
        public abstract class VarOffsets
        {
            public const int skyMode = 0,
            texture = 4,
            texName = 8,
            texAlpha = 28,
            texScale = 32,
            texSpeed = 36;
        }
        
        public zString TexName
        {
            get { return new zString(Address + VarOffsets.texName); }
        }
    }
}
