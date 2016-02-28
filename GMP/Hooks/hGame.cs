using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Log;
using Gothic;
using GUC.Network;
using WinApi;
using Gothic.Types;
using Gothic.System;
using Gothic.View;
using Gothic.Objects.Sky;

namespace GUC.Client.Hooks
{
    static class hGame
    {
        public static void AddHooks()
        {
            // hook outgame loop and kick out the original menus
            var hi = Process.Hook(Program.GUCDll, typeof(hGame).GetMethod("RunOutgame"), 0x004292D0, 7, 2);
            Process.Write(new byte[] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90, 0x90 }, hi.oldFuncInNewFunc.ToInt32());

            // hook ingame loop
            Process.Hook(Program.GUCDll, typeof(hGame).GetMethod("RunIngame"), 0x006C86A0, 7, 0);

            Logger.Log("Added game loop hooks.");
        }

        static long next = 0;
        static bool outgameStarted = false;
        public static Int32 RunOutgame(String message)
        {
            try
            {
                if (!outgameStarted)
                {
                    outgameStarted = true;
                    ScriptManager.StartScripts(Program.ProjectPath + "Scripts\\ClientScripts.dll"); // Load Scripts
                    ScriptManager.Interface.StartOutgame();
                }

                if (next < DateTime.UtcNow.Ticks)
                {
                    GameClient.Client.Update();
                    InputHandler.Update();
                    ScriptManager.Interface.Update(DateTime.UtcNow.Ticks);

                    if (InputHandler.IsPressed(WinApi.User.Enumeration.VirtualKeys.F1))
                    {
                        oCGame.LoadGame(true, "NEWWORLD\\NEWWORLD.ZEN");
                    }

                    #region Gothic 
                    int address = Convert.ToInt32(message);
                    int CGameMngerAddress = Process.ReadInt(address);
                    int arg = Process.ReadInt(address + 4);

                    Process.CDECLCALL<NullReturnCall>(0x5053E0); // void __cdecl sysEvent(void)
                    Process.CDECLCALL<NullReturnCall>(0x7A55C0); // public: static void __cdecl zCInputCallback::GetInput(void)

                    using (zColor color = zColor.Create(0, 0, 0, 0))
                        zCRenderer.Vid_Clear(color, 3);

                    zCRenderer.BeginFrame();
                    zCView.GetScreen().Render();
                    zCRenderer.EndFrame();
                    zCRenderer.Vid_Blit(1, 0, 0);
                    zCSoundSystem.DoSoundUpdate();
                    #endregion

                    next = DateTime.UtcNow.Ticks + 33 * TimeSpan.TicksPerMillisecond;
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }

            return 0;
        }



        static bool ingameStarted = false;
        public static Int32 RunIngame(String message)
        {
            try
            {
                if (!ingameStarted)
                {
                    ingameStarted = true;
                    ScriptManager.Interface.StartIngame();
                }
                
                GameClient.Client.Update();
                InputHandler.Update();
                ScriptManager.Interface.Update(DateTime.UtcNow.Ticks);

                GameClient.Client.UpdateCharacters();

                if (InputHandler.IsPressed(WinApi.User.Enumeration.VirtualKeys.F4))
                {
                    Program.Exit();
                }


                /*if ((WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F1) & 0x8001) == 0x8001 || (WinApi.User.Input.GetAsyncKeyState(WinApi.User.Enumeration.VirtualKeys.F1) & 0x8000) == 0x8000)
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
                }*/
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
            return 0;
        }
    }
}
