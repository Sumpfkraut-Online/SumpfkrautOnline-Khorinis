using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Objects.Meshes
{
    public class zCPolygon : zClass
    {
        public zCPolygon(int address)
            : base(address)
        {
        }

        public zCPolygon()
        {
        }

        public byte NumVertices { get { return Process.ReadByte(Address + 0x30); } }

        public zCVertFeature GetVertFeature(int index)
        {
            return new zCVertFeature(Process.ReadInt(Process.ReadInt(Address + 0x2C) + 4 * index));
        }

        public zCVertex GetVertex(int index)
        {
            return new zCVertex(Process.ReadInt(Process.ReadInt(Address) + 4 * index));
        }

        public zCMaterial Material
        {
            get { return new zCMaterial(Process.ReadInt(Address + 0x18)); }
        }
    }
}
