using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using GUC.Scripting;
using System.Reflection;
using System.IO;
using GUC.Client.Scripts.Menus;
using GUC.Client.Scripts.Menus.MainMenus;
using GUC.Client.Scripts;

namespace GUC.Scripts
{
    public class Init : ScriptInterface
    {
        public static bool Ingame = false;

        public Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            Logger.Log("ClientScripts loaded!");
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
            GUCMenu activeMenu = GUCMenu.GetActiveMenus().ElementAtOrDefault(0);
            if (activeMenu != null)
            {
                activeMenu.Update(ticks);
            }
        }

        public void StartOutgame()
        {
            InputControl.Init();
            MainMenu.Menu.Open();
            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
            InputControl.Init();
            Ingame = true;
            Logger.Log("Ingame started.");
        }
    }
}
