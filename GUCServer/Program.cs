using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using GUC.Scripting;
using GUC.Log;
using GUC.Network;
using GUC.Options;
using GUC.Utilities;

namespace GUC
{
    public static class Program
    {
        private static long updateRate = 15 * TimeSpan.TicksPerMillisecond;
        public static long UpdateRate { get { return updateRate; } }

        private static long timeTillNextUpdate = 0L;
        public static long TimeTillNextUpdate { get { return timeTillNextUpdate; } }

        private static TimeStat timeAll = new TimeStat();
        public static long CurrentElapsedTicks { get { return timeAll.Ticks; } }

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

        static Thread server;
        static Thread tcpListener;
        static void Main(string[] args)
        {
            try
            {
                ServerOptions.Load();
                Console.Title = ServerOptions.ServerName;

                ScriptManager.StartScripts("Scripts\\ServerScripts.dll");

                server = new Thread(RunServer);
                server.Start();

                tcpListener = new Thread(TCPListener.Run);
                tcpListener.Start();

                Logger.RunLog();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Source + "<br>" + e.Message + "<br>" + e.StackTrace);
                Logger.LogError("InnerException: " + e.InnerException.Source + "<br>" + e.InnerException.Message);
            }
            Console.ReadLine();
        }

        public delegate void OnTickEventHandler();
        public static event OnTickEventHandler OnTick;
        static void RunServer()
        {
            try
            {
                TimeStat timeAll = new TimeStat();
                LockTimer perfUpdateTimer = new LockTimer(60 * 1000);
                perfUpdateTimer.Trigger();

                while (true)
                {
                    timeAll.Start();

                    GameTime.Update();
                    OnTick?.Invoke();

                    GameTime.Update();
                    GUCTimer.Update(GameTime.Ticks); // move to new thread?
                    
                    GameTime.Update();
                    GameServer.Update(); //process received packets

                    GameTime.Update();
                    WorldObjects.World.ForEach(w => w.OnTick(GameTime.Ticks));

                    long elapsed = timeAll.Stop();
                    if (perfUpdateTimer.IsReady)
                    {
                        Logger.Log("Performance: {0:0}ms avg, {1:0}ms max. RAM: {2:0.0}MB", timeAll.Average, timeAll.Maximum, Process.GetCurrentProcess().PrivateMemorySize64 / 1000000d);
                        timeAll.Reset();
                    }

                    long diff = (updateRate - elapsed) / TimeSpan.TicksPerMillisecond;
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
