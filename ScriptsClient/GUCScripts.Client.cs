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
using GUC.Scripts.Sumpfkraut.Controls;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public static bool Ingame = false;

        partial void pConstruct()
        { 
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            InputControl.Active = new Arena.Controls.ArenaControl();
            
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

        public static event Action<long> OnUpdate;
        public void Update(long ticks)
        {
            GUCMenu.UpdateMenus(ticks);
            InputControl.UpdateControls(ticks);
            OnUpdate?.Invoke(ticks);
            //CheckMusic();
        }

        public void StartOutgame()
        { 
            // not needed anymore, we do the hit feedback ourselves
            // always do T_GOTHIT instead of T_STUMBLE/B when getting hit, so animation don't interrupt too much
            // WinApi.Process.Write(0x0067836C, 0xE9, 0x99, 0x04, 0x00, 0x00);
                  
            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
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
