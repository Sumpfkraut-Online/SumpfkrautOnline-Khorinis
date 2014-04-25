using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zCVisual : zCObject
    {

        public enum Offsets
        {
            meshName = 192
        }


        public zCVisual(Process process, int address)
            : base(process, address)
        {
        }

        public zCVisual() { }


        public zString MeshName
        {
            get { return new zString(Process, Address + (int)Offsets.meshName); }
        }



    }
}
