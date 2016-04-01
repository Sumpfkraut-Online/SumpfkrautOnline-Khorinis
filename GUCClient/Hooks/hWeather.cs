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

        static HookInfos rainHook;
        public static void AddHooks()
        {
            Process.Hook(Program.GUCDll, typeof(hWeather).GetMethod("hook_ResetTime"), 0x005EB02A, 6, 0);
            rainHook = Process.Hook(Program.GUCDll, typeof(hWeather).GetMethod("hook_ParticleMaximum"), 0x005E1F0F, 6, 0);

            addr1 = rainHook.oldFuncInNewFunc.ToInt32();
            addr2 = 0x005E1F78;
            Process.Write(new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0x90 }, addr1);
            Process.Write(new byte[] { 0xB8, 0x00, 0x00, 0x00, 0x00, 0x90 }, addr2);
            addr1++;
            addr2++;

            Process.Write(new byte[] { 0xE9, 0x8D, 0x00, 0x00, 0x00 }, 0x005EAF54); // jmp over rain weight calculation
        }

        static int addr1;
        static int addr2;

        public static Int32 hook_ResetTime(String message)
        {
            try
            {
                lastTime = 0;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }

        static long lastTime = 0;
        public static Int32 hook_ParticleMaximum(String message)
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

                Process.Write(num, addr1);
                Process.Write(num, addr2);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
    }
}
