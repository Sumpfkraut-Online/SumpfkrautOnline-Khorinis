using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;

namespace Gothic.Objects.Meshes
{
    public class zCVertFeature : zClass
    {
        public zCVertFeature(int address)
            : base(address)
        {
        }

        public zCVertFeature()
        {
        }

        public zColor maybeDefaultColor { get { return new zColor(Address + 12); } }
        public zColor Color { get { return new zColor(Address + 16); } }
    }
}
