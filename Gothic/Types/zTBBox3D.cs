using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zTBBox3D : zClass
    {
        public const int ByteSize = 24;

        public zTBBox3D(int address)
            : base(address)
        {
            
        }

        public zTBBox3D()
        {

        }

        public zPoint3 Min { get { return new zPoint3(Address); } }
        public zPoint3 Max { get { return new zPoint3(Address+12); } }

        public float Height
        {
            get { return Max.Y - Min.Y; }
        }
    }
}
