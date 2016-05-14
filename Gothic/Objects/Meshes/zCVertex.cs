using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;

namespace Gothic.Objects.Meshes
{
    public class zCVertex : zClass
    {
        public zCVertex(int address)
            : base(address)
        {
        }

        public zCVertex()
        {
        }

        public zVec3 Position { get { return new zVec3(Address); } }
    }
}
