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
using GUC.Utilities;

namespace GUC.Hooks
{
    static class hGame
    {
        static bool inited = false;
        public static void AddHooks()
        {
            if (inited) return;
            inited = true;

            // should never be called anyway, but just to be sure
            Process.AddHook((k, m) => GothicGlobals.UpdateGameAddress(), 0x426F5E, 0xB); // GameSessionInit
            Process.AddHook((k, m) => GothicGlobals.UpdateGameAddress(), 0x0042705B, 0xB); // GameSessionDone

            // hook outgame loop and kick out the original menus
            var h = Process.AddHook(RunOutgame, 0x004292D0, 7);
            Process.Write(h.OldInNewAddress, 0xC2, 0x04, 0x00);

            // hook ingame loop
            Process.AddHook(RunIngame, 0x6C86A0, 7); // before gothic's precache

            // first render
            Process.AddHook(FirstRender, 0x6C876B, 6);

            Logger.Log("Added game loop hooks.");
        }

        public static bool FirstRenderDone = false;
        static void FirstRender(Hook hook, RegisterMemory rmem)
        {
            if (FirstRenderDone) // improveme: remove & add hook
                return;

            ScriptManager.Interface.FirstWorldRender();
            FirstRenderDone = true;
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
                var screenSize = GUCView.GetScreenSize();
                connectionVis.SetPosX(screenSize.X / 2 - 200);
                connectionVis.SetPosY(200);
                connectionVis.SetHeight(40);
                connectionVis.SetWidth(400);
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
        static void RunOutgame(Hook hook, RegisterMemory rmem)
        {
            try
            {
                GameTime.Update();
                GUCTimer.Update(GameTime.Ticks);

                GameTime.Update();
                GameClient.Update();

                GameTime.Update();
                InputHandler.Update();

                if (!ShowConnectionAttempts())
                {
                    if (!outgameStarted)
                    {
                        outgameStarted = true;
                        VobRenderArgs.Init();
                        ScriptManager.Interface.StartOutgame();
                    }

                    GameTime.Update();
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
                    long diff = 8 * TimeSpan.TicksPerMillisecond - fpsWatch.Elapsed.Ticks;
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
        static void RunIngame(Hook hook, RegisterMemory rmem)
        {
            try
            {
                spikeWatch.Restart();
                // Update Timers
                GameTime.Update();
                GUCTimer.Update(GameTime.Ticks);

                // Do networking, read packets
                GameTime.Update();
                GameClient.Update();

                // handle peripherals' input
                GameTime.Update();
                InputHandler.Update();

                if (!ShowConnectionAttempts())
                {
                    if (!ingameStarted)
                    {
                        ingameStarted = true;
                        ScriptManager.Interface.StartIngame();
                    }

                    // ClientScripts update
                    GameTime.Update();
                    ScriptManager.Interface.Update(GameTime.Ticks);

                    if (GameClient.Client.IsIngame)
                    {
                        // Update worlds
                        GameTime.Update();
                        World.UpdateWorlds(GameTime.Ticks);
                    }

                    // Check spectator stuff
                    GameTime.Update();
                    GameClient.UpdateSpectator(GameTime.Ticks);

                    // check player hero
                    GameTime.Update();
                    NPC.UpdateHero(GameTime.Ticks);
                }

                // update guc sounds
                GameTime.Update();
                SoundHandler.Update3DSounds();

                spikeWatch.Stop();
                if (spikeTimer.IsReady)
                    spikeLongest = 0;
                if (spikeLongest < spikeWatch.Elapsed.Ticks)
                    spikeLongest = spikeWatch.Elapsed.Ticks;

                if (fpsWatch.IsRunning)
                {
                    long diff;
                    if ((diff = 8 * TimeSpan.TicksPerMillisecond - fpsWatch.Elapsed.Ticks) > 0)
                        Thread.Sleep((int)(diff / TimeSpan.TicksPerMillisecond));
                }
                lastElapsed = fpsWatch.Elapsed.Ticks;
                fpsWatch.Restart();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        static long lastElapsed = 0;
        public static long LastElapsedTicks { get { return lastElapsed; } }

        static System.Diagnostics.Stopwatch spikeWatch = new System.Diagnostics.Stopwatch();
        static long spikeLongest = 0;
        static LockTimer spikeTimer = new LockTimer(1000);
        public static long SpikeLongest { get { return spikeLongest; } }
    }
}
