using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Types;
using WinApi;

namespace Gothic.Objects.Meshes
{
    public class zCModelPrototype : zClass
    {
        public zCModelPrototype()
        {

        }

        public zCModelPrototype(int address)
            : base(address)
        {

        }

        public abstract class VarOffsets
        {
            public const int Mesh = 0x80;
        }

        public zCModelAni SearchAni(string aniName)
        {
            zCModelAni ret;
            using (zString z = zString.Create(aniName))
                ret = SearchAni(z);
            return ret;
        }

        public zCModelAni SearchAni(zString aniName)
        {
            return Process.THISCALL<zCModelAni>(Address, 0x58A090, aniName);
        }

        public int InsertSort(zCModelAni ani)
        {
            return Process.THISCALL<IntArg>(Address + 72, 0x5A1FA0, ani);
        }

        public void AddAni(zCModelAni ani)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x589DB0, ani);
        }

        public zString Mesh
        {
            get { return new zString(Address + VarOffsets.Mesh); }
        }
    }
}
