using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace GUC.Client
{
    public static class SoundHandler
    {
        public enum MusicType
        {
            Normal,
            Threat,
            Fight
        }

        static MusicType type = MusicType.Normal;
        public static void PlayMusicType(MusicType musicType)
        {
            if (musicType == type)
                return;

            musicType = type;
            Process.Write(new byte[] { 0xB8, (byte)musicType, 0x00, 0x00, 0x00, 0xC3 }, 0x6C2D10);
        }
    }
}
