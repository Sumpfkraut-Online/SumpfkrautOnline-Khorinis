using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;

namespace Gothic.Objects.Meshes
{
    public class zCMesh : zCObject
    {
        new public abstract class VarOffsets : zCObject.VarOffsets
        {
            public const int MeshName = 192;
        }

        public zCMesh(int address)
            : base(address)
        {
        }

        public zCMesh()
        {
        }

        public zString MeshName
        {
            get { return new zString(Address + VarOffsets.MeshName); }
        }

        public static zCMesh LoadMesh()
        {
            return null;
        }
    }
}
