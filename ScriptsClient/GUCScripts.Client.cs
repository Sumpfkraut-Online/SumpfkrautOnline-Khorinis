using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using GUC.Scripting;
using System.Reflection;
using System.IO;
using GUC.Scripts.Sumpfkraut.Menus;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public static bool Ingame = false;

        public GUCScripts()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            Logger.Log("SumpfkrautOnline ClientScripts loaded!");
        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            Assembly asm = Assembly.LoadFrom(Path.GetFullPath("System\\Multiplayer\\UntoldChapters\\SumpfkrautOnline\\" + name + ".dll"));
            if (asm == null)
            {
                asm = Assembly.LoadFrom(Path.GetFullPath("Multiplayer\\UntoldChapters\\SumpfkrautOnline\\" + name + ".dll"));
            }
            return asm;
        }

        public void Update(long ticks)
        {
            GUCMenu.UpdateMenus(ticks);
            TFFA.InputControl.Update(ticks);
            CheckMusic();
            CheckBarrier();
        }

        public void StartOutgame()
        {
            WinApi.Process.Write(new byte[] { 0xE9, 0x99, 0x04, 0x00, 0x00 }, 0x0067836C); // always do T_GOTHIT instead of T_STUMBLE/B when getting hit

            TFFA.InputControl.Init();
            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
            TFFA.InputControl.Init();
            TFFA.MainMenu.Menu.Open();
            TFFA.ChatMenu.Menu.Show();
            //GUCMenu.CloseActiveMenus();
            Ingame = true;
            Logger.Log("Ingame started.");
        }

        const long FightMusicTime = 50 * TimeSpan.TicksPerSecond;
        static void CheckMusic()
        {
            if (!Ingame || TFFA.TFFAClient.Info == null)
                return;

            if (TFFA.TFFAClient.Status == TFFA.TFFAPhase.Fight && TFFA.TFFAClient.PhaseEndTime > 0 && TFFA.TFFAClient.PhaseEndTime - GameTime.Ticks < FightMusicTime)
            {
                SoundHandler.CurrentMusicType = SoundHandler.MusicType.Fight;
            }
            else
            {
                SoundHandler.CurrentMusicType = SoundHandler.MusicType.Normal;
            }
        }

        static Random random = new Random("Scavenger".GetHashCode());
        static long barrierLastTime = GameTime.Ticks;
        static long barrierNextTime = GameTime.Ticks + random.Next(180, 300) * TimeSpan.TicksPerSecond;
        static int barrierStatus = 0;
        static void CheckBarrier()
        {
            long now = GameTime.Ticks;

            if (barrierNextTime <= now)
            {
                if (barrierStatus == 1) // fade in
                {
                    barrierStatus = 2;
                    barrierNextTime = now + random.Next(6, 16) * TimeSpan.TicksPerSecond; // enabled time
                    barrierLastTime = now;
                }
                else if (barrierStatus == 2) // enabled
                {
                    barrierStatus = 3;
                    barrierNextTime = now + random.Next(3, 10) * TimeSpan.TicksPerSecond; // fade out time
                    barrierLastTime = now;
                }
                else if (barrierStatus == 3) // fade out
                {
                    barrierStatus = 0;
                    barrierNextTime = now + random.Next(180, 300) * TimeSpan.TicksPerSecond; // disabled time
                    barrierLastTime = now;
                }
                else // disabled
                {
                    barrierStatus = 1;
                    barrierNextTime = now + random.Next(3, 10) * TimeSpan.TicksPerSecond; // fade in time
                    barrierLastTime = now;
                }
            }
            else
            {
                byte value = 0; // disabled
                if (barrierStatus == 1) // fade in
                {
                    value = (byte)(255d * (double)(now - barrierLastTime) / (double)(barrierNextTime - barrierLastTime));
                }
                else if (barrierStatus == 2) // enabled
                {
                    value = 255;
                }
                else if (barrierStatus == 3) // fade out
                {
                    value = (byte)(255d * (double)(barrierNextTime - now) / (double)(barrierNextTime - barrierLastTime));
                }

                Barrier.BarrierAlpha = value;
                if (barrierStatus == 1 || barrierStatus == 2 || (barrierStatus == 3 && value > 75))
                {
                    Barrier.PlaySound = true;
                }
                else
                {
                    Barrier.PlaySound = false;
                }
            }
        }
    }
}
