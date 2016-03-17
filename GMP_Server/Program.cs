using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GUC.Scripting;
using GUC.Log;
using GUC.Server.Network;

namespace GUC.Server
{
    static class Program
    {
        static GUC.WorldObjects.NPC test;

        static void Main(string[] args)
        {
            //TCPStatus.Start();

            try
            {


                GameServer.Start();

                ScriptManager.StartScripts("Scripts\\ServerScripts.dll");

                const int updateRate = 20; //time between server ticks

                const long nextInfoUpdateTime = 60 * TimeSpan.TicksPerSecond;
                long nextInfoUpdates = DateTime.UtcNow.Ticks + nextInfoUpdateTime;

                long tickAverage = 0;
                long tickCount = 0;
                long tickMax = 0;
                
                Stopwatch watch = new Stopwatch();
                while (true)
                {
                    watch.Restart();

                    GUCTimer.Update(); // move to new thread?
                    GameServer.Update(); //process received packets

                    if (nextInfoUpdates < DateTime.UtcNow.Ticks)
                    {
                        tickAverage /= tickCount;
                        Logger.Log("Tick rate info: {0:0.00}ms average, {1:0.00}ms max. Allocated RAM: {2:0.0}MB", (double)tickAverage / TimeSpan.TicksPerMillisecond, (double)tickMax / TimeSpan.TicksPerMillisecond, Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d);
                        nextInfoUpdates = DateTime.UtcNow.Ticks + nextInfoUpdateTime;
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
                Logger.LogError("InnerException: " + e.InnerException.Source + "<br>" + e.InnerException.Message);
            }
            Console.ReadLine();
        }
    }
}
