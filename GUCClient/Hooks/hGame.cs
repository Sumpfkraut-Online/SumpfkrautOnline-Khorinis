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
using GUC.Client.GUI;

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
            //Process.Hook(Program.GUCDll, typeof(hGame).GetMethod("RunIngame"), 0x006C876B, 6, 0);

            Logger.Log("Added game loop hooks.");
        }

        static bool outgameStarted = false;

        static GUCVisual connectionVis = null;
        static bool ShowConnectionAttempts(GameClient client)
        {
            if (!client.IsConnecting)
            {
                if (connectionVis != null)
                {
                    connectionVis.Hide();
                }
                return !client.IsConnected;
            }
            else
            {
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
                connectionVis.Texts[0].Text = String.Format("Connecting to {0} ... ({1})", client.ServerAddress, client.ConnectionAttempts + 1);
                connectionVis.Show();
                return true;
            }
        }

        static System.Diagnostics.Stopwatch fpsWatch = new System.Diagnostics.Stopwatch();
        public static Int32 RunOutgame(String message)
        {
            try
            {
                var client = GameClient.Client;
                if (client == null) return 0;

                GameTime.Update();
                GUCTimer.Update(GameTime.Ticks);
                InputHandler.Update();

                if (!ShowConnectionAttempts(client))
                {
                    if (!outgameStarted)
                    {
                        outgameStarted = true;
                        ScriptManager.Interface.StartOutgame();
                    }

                    client.Update();
                    ScriptManager.Interface.Update(GameTime.Ticks);
                }

                #region Gothic 
                int address = Convert.ToInt32(message);
                int CGameMngerAddress = Process.ReadInt(address);
                int arg = Process.ReadInt(address + 4);

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

            return 0;
        }

        static bool ingameStarted = false;
        public static Int32 RunIngame(String message)
        {
            try
            {
                var client = GameClient.Client;
                if (client == null) return 0;

                GameTime.Update();
                GUCTimer.Update(GameTime.Ticks);
                InputHandler.Update();

                if (!ShowConnectionAttempts(client))
                {
                    if (!ingameStarted)
                    {
                        ingameStarted = true;
                        ScriptManager.Interface.StartIngame();
                    }

                    WorldObjects.World.ForEach(w => { w.Clock.UpdateTime(); w.SkyCtrl.UpdateWeather(); });
                    client.Update();
                    ScriptManager.Interface.Update(GameTime.Ticks);
                    
                    if (client.IsSpectating)
                    {
                        client.UpdateSpectator(GameTime.Ticks);
                    }
                    client.UpdateCharacters(GameTime.Ticks);
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
            return 0;
        }
    }
}
