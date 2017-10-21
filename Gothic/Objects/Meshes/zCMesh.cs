using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

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

        public zCMaterial Material
        {
            get { return new zCMaterial(Address + 0x18); }
        }

        public zCMaterial GetSomethingMaterial()
        {
            int addr = Process.ReadInt(Address + 68);
            if (addr != 0)
            {
                addr = Process.ReadInt(addr);
                if (addr != 0)
                {
                    addr = Process.ReadInt(addr + 24);
                    if (addr != 0)
                    {
                        return new zCMaterial(addr);
                    }
                }
            }
            return null;
        }

        public static zCMesh LoadMesh()
        {
            return null;
        }

        public void SetMaterial(zCMaterial material)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x56AEC0, material);
        }

        public int NumPolygons { get { return Process.ReadInt(Address + 0x34); } }

        public zCPolygon GetPolygon(int index)
        {
            return new zCPolygon(Process.ReadInt(Process.ReadInt(Address + 0x44) + index * 4));
        }

        public zTBBox3D BBox3D { get { return new zTBBox3D(Address + 88); } }


        public void CalcBBox3D()
        {
            Process.THISCALL<NullReturnCall>(Address, 0x56A610);
        }
    }
}
