using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GUC.Scripting;

namespace GUC.Server
{
    static class Program
    {
        static void Main(string[] args)
        {
            TCPStatus.Start();

            try
            {
                Network.Server.Start();

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

                    Network.Server.Update(); //process received packets
                    GUCTimer.Update();

                    watch.Stop();

                    if (nextInfoUpdates < DateTime.UtcNow.Ticks)
                    {
                        tickAverage /= tickCount;
                        Log.Logger.Log(String.Format("Tick rate info: {0}ms average, {1}ms max. Allocated RAM: {2:0.0}MB", tickAverage / TimeSpan.TicksPerMillisecond, tickMax / TimeSpan.TicksPerMillisecond, (double)Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d));
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
                        Thread.Sleep(diff); //remove?
                    }
                }
            }
            catch (Exception e)
            {
                Log.Logger.LogError(e.Source + "<br>" + e.Message + "<br>" + e.StackTrace);
                Log.Logger.LogError("InnerException: " + e.InnerException.Source + "<br>" + e.InnerException.Message);
            }
            Console.Read();
        }
    }
}
