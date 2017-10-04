using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;

namespace Gothic.Objects.Meshes
{
    public class zCModelAniEvent : zClass
    {
        public const int ByteSize = 0x94;

        public zCModelAniEvent(int address) : base(address)
        {
        }

        public enum Types
        {
            Tag,
            Sound,
            Sound_Step,
            //...
        }

        public Types AniType { get { return (Types)Process.ReadInt(this.Address); } }
        public int Frame { get { return Process.ReadInt(this.Address + 4); } }
        public zString TagString { get { return new zString(this.Address + 8); } }

        public zString String0 { get { return new zString(this.Address + 28); } }
        public zString String1 { get { return new zString(this.Address + 48); } }
        public zString String2 { get { return new zString(this.Address + 68); } }
        public zString String3 { get { return new zString(this.Address + 88); } }

        public float Value0 { get { return Process.ReadFloat(this.Address + 108); } }
        public float Value1 { get { return Process.ReadFloat(this.Address + 112); } }
        public float Value2 { get { return Process.ReadFloat(this.Address + 116); } }
        public float Value3 { get { return Process.ReadFloat(this.Address + 120); } }
    }
}
