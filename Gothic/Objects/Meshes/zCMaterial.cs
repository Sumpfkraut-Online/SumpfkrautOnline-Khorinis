using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects.Meshes
{
    public class zCMaterial : zCObject
    {
        public zCMaterial(int address)
            : base(address)
        {
        }

        public zCMaterial()
        {
        }

        public int AlphaFunc
        {
            get { return Process.ReadInt(Address + 116); }
            set { Process.Write(Address + 116, value); }
        }

        public void SetTexture(string str)
        {
            using (var zstr = zString.Create(str))
                SetTexture(zstr);
        }

        public void SetTexture(zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x5649E0, str);
        }

        public zColor Color { get { return new zColor(Address + 56); } }
        public int MatGroup
        {
            get { return Process.ReadInt(Address + 0x40); }
            set { Process.Write(Address + 0x40, value); }
        }

        public static zCMaterial Create()
        {
            int ptr = Process.CDECLCALL<IntArg>(0x563D20); // createinstance
            //Process.THISCALL<NullReturnCall>(ptr, 0x563E00); // constructor
            return new zCMaterial(ptr);
        }
    }
}
