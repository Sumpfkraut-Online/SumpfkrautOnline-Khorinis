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
        class TimeStat
        {
            long tickCount;
            long tickMax;
            long counter;
            Stopwatch watch;

            public long Ticks { get { return this.tickCount; } }
            public double Average { get { return (double)tickCount / (double)counter / (double)TimeSpan.TicksPerMillisecond; } }
            public double Maximum { get { return (double)tickMax / (double)TimeSpan.TicksPerMillisecond; } }

            public TimeStat()
            {
                watch = new Stopwatch();
                this.Reset();
            }

            public void Start()
            {
                watch.Restart();
            }

            public long Stop()
            {
                watch.Stop();
                long ticks = watch.Elapsed.Ticks;

                this.tickCount += ticks;
                counter++;

                if (ticks > tickMax)
                {
                    tickMax = ticks;
                }
                return ticks;
            }

            public void Reset()
            {
                tickCount = 0;
                counter = 0;
                tickMax = 0;
            }
        }

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
                const long updateRate = 15 * TimeSpan.TicksPerMillisecond; //min time between server ticks
                const long idleTimeSpan = 140000;

                const long nextInfoUpdateInterval = 1 * TimeSpan.TicksPerMinute;
                long nextInfoUpdateTime = GameTime.Ticks + nextInfoUpdateInterval;

                Stopwatch gcWatch = new Stopwatch();
                TimeStat timeAll = new TimeStat();
                while (true)
                {
                    timeAll.Start();

                    GameTime.Update();
                    WorldObjects.World.ForEach(w => w.OnTick(GameTime.Ticks));
                    GUCTimer.Update(GameTime.Ticks); // move to new thread?
                    GameServer.Update(); //process received packets

                    if (nextInfoUpdateTime < GameTime.Ticks)
                    {
                        Logger.Log("Performance info: {0:0.00}ms average, {1:0.00}ms max. Allocated RAM: {2:0.0}MB", timeAll.Average, timeAll.Maximum, Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d);
                        timeAll.Reset();
                        nextInfoUpdateTime = GameTime.Ticks + nextInfoUpdateInterval;
                    }

                    long diff = updateRate - timeAll.Stop();
                    if (diff >= idleTimeSpan) // server is idling pretty much
                    {
                        gcWatch.Start();
                        GC.Collect(); // do garbage collecting
                        gcWatch.Stop();
                        if (gcWatch.Elapsed.Ticks > idleTimeSpan)
                        {
                            Logger.Log("Forced GC Collection took way too long! {0:0.00}ms > {1:0.00}ms (Remove it?)", gcWatch.Elapsed.TotalMilliseconds, idleTimeSpan / (double)TimeSpan.TicksPerMillisecond);
                        }
                        diff -= gcWatch.ElapsedMilliseconds;
                        gcWatch.Reset();
                    }

                    diff /= TimeSpan.TicksPerMillisecond;
                    if (diff > 0)
                    {
                        Thread.Sleep((int)diff);
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
