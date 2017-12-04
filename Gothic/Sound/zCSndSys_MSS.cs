using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.Types;
using Gothic.Objects;
using WinApi.Calls;

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

        static ThisReturnCall<int, int> loadsoundfx = new ThisReturnCall<int, int>(0x004ED960);
        public static zCSndFX_MSS LoadSoundFX(zString str)
        {
            return new zCSndFX_MSS(loadsoundfx.Call(GetZSound(), str.Address));
            //return Process.THISCALL<zCSndFX_MSS>(GetZSound(), 0x004ED960, str);
        }

        static ThisReturnCall<int, int, int, int, int> playsound3d = new ThisReturnCall<int, int, int, int, int>(0x004F10F0);
        public static int PlaySound3D(zCSndFX_MSS snd, zCVob vob, int arg, zTSound3DParams param)
        {
            return playsound3d.Call(GetZSound(), snd.Address, vob.Address, arg, param.Address);
            //return Process.THISCALL<IntArg>(GetZSound(), 0x004F10F0, snd, vob, new IntArg(arg), param);
        }

        public static int PlaySound3D(string snd, zCVob vob, int arg, zTSound3DParams param)
        {
            int ret;
            using (zString z = zString.Create(snd))
                ret = PlaySound3D(z, vob, arg, param);
            return ret;
        }

        public static int PlaySound3D(zString snd, zCVob vob, int arg, zTSound3DParams param)
        {
            return Process.THISCALL<IntArg>(GetZSound(), 0x004F1060, snd, vob, new IntArg(arg), param);
        }

        public static int PlaySound(zCSndFX_MSS snd, bool arg)
        {
            return Process.THISCALL<IntArg>(GetZSound(), 0x004EF7B0, snd, new BoolArg(arg));
        }

        public static void StopSound(int snd)
        {
            int ptr = Process.Alloc(4).ToInt32();
            Process.Write(ptr, snd);
            Process.THISCALL<IntArg>(GetZSound(), 0x004F2300, (IntArg)ptr);
            Process.Free(ptr, 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snd"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="volume">0.0 to 2.0</param>
        /// <param name="stereo">-1 for only left to +1 for only right</param>
        public static int PlaySound(zCSndFX_MSS snd, bool arg0, int arg1, float volume, float stereo)
        {
            return Process.THISCALL<IntArg>(GetZSound(), 0x004F0B70, snd, (BoolArg)arg0, (IntArg)arg1, (FloatArg)volume, (FloatArg)stereo);
        }

        public static bool UpdateSound3D(int sndIDPtr, zTSound3DParams param)
        {
            return Process.THISCALL<BoolArg>(GetZSound(), 0x4F2410, (IntArg)sndIDPtr, param);
        }

        public static void SetMasterVolume(float value)
        {
            Process.THISCALL<NullReturnCall>(GetZSound(), 0x004ED8E0, new FloatArg(value));
        }

        public static float GetMasterVolume()
        {
            throw new NotSupportedException("Float return!");
            return Process.THISCALL<FloatArg>(GetZSound(), 0x004ED730);
        }

        public static bool IsSoundActive(int sndIDPtr)
        {
            return Process.THISCALL<BoolArg>(GetZSound(), 0x4F3FD0, (IntArg)sndIDPtr);
        }
    }
}
