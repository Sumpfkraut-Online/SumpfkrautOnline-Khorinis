using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GUC.Scripting;
using GUC.Log;
using GUC.Network;

namespace GUC
{
    static class Program
    {
        static Thread game;
        static void Main(string[] args)
        {
            try
            {
                GameServer.Start();

                ScriptManager.StartScripts("Scripts\\ServerScripts.dll");

                game = new Thread(RunServer);
                game.Start();

                Logger.RunLog();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Source + "<br>" + e.Message + "<br>" + e.StackTrace);
                Logger.LogError("InnerException: " + e.InnerException.Source + "<br>" + e.InnerException.Message);
            }
            Console.ReadLine();
        }

        static void RunServer()
        {
            try
            {
                const int updateRate = 8; //min time between server ticks

                const long nextInfoUpdateTime = 3 * TimeSpan.TicksPerMinute;
                long nextInfoUpdates = DateTime.UtcNow.Ticks + nextInfoUpdateTime;

                long tickAverage = 0;
                long tickCount = 0;
                long tickMax = 0;

                Random rand = new Random();
                Stopwatch watch = new Stopwatch();

                while (true)
                {
                    watch.Restart();

                    GameTime.Update();
                    GUC.WorldObjects.World.ForEach(w => w.OnTick(GameTime.Ticks));
                    GUCTimer.Update(GameTime.Ticks); // move to new thread?
                    GameServer.Update(); //process received packets

                    if (nextInfoUpdates < GameTime.Ticks)
                    {
                        tickAverage /= tickCount;
                        Logger.Log("Tick rate info: {0:0.00}ms average, {1:0.00}ms max. Allocated RAM: {2:0.0}MB", (double)tickAverage / TimeSpan.TicksPerMillisecond, (double)tickMax / TimeSpan.TicksPerMillisecond, Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d);
                        nextInfoUpdates = GameTime.Ticks + nextInfoUpdateTime;
                        tickMax = 0;
                        tickAverage = 0;
                        tickCount = 0;
                    }

                    tickCount++;
                    tickAverage += watch.ElapsedTicks;
                    if (watch.ElapsedTicks > tickMax)
                    {
                        tickMax = watch.ElapsedTicks;
                    }

                    int diff = updateRate - (int)watch.ElapsedMilliseconds;
                    if (diff > 0)
                    {
                        Thread.Sleep(diff);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e.Source + "<br>" + e.Message + "<br>" + e.StackTrace);
            }
        }
    }
}
