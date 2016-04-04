using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects;

namespace Gothic.Sound
{
    public static class zCSndSys_MSS
    {
        public const int zsound = 0x99B03C;

        public static int GetZSound()
        {
            return Process.ReadInt(zsound);
        }

        public static void DoSoundUpdate()
        {
            Process.THISCALL<NullReturnCall>(GetZSound(), 0x004F3D60);
        }

        public static zCSndFX_MSS LoadSoundFX(string str)
        {
            zCSndFX_MSS ret;
            using (zString z = zString.Create(str))
            {
                ret = LoadSoundFX(z);
            }
            return ret;
        }

        public static zCSndFX_MSS LoadSoundFX(zString str)
        {
            return Process.THISCALL<zCSndFX_MSS>(GetZSound(), 0x004ED960, str);
        }

        public static void PlaySound3D(zCSndFX_MSS snd, zCVob vob, int arg, int sndParams)
        {
            Process.THISCALL<NullReturnCall>(GetZSound(), 0x004F10F0, snd, vob, new IntArg(arg), new IntArg(sndParams));
        }

        public static void PlaySound3D(string snd, zCVob vob, int arg, int sndParams)
        {
            using (zString z = zString.Create(snd))
                PlaySound3D(z, vob, arg, sndParams);
        }

        public static void PlaySound3D(zString snd, zCVob vob, int arg, int sndParams)
        {
            Process.THISCALL<NullReturnCall>(GetZSound(), 0x004F1060, snd, vob, new IntArg(arg), new IntArg(sndParams));
        }
    }
}
