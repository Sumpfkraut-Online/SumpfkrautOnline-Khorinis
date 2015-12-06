using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Options;
using GUC.Types;
using System.Threading;
using GUC.Server.WorldObjects;
using System.Diagnostics;
using GUC.Network;

namespace GUC.Server
{
    class Program
    {
        internal static GUC.Options.ServerOptions serverOptions = null;
        internal static Network.Server server = null;
        protected static Scripting.ScriptManager scriptManager = null;


        public static Scripting.ScriptManager ScriptManager { get { return scriptManager; } }

        static void loadServerConfig()
        {
            try
            {
                if (ServerOptions.Exist())
                    serverOptions = ServerOptions.Load();
                else
                {
                    serverOptions = new ServerOptions();
                    serverOptions.Save();
                }
            }
            catch (System.Exception ex) { Log.Logger.log(Log.Logger.LOG_ERROR, ex.ToString()); serverOptions = new ServerOptions(); }
        }

        static void initFolders()
        {
            //if (!Directory.Exists("AvailableServerModules"))
            //    Directory.CreateDirectory("AvailableServerModules");
            //if (!Directory.Exists("ServerModules"))
            //    Directory.CreateDirectory("ServerModules");
            //if (!Directory.Exists("Modules"))
            //    Directory.CreateDirectory("Modules");
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");

            if (!Directory.Exists("scripts"))
                Directory.CreateDirectory("scripts");
            if (!Directory.Exists("scripts/server"))
                Directory.CreateDirectory("scripts/server");
            if (!Directory.Exists("scripts/_compiled"))
                Directory.CreateDirectory("scripts/_compiled");
        }

        static void RunServer()
        {
            const long nextInfoUpdateTime = 120 * 1000 * TimeSpan.TicksPerMillisecond;
            long nextInfoUpdates = DateTime.UtcNow.Ticks + nextInfoUpdateTime;

            long tickAverage = 0;
            long tickCount = 0;
            long tickMax = 0;

            Stopwatch watch = new Stopwatch();
            try
            {
                while (true)
                {
                    watch.Restart();

                    Scripting.Timer.Update();
                    server.Update(); //process received packets

                    watch.Stop();

                    if (nextInfoUpdates < DateTime.UtcNow.Ticks)
                    {
                        tickAverage /= tickCount;
                        Log.Logger.log(String.Format("Tick rate info: {0}ms average, {1}ms max. Allocated RAM: {2:0.0}MB", tickAverage / TimeSpan.TicksPerMillisecond, tickMax / TimeSpan.TicksPerMillisecond, (double)Process.GetCurrentProcess().PrivateMemorySize64 / 1048576d));
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

                    if (watch.ElapsedTicks < 200000)
                    {
                        Thread.Sleep(1);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Logger.log(Log.Logger.LOG_ERROR, e.Message);
                Log.Logger.log(Log.Logger.LOG_ERROR, e.Source);
                Log.Logger.log(Log.Logger.LOG_ERROR, e.StackTrace);
            }
        }

        static void Main(string[] args)
        {
            
            initFolders();
            loadServerConfig();

            TCPStatus.getTCPStatus();
            TCPStatus.getTCPStatus().addInfo("servername", serverOptions.ServerName);
            TCPStatus.getTCPStatus().addInfo("serverlanguage", "");
            TCPStatus.getTCPStatus().addInfo("maxslots", ""+serverOptions.Slots);

            try
            {
                server = new Network.Server();
                server.Start((ushort)serverOptions.Port, (ushort)serverOptions.Slots, serverOptions.password);

                AnimationControl.Init();

                scriptManager = new Scripting.ScriptManager();
                scriptManager.Init();
                scriptManager.Startup();

                new Thread(RunServer).Start();

                Log.Logger.RunLog();
            }
            catch (System.Exception ex)
            {
                Log.Logger.log(Log.Logger.LOG_ERROR, ex.Message);
                Log.Logger.log(Log.Logger.LOG_ERROR, ex.Source);
                Log.Logger.log(Log.Logger.LOG_ERROR, ex.StackTrace);
            }
            Console.Read();
        }
    }
}
