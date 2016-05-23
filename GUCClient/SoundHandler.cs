using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace GUC.Client
{
    public static class SoundHandler
    {
        static bool playFightMusic = false;
        public static void SetPlayFightMusic(bool play)
        {
            if (play != playFightMusic)
            {
                playFightMusic = play;
                if (playFightMusic)
                {
                    Process.Write(new byte[] { 0xB8, 0x02, 0x00, 0x00, 0x00, 0xC3 }, 0x6C2D10);
                }
                else
                {
                    Process.Write(new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0xC3 }, 0x6C2D10);
                }
            }
        }
    }
}
