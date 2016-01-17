using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects.Sky
{
    public class zCSkyLayer : zClass
    {
        public zCSkyLayer()
        {
        }

        public zCSkyLayer(int address)
            : base(address)
        {
        }

        public enum zESkyLayerMode { zSKY_MODE_POLY, zSKY_MODE_BOX };
        
        public abstract class VarOffsets
        {
            public const int PolyMesh = 0,
            Poly = 4,
            TexOffsets = 8,
            DomeMesh = 16,
            Mode = 20;
        }

        public abstract class FuncAddresses
        {
            public const int SetDomeMeshEnabled = 0x005E4D40;
        }

        /*public enum HookSize
        {
            SetDomeMeshEnabled = 6
        }*/
        
        /*public zCMesh PolyMesh
        {
            get { return new zCMesh(Process.ReadInt(Address + VarOffsets.PolyMesh)); }
        }
        public zCMesh DomeMesh
        {
            get { return new zCMesh(Process.ReadInt(Address + VarOffsets.DomeMesh)); }
        }*/

        public void SetDomeMeshEnabled(int x)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.SetDomeMeshEnabled, new IntArg(x));
        }
    }
}
