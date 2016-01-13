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
using Gothic.Types;
using Gothic.System;
using Gothic.Objects;

namespace GUC.Client
{
    class Program
    {
        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            if (name.ToUpper() == "GUC.RESOURCES")
            {
                //load from resource
                var resxStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                byte[] buffer = new byte[resxStream.Length];
                resxStream.Read(buffer, 0, (int)resxStream.Length);
                return Assembly.Load(buffer);
            }
            return Assembly.LoadFrom(ClientPaths.GUCDlls + name + ".dll");
        }

        public static Int32 InjectedMain(String message)
        {
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
                
                SplashScreen.SetUpHooks();
                SplashScreen.Create();

                Process.Write(new byte[] { 0xE9, 0x8C, 0x00, 0x00, 0x00 }, 0x0044AEDF); // skip visual vdfs init (vdfs32g.exe)

                Hooks.hParser.Init();

                // hook the outgame menu
                var hi = Process.Hook(Constants.GUCDll, typeof(Program).GetMethod("RunOutgame"), 0x004292D0, 7, 2);
                Process.Write(new byte[] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90, 0x90 }, hi.oldFuncInNewFunc.ToInt32());

                Process.Hook(Constants.GUCDll, typeof(Program).GetMethod("RunIngame"), 0x006C86A0, 7, 0);
                
                //Process.Write(new byte[] { 0xC2, 0x04, 0x00 }, 0x00626570); // kick out zCWorld::UnarcTraverseVobs
                Process.Write((byte)0xC3, 0x006C1F60); //Blocking Call Init Scripts!
                Process.Write((byte)0xC3, 0x006C1C70); //Blocking Call Startup Scripts!
                
                Process.Write((byte)0xC3 , 0x00780D80);//Blocking time!

                // hook hero creating
                Process.Write(new byte[] { 0xE9, 0xBD, 0x00, 0x00, 0x00 }, 0x006C434B);
                hi = Process.Hook(Constants.GUCDll, typeof(Program).GetMethod("CreatePlayerVob"), 0x006C440D, 5, 0);
                Process.Write(new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }, hi.oldFuncInNewFunc.ToInt32());
                Process.Write(new byte[] { 0xEB, 0x67 }, 0x006C4662); //skip instance check

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

        public static Int32 RunIngame(String message)
        {
            try
            {
                if ((WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F1) & 0x8001) == 0x8001 || (WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F1) & 0x8000) == 0x8000)
                {
                    //Process.THISCALL<NullReturnCall>(Process.ReadInt(oCGame.ogame), 0x6C9A50); //oCGame::Compile

                    IntPtr ptr = Process.Alloc(4);
                    Process.Write(2, ptr.ToInt32());

                    int world = Process.ReadInt(Process.ReadInt(oCGame.ogame) + 8);

                    Process.THISCALL<IntArg>(world, 0x62FB70, new IntArg(ptr.ToInt32()), new IntArg(0));

                    Process.Free(ptr, 4);
                }

                if ((WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F2) & 0x8001) == 0x8001 || (WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F2) & 0x8000) == 0x8000)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        var vob = zCVob.Create();
                        vob.SetVisual("OW_Forest_Tree_V1.3ds");

                        int world = Process.ReadInt(Process.ReadInt(oCGame.ogame) + 8);
                        Process.THISCALL<IntArg>(world, 0x624810, vob);
                    }
                }

                
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }

        static long next = 0;
        public static Int32 RunOutgame(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);
                int CGameMngerAddress = Process.ReadInt(address);
                int arg = Process.ReadInt(address + 4);

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

                if ((WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F1) & 0x8001) == 0x8001 || (WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F1) & 0x8000) == 0x8000)
                {
                    oCGame.LoadGame(true, "NEWWORLD\\NEWWORLD.ZEN");
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            return 0;
        }

        public static Int32 CreatePlayerVob(String message)
        {
            try
            {
                int address = Convert.ToInt32(message);

                oCNpc player = oCNpc.Create();
                player.SetVisual("HUMANS.MDS");
                player.SetAdditionalVisuals("hum_body_Naked0", 9, 0, "Hum_Head_Pony", 1, 0, -1);

                player.HPMax = 100;
                player.HP = 100;

                Process.Write(player.Address, address + 4); //write address into eax
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
    }
}
