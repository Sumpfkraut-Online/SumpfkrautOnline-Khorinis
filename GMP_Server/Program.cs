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

        static void initClientModule()
        {
            try
            {
                foreach (Module module in serverOptions.Modules)
                    module.Hash = FileFunc.Datei2MD5(@"Modules/" + module.name);

            }
            catch (System.Exception ex)
            {
                Log.Logger.log(Log.Logger.LOG_ERROR, ex.ToString());
            }
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


        static void Main(string[] args)
        {
            
            initFolders();
            loadServerConfig();
            initClientModule();

            TCPStatus.getTCPStatus();
            TCPStatus.getTCPStatus().addInfo("servername", serverOptions.ServerName);
            TCPStatus.getTCPStatus().addInfo("serverlanguage", "");
            TCPStatus.getTCPStatus().addInfo("maxslots", ""+serverOptions.Slots);

            try
            {
                server = new Network.Server();
                server.Start((ushort)serverOptions.Port, (ushort)serverOptions.Slots, serverOptions.password);
                
                scriptManager = new Scripting.ScriptManager();
                scriptManager.Init();
                scriptManager.Startup();

                const long nextInfoUpdateTime = 60 * 1000 * TimeSpan.TicksPerMillisecond;
                long nextInfoUpdates = DateTime.Now.Ticks + nextInfoUpdateTime;

                long tickAverage = 0;
                long tickCount = 0;
                long tickMax = 0;

                long ticks, elapsed;
                //const int serverTickrate = 30;
                const long updateTime = 20 * TimeSpan.TicksPerMillisecond;
                while (true)
                {
                    ticks = DateTime.Now.Ticks;

                    scriptManager.Update();
                    server.Update(); //process received packets

                    if (nextInfoUpdates < DateTime.Now.Ticks)
                    {
                        tickAverage /= tickCount;
                        Log.Logger.log(String.Format("Server tick rate info: {0}ms average, {1}ms max.", tickAverage / TimeSpan.TicksPerMillisecond, tickMax / TimeSpan.TicksPerMillisecond));
                        nextInfoUpdates = DateTime.Now.Ticks + nextInfoUpdateTime;
                        tickMax = 0;
                        tickAverage = 0;
                        tickCount = 0;
                    }

                    tickCount++;
                    elapsed = DateTime.Now.Ticks - ticks;
                    tickAverage += elapsed;
                    if (elapsed > tickMax)
                    {
                        tickMax = elapsed;
                    }

                    if (elapsed < updateTime)
                    {
                        Thread.Sleep((int)(elapsed / TimeSpan.TicksPerMillisecond));
                    }
                }
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
