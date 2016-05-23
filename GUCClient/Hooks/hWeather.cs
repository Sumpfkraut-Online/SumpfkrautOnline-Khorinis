using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Log;
using GUC.WorldObjects;

namespace GUC.Client.Hooks
{
    static class hWeather
    {
        const int MaxDropsPerFrame = 128;

        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            var h = Process.AddHook(hook_ParticleMaximum, 0x005E2049, 8, 0);
            Process.Write(BitConverter.GetBytes(0x005E23EC - (h.OldInNewAddress + 8)), h.OldInNewAddress + 4); // readjust JLE

            Process.Write(new byte[] { 0xE9, 0x8D, 0x00, 0x00, 0x00 }, 0x005EAF54); // jmp over rain weight calculation
        }

        static long lastTime = 0;
        static void hook_ParticleMaximum(Hook hook)
        {
            try
            {
                long now = GameTime.Ticks;

                int num;
                if (lastTime > 0)
                {
                    long diff = now - lastTime;
                    double t = 10500d / World.current.SkyCtrl.CurrentWeight;

                    num = (int)(diff / t);
                    lastTime = now - (int)(diff % t);
                }
                else
                {
                    num = 1;
                    lastTime = now;
                }

                if (num > MaxDropsPerFrame)
                    num = MaxDropsPerFrame;

                hook.SetEDI(num);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
