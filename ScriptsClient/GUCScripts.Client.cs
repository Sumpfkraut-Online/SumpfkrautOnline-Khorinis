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
            Sumpfkraut.Controls.InputControl.Update(ticks);
            //CheckMusic();
        }

        public void StartOutgame()
        {
            //WinApi.Process.Write(new byte[] { 0xE9, 0x99, 0x04, 0x00, 0x00 }, 0x0067836C); // always do T_GOTHIT instead of T_STUMBLE/B when getting hit

            Left4Gothic.CharCreationMenu.Menu.Open();
            Sumpfkraut.Controls.InputControl.Init();
            

            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
            Sumpfkraut.Controls.InputControl.Init();

            Gothic.Objects.oCNpcFocus.SetFocusMode(1);
            GUCMenu.CloseActiveMenus();
            Ingame = true;
            Logger.Log("Ingame started.");

        }

        /*const long FightMusicTime = 50 * TimeSpan.TicksPerSecond;
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
        }*/
    }
}
