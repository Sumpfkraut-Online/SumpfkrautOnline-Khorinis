using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCMesh : zCVisual
    {
        public zCMesh(Process process, int address)
            : base(process, address)
        {
        }

        public zCMesh() { }

        public enum Offsets
        {
            Material = 0x18
        }


        public zCMaterial Material
        {
            get { return new zCMaterial(Process, Process.ReadInt(Address + (int)Offsets.Material)); }
        }
    }
}
