using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Log;
using GUC.Network;
using WinApi;
using Gothic.Types;
using Gothic.System;
using Gothic.View;
using Gothic.Sound;
using System.Threading;
using GUC.GUI;
using GUC.WorldObjects;

namespace GUC.Hooks
{
    static class hGame
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            // hook outgame loop and kick out the original menus
            var h = Process.AddHook(RunOutgame, 0x004292D0, 7, 1);
            Process.Write(new byte[7] { 0xC2, 0x04, 0x00, 0x90, 0x90, 0x90, 0x90 }, h.OldInNewAddress);

            // hook ingame loop
            Process.AddHook(RunIngame, 0x006C86A0, 7, 0);

            Logger.Log("Added game loop hooks.");
        }

        static bool outgameStarted = false;

        static GUCVisual connectionVis = null;
        static bool ShowConnectionAttempts()
        {
            if (GameClient.IsDisconnected)
                return true;

            if (!GameClient.IsConnected)
            {
                if (!GameClient.IsConnecting)
                {
                    GameClient.Connect();
                }

                if (connectionVis == null)
                {
                    connectionVis = new GUCVisual();
                    connectionVis.SetBackTexture("MENU_CHOICE_BACK.TGA");
                    var text = connectionVis.CreateText("");
                }
                int[] screenSize = GUCView.GetScreenSize();
                connectionVis.SetPosX(screenSize[0] / 2 - 200);
                connectionVis.SetPosY(200);
                connectionVis.SetSizeY(40);
                connectionVis.SetSizeX(400);
                connectionVis.Texts[0].Text = string.Format("Connecting to '{0}:{1}' ... ({2})", Program.ServerIP, Program.ServerPort, GameClient.ConnectionAttempts);
                connectionVis.Show();
                return true;
            }

            if (connectionVis != null)
            {
                connectionVis.Hide();
            }
            return !GameClient.IsConnected;
        }

        static System.Diagnostics.Stopwatch fpsWatch = new System.Diagnostics.Stopwatch();
        static void RunOutgame(Hook hook)
        {
            try
            {
                GameTime.Update();
                GUCTimer.Update(GameTime.Ticks);
                GameClient.Update();
                InputHandler.Update();
                
                if (!ShowConnectionAttempts())
                {
                    if (!outgameStarted)
                    {
                        outgameStarted = true;
                        ScriptManager.Interface.StartOutgame();
                    }

                    ScriptManager.Interface.Update(GameTime.Ticks);
                }

                #region Gothic 

                Process.CDECLCALL<NullReturnCall>(0x5053E0); // void __cdecl sysEvent(void)

                using (zColor color = zColor.Create(0, 0, 0, 0))
                    zCRenderer.Vid_Clear(color, 3);

                zCRenderer.BeginFrame();
                zCView.GetScreen().Render();
                zCRenderer.EndFrame();
                zCRenderer.Vid_Blit(1, 0, 0);
                zCSndSys_MSS.DoSoundUpdate();

                #endregion

                if (fpsWatch.IsRunning)
                {
                    long diff = 8 * TimeSpan.TicksPerMillisecond - fpsWatch.ElapsedTicks;
                    if (diff > 0)
                    {
                        Thread.Sleep((int)(diff / TimeSpan.TicksPerMillisecond));
                    }
                }
                fpsWatch.Restart();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        static bool ingameStarted = false;
        static void RunIngame(Hook hook)
        {
            try
            {
                GameTime.Update();
                GUCTimer.Update(GameTime.Ticks);
                GameClient.Update();
                InputHandler.Update();
                SoundHandler.Update3DSounds();

                if (!ShowConnectionAttempts())
                {
                    if (!ingameStarted)
                    {
                        ingameStarted = true;
                        ScriptManager.Interface.StartIngame();
                    }

                    ScriptManager.Interface.Update(GameTime.Ticks);

                    if (GameClient.Client.IsIngame)
                    {
                        World.UpdateWorlds(GameTime.Ticks);
                    }

                    GameClient.UpdateSpectator(GameTime.Ticks);
                    NPC.UpdateHero(GameTime.Ticks);
                }

                if (fpsWatch.IsRunning)
                {
                    long diff = 8 * TimeSpan.TicksPerMillisecond - fpsWatch.ElapsedTicks;
                    if (diff > 0)
                    {
                        Thread.Sleep((int)(diff / TimeSpan.TicksPerMillisecond));
                    }
                }
                fpsWatch.Restart();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }
    }
}
