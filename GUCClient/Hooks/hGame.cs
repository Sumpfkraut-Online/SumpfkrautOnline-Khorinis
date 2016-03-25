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

        static bool outgameStarted = false;

        static System.Diagnostics.Stopwatch fpsWatch = new System.Diagnostics.Stopwatch();
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

                GameTime.Update();

                GUCTimer.Update(GameTime.Ticks);
                GameClient.Client.Update();
                InputHandler.Update();
                ScriptManager.Interface.Update(GameTime.Ticks);

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

                if (fpsWatch.IsRunning)
                {
                    long diff = 8 * TimeSpan.TicksPerMillisecond - fpsWatch.ElapsedTicks;
                    if (diff > 0)
                    {
                        System.Threading.Thread.Sleep((int)(diff / TimeSpan.TicksPerMillisecond));
                    }
                }
                fpsWatch.Restart();
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
                
                GameTime.Update();
                WorldObjects.World.ForEach(w => w.Clock.UpdateTime());
                GUCTimer.Update(GameTime.Ticks);
                GameClient.Client.Update();
                InputHandler.Update();
                ScriptManager.Interface.Update(GameTime.Ticks);

                GameClient.Client.UpdateCharacters(GameTime.Ticks);

                if (InputHandler.IsPressed(WinApi.User.Enumeration.VirtualKeys.F4))
                {
                    Program.Exit();
                }

                if (InputHandler.IsPressed(WinApi.User.Enumeration.VirtualKeys.F6))
                {
                    int bitField = Process.ReadInt(GameClient.Client.Character.gVob.HumanAI.Address + 0x1204);
                    if ((bitField & 0x10) != 0)
                    {
                        bitField &= ~0x10;
                    }
                    else
                    {
                        bitField |= 0x10;
                    }
                    Process.Write(bitField, GameClient.Client.Character.gVob.HumanAI.Address + 0x1204);
                }

                if (fpsWatch.IsRunning)
                {
                    long diff = 8 * TimeSpan.TicksPerMillisecond - fpsWatch.ElapsedTicks;
                    if (diff > 0)
                    {
                        System.Threading.Thread.Sleep((int)(diff / TimeSpan.TicksPerMillisecond));
                    }
                }
                fpsWatch.Restart();

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
