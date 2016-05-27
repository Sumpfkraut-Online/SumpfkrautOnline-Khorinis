using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Log;

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

        static MusicType currentMusicType = MusicType.Normal;
        public static void PlayMusicType(MusicType type)
        {
            if (currentMusicType == type)
                return;

            currentMusicType = type;
            Process.Write(new byte[] { 0xB8, (byte)type, 0x00, 0x00, 0x00, 0xC3 }, 0x6C2D10);

            Logger.Log("SoundHandler: Player music type " + currentMusicType);
        }
    }
}
