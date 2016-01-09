using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GUC.Scripting;
using GUC.Log;
using GUC.Options;
using System.Reflection;
using WinApi;
using Gothic;

namespace GUC.Client
{
    class Program
    {
        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));
            return Assembly.LoadFrom(ClientPaths.GUCDlls + name + ".dll");
        }

        public static Int32 InjectedMain(String message)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

                //SplashScreen.SetUpHooks();
                //SplashScreen.Create();

                Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)

                Hooks.hParser.Init();

                // hook the outgame menu
                var hi = Process.Hook(Constants.GUCDll, typeof(Program).GetMethod("RunOutgame"), 0x004292D0, 7, 2);
                Process.Write(new byte[] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90, 0x90 }, hi.oldFuncInNewFunc.ToInt32());

                Thread.Sleep(3000); // Wait a bit for Gothic to start

                ClientPaths.CreateFolders(); // Set up folders
                BaseOptions.Load(ClientPaths.GUCConfig + "guc.xml"); // Load Options

                ScriptManager.StartScripts(ClientPaths.GUCDlls + "ClientScripts.dll"); // Load Scripts

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Environment.CurrentDirectory + " " + ex.Source + " " + ex.Message + " " + ex.StackTrace);
            }
            return 0;
        }

        static long next = 0;

        static zCView view = null;
        public static Int32 RunOutgame(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                int CGameMngerAddress = Process.ReadInt(address);
                int arg = Process.ReadInt(address + 4);

                if (view == null)
                {
                    Process.THISCALL<NullReturnCall>(CGameMngerAddress, 0x427090); // PreMenu();

                    view = zCView.Create(0, 0, 1000, 1000);
                    view.InsertBack("Adanos_Stoneplate_01.tga");
                    zCView.GetScreen().InsertItem(view, 1);
                }

                Process.CDECLCALL<NullReturnCall>(0x5053E0); // void __cdecl sysEvent(void)
                Process.CDECLCALL<NullReturnCall>(0x7A55C0); // public: static void __cdecl zCInputCallback::GetInput(void)

                if (next < DateTime.UtcNow.Ticks)
                {
                    using (zColor color = zColor.Create(0, 0, 0, 0))
                        zCRenderer.Vid_Clear(color, 3);

                    zCRenderer.BeginFrame();
                    zCView.GetScreen().Render();
                    zCRenderer.EndFrame();
                    zCRenderer.Vid_Blit(1, 0, 0);
                    zCSoundSystem.DoSoundUpdate();
                    
                    next = DateTime.UtcNow.Ticks + 33 * TimeSpan.TicksPerMillisecond;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            return 0;
        }
    }
}
