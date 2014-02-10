using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSkyLayer : zClass
    {
        public zCSkyLayer() { }
        public zCSkyLayer(Process process, int address)
            : base(process, address)
        {

        }

        public enum zESkyLayerMode { zSKY_MODE_POLY, zSKY_MODE_BOX };
        #region Offsets
        public enum Offsets
        {
            PolyMesh = 0,
            Poly = 4,
            TexOffsets = 8,
            DomeMesh = 16,
            Mode = 20
        }

        public enum FuncOffsets
        {
            SetDomeMeshEnabled = 0x005E4D40
        }

        public enum HookSize
        {
            SetDomeMeshEnabled = 6
        }
        #endregion

        #region Fields
        public zCMesh PolyMesh
        {
            get { return new zCMesh(Process, Process.ReadInt(Address + (int)Offsets.PolyMesh)); }
        }
        public zCMesh DomeMesh
        {
            get { return new zCMesh(Process, Process.ReadInt(Address + (int)Offsets.DomeMesh)); }
        }
        #endregion

        #region methods

        public void SetDomeMeshEnabled(int x)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)FuncOffsets.SetDomeMeshEnabled, new CallValue[]{new IntArg(x) });
        }

        #endregion
    }
}
