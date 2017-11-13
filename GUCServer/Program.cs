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
using System.Collections;

namespace GUC
{
    public static class Program
    {
        private static long updateRate = 0L;
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
                const long updateRate = 15 * TimeSpan.TicksPerMillisecond; //min time between server ticks

                const long nextInfoUpdateInterval = 1 * TimeSpan.TicksPerMinute;
                long nextInfoUpdateTime = GameTime.Ticks + nextInfoUpdateInterval;

                TimeStat timeAll = new TimeStat();
                while (true)
                {
                    timeAll.Start();

                    GameTime.Update();
                    OnTick?.Invoke();

                    GUCTimer.Update(GameTime.Ticks); // move to new thread?
                    GameServer.Update(); //process received packets
                    //WorldObjects.World.UpdateWorlds(GameTime.Ticks);
                    WorldObjects.World.ForEach(w => w.OnTick(GameTime.Ticks));

                    if (nextInfoUpdateTime < GameTime.Ticks)
                    {
                        Logger.Log("Performance: {0:0.00}ms avg, {1:0.00}ms max. RAM: {2:0.0}MB", timeAll.Average, timeAll.Maximum, Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d);
                        timeAll.Reset();
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
