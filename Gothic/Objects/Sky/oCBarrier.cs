using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects.Meshes;
using WinApi;

namespace Gothic.Objects.Sky
{
    public class oCBarrier : zClass
    {
        public oCBarrier(int address)
            : base(address)
        {
        }

        public oCBarrier()
        {
        }

        public zCMesh FrontierMesh { get { return new zCMesh(Process.ReadInt(Address)); } }

        public bool DwordDC
        {
            get { return Process.ReadBool(Address + 0xDC); }
            set { Process.Write(Address + 0xDC, value); }
        }

        public int Alpha
        {
            get { return Process.ReadInt(Address + 0xE0); }
            set { Process.Write(Address + 0xE0, value); }
        }

        public int DwordE4
        {
            get { return Process.ReadInt(Address + 0xE4); }
            set { Process.Write(Address + 0xE4, value); }
        }

        public int DwordE8
        {
            get { return Process.ReadInt(Address + 0xE8); }
            set { Process.Write(Address + 0xE8, value); }
        }

        public void RenderLayer(int renderContext, int arg, int ptr)
        {
            Process.THISCALL<NullReturnCall>(Address, 0x6B9CF0, (IntArg)renderContext, (IntArg)arg, (IntArg)ptr);
        }
    }
}
