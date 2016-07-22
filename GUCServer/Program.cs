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
                const long updateRate = 10 * TimeSpan.TicksPerMillisecond; //min time between server ticks

                const long nextInfoUpdateInterval = 10*TimeSpan.TicksPerSecond;
                long nextInfoUpdateTime = GameTime.Ticks + nextInfoUpdateInterval;

                TimeStat timeAll = new TimeStat();
                TimeStat timeWorlds = new TimeStat();
                TimeStat timeTimers = new TimeStat();
                TimeStat timePackets = new TimeStat();

                while (true)
                {
                    timeAll.Start();

                    GameTime.Update();

                    timeWorlds.Start();
                    WorldObjects.World.ForEach(w => w.OnTick(GameTime.Ticks));
                    timeWorlds.Stop();

                    timeTimers.Start();
                    GUCTimer.Update(GameTime.Ticks); // move to new thread?
                    timeTimers.Stop();

                    timePackets.Start();
                    GameServer.Update(); //process received packets
                    timePackets.Stop();

                    if (nextInfoUpdateTime < GameTime.Ticks)
                    {
                        Logger.Log("");
                        Logger.Log("Performance info: {0:0.00}ms average, {1:0.00}ms max. Allocated RAM: {2:0.0}MB", timeAll.Average, timeAll.Maximum, Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d);
                        Logger.Log("World & Vobs ({0}%): {1:0.00}ms average, {2:0.00}ms max.", 100 * timeWorlds.Ticks / timeAll.Ticks, timeWorlds.Average, timeWorlds.Maximum);
                        Logger.Log("Timers ({0}%): {1:0.00}ms average, {2:0.00}ms max.", 100 * timeTimers.Ticks / timeAll.Ticks, timeTimers.Average, timeTimers.Maximum);
                        Logger.Log("Packets ({0}%): {1:0.00}ms average, {2:0.00}ms max.", 100 * timePackets.Ticks / timeAll.Ticks, timePackets.Average, timePackets.Maximum);
                        Logger.Log("");

                        timeAll.Reset();
                        timeWorlds.Reset();
                        timeTimers.Reset();
                        timePackets.Reset();

                        nextInfoUpdateTime = GameTime.Ticks + nextInfoUpdateInterval;
                    }

                    long diff = (updateRate - timeAll.Stop()) / TimeSpan.TicksPerMillisecond;
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
