using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic
{
    public static class zCSoundSystem
    {
        public const int zsound = 0x99B03C;

        public static void DoSoundUpdate()
        {
            Process.THISCALL<NullReturnCall>(Process.ReadInt(zsound), 0x004F3D60);
        }
    }
}
