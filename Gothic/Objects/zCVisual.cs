using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects
{
    public class zCVisual : zCObject
    {
        new public abstract class VarOffsets : zCObject.VarOffsets
        {
            public const int MeshName = 192;
        }

        public zCVisual(int address)
            : base(address)
        {
        }

        public zCVisual()
        {
        }

        public zString MeshName
        {
            get { return new zString(Address + VarOffsets.MeshName); }
        }
    }
}
