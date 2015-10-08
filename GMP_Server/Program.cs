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
using System.Runtime.InteropServices;

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
            TCPStatus.getTCPStatus().addInfo("maxslots", "" + serverOptions.Slots);

            try
            {
                server = new Network.Server();
                server.Start((ushort)serverOptions.Port, (ushort)serverOptions.Slots, serverOptions.password);

                /*
                Log.Logger.log("START");

                Stopwatch watch = new Stopwatch();

                const int numtimes = 10000;
                const int numloops = 2;
                string str = new string('A', 10000);
                int val = int.MaxValue;

                long times1 = 0;
                long times2 = 0;
                long times3 = 0;
                long times4 = 0;

                using (Network.MyStream mys = new Network.MyStream())
                using (RakNet.BitStream strm = new RakNet.BitStream())
                using (MemoryStream ms = new MemoryStream())
                using (BinaryWriter bw = new BinaryWriter(ms))
                using (Network.OldStream os = new Network.OldStream())
                    for (int t = 0; t < numtimes; t++)
                    {
                        Thread.Sleep(0);
                        watch.Reset();
                        Thread.Sleep(0);
                        watch.Start();

                        mys.Reset();
                        for (int i = 0; i < numloops; i++)
                        {
                            mys.Write(byte.MaxValue);
                            //mys.Write(false);
                            //mys.Write(true);
                            //mys.Write(false);
                            //mys.Write(byte.MaxValue);
                            mys.Write(val);
                            mys.Write(str);
                        }
                        Program.server.ServerInterface.Send(mys.GetData(), mys.GetLength(), RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                        //mys.GetData(); mys.GetLength();

                        watch.Stop();
                        times1 += watch.ElapsedTicks;

                        Thread.Sleep(0);
                        watch.Reset();
                        Thread.Sleep(0);
                        watch.Start();

                        strm.Reset();
                        for (int i = 0; i < numloops; i++)
                        {
                            strm.mWrite(byte.MaxValue);
                            //strm.Write(false);
                            //strm.Write(true);
                            //strm.Write(false);
                            //strm.mWrite(byte.MaxValue);
                            strm.mWrite(val);
                            strm.mWrite(str);
                        }
                        Program.server.ServerInterface.Send(strm, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                        
                        watch.Stop();
                        times2 += watch.ElapsedTicks;

                        Thread.Sleep(0);
                        watch.Reset();
                        Thread.Sleep(0);
                        watch.Start();

                        ms.Position = 0;
                        ms.SetLength(0);
                        for (int i = 0; i < numloops; i++)
                        {
                            bw.Write(byte.MaxValue);
                            //bw.Write(false);
                            //bw.Write(true);
                            //bw.Write(false);
                            //bw.Write(byte.MaxValue);
                            bw.Write(val);
                            bw.Write(str);
                        }
                        Program.server.ServerInterface.Send(ms.ToArray(), (int)ms.Length, RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                        
                        watch.Stop();
                        times3 += watch.ElapsedTicks;

                        Thread.Sleep(0);
                        watch.Reset();
                        Thread.Sleep(0);
                        watch.Start();

                        os.Reset();
                        for (int i = 0; i < numloops; i++)
                        {
                            os.Write(byte.MaxValue);
                            //os.Write(false);
                            //os.Write(true);
                            //os.Write(false);
                            //os.Write(byte.MaxValue);
                            os.Write(val);
                            os.Write(str);
                        }
                        Program.server.ServerInterface.Send(os.GetData(), os.GetLength(), RakNet.PacketPriority.LOW_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                        watch.Stop();
                        times4 += watch.ElapsedTicks;
                    }

                Dictionary<string, long> list = new Dictionary<string, long>() { { "MyStream: ", times1 }, { "BitStream: ", times2 }, { "BinaryWriter: ", times3 }, { "OldStream: ", times4 } };

                foreach (KeyValuePair<string, long> x in list.OrderBy(x => x.Value))
                    Log.Logger.log(x.Key + x.Value);

                Log.Logger.log("END");
                */

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
