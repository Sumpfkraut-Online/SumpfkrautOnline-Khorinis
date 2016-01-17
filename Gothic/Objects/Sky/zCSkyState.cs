using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects.Sky
{
    public class zCSkyState : zClass
    {
        public zCSkyState()
        {
        }

        public zCSkyState(int address)
            : base(address)
        {

        }
        
        public abstract class VarOffsets
        {
            public const int Layer0 = 64,
            Layer1 = 108;
        }
        
        public zCSkyLayerData Layer0
        {
            get { return new zCSkyLayerData(Address + VarOffsets.Layer0); }
        }
        public zCSkyLayerData Layer1
        {
            get { return new zCSkyLayerData(Address + VarOffsets.Layer1); }
        }
    }
}
