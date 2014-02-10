using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace Gothic.zTypes
{
    public class zTBBox3D : zClass
    {
        public zTBBox3D(Process process, int address)
            : base(process, address)
        {
            
        }

        public zTBBox3D()
        {

        }

        public zPoint3 Min { get { return new zPoint3(Process, Address); } }
        public zPoint3 Max { get { return new zPoint3(Process, Address+12); } }

        public float Height
        {
            get { return Max.Y - Min.Y; }
        }

    }
}
