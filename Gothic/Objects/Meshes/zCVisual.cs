using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

namespace Gothic.Objects.Meshes
{
    public class zCVisual : zCObject
    {
        new public abstract class VarOffsets : zCObject.VarOffsets
        {
            public const int meshName = 192;
        }

        public abstract class FuncAddresses
        {
            /// <summary>
            /// zCVisual * __cdecl zCVisual::LoadVisual(class zSTRING const &)
            /// </summary>
            public const int LoadVisual = 0x606AD0;
        }

        public zCVisual(int address)
            : base(address)
        {
        }

        public zCVisual() { }

        public zString MeshName
        {
            get { return new zString(Address + VarOffsets.meshName); }
        }

        public static zCVisual LoadVisual(string visual)
        {
            zCVisual ret;
            using (zString str = zString.Create(visual))
                ret = LoadVisual(str);
            return ret;
        }

        public static zCVisual LoadVisual(zString visual)
        {
            return Process.CDECLCALL<zCVisual>(FuncAddresses.LoadVisual, visual);
        }
    }
}
