using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using GMP_Server.Net;
using GMP_Server.Modules;
using GMP_Server.Net.Message;
using GMP_Server.DB;
using Network.Savings;
using Network.Types;

namespace GMP_Server
{
    public class Program
    {
        public static Dictionary<Int32, Player> playerDict = new Dictionary<Int32, Player>();
        public static List<Player> playerList = new List<Player>();
        public static List<NPC> npcList = new List<NPC>();
        public static List<Player> playList = new List<Player>();//eigentliche Spielerliste
        public static World World = new World();

        public static int idCount;
        public static ServerConfig config = null;
        public static Server server;

        public static double time;
        public static long lastTimeTick;
        public static GothicTime gTime;
        public static long lastTimeSendet;

        public static SQLLite sqlite;
        public static WorldSave worldsave;
        public static Scripting.ScriptManager scriptManager;

        static void Main(string[] args)
        {
            sqlite = new SQLLite();
            sqlite.Open();

            worldsave = new WorldSave();
            worldsave.LoadWorld();

            
            
            try
            {
                if (ServerConfig.Exist())
                    config = ServerConfig.Load();
                else
                {
                    config = new ServerConfig();
                    config.Save();
                }
            }
            catch (System.Exception ex) { Console.WriteLine(ex.ToString()); config = new ServerConfig(); }

            try
            {
                foreach (Module module in config.Modules)
                    module.Hash = Datei2MD5(@"Modules/" + module.name);

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            gTime = new GothicTime();
            gTime.time = GothicTime.GetTime(config.day, config.hour, config.minute);
            time = gTime.time;

            Console.WriteLine(gTime.getHourInDay()+":"+gTime.GetMinuteInHour());
            
            LoadSpawnXML();

            


            try
            {
                server = new Server();
                server.Start((ushort)config.Port, (ushort)config.Slots, config.password);
                ModuleLoader.loadAllModules();

                scriptManager = new Scripting.ScriptManager();
                scriptManager.init();
                scriptManager.Startup();


                lastTimeTick = DateTime.Now.Ticks;
                while (true)
                {

                    double revTicks = DateTime.Now.Ticks - lastTimeTick;
                    double sec = revTicks / (10000.0 * 1000.0);//In Sekunde
                    time += ((double)sec) * 0.25;
                    lastTimeTick = DateTime.Now.Ticks;
                    gTime.time = (long)time;

                    if (lastTimeSendet + 1000*10000 < DateTime.Now.Ticks)
                    {
                        new TimeMessage().Write(server.receiveBitStream, server);
                        lastTimeSendet = DateTime.Now.Ticks;
                    }
                    
                    ModuleLoader.updateAllModules();
                    scriptManager.update();
                    server.Update();
                    

                    //updateControllers();
                    updateControllersNear();
                    RemoveDeadNPC();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.Read();
        }


        public static SpawnList spawnConfig = null;
        public static void LoadSpawnXML()
        {
            try
            {
                spawnConfig = SpawnList.Load();
            }
            catch (System.Exception ex) {
                spawnConfig = new SpawnList();
                if (ex.GetType() == typeof(System.IO.FileNotFoundException))
                {
                    spawnConfig.Save();
                }
                else
                    Console.WriteLine(ex.ToString()); 
                
            }


            if (spawnConfig.StartSpawnFunction == null || spawnConfig.StartSpawnFunction.Trim().Length == 0)
                return;

            foreach (SpawnFunction spawnfunc in spawnConfig.SpawnFunctions)
            {
                if (spawnfunc.name.Trim().ToUpper() == spawnConfig.StartSpawnFunction.Trim().ToUpper())
                {
                    foreach (SpawnContent spawn in spawnfunc.Spawns)
                    {
                        foreach (Object spawnPoint in spawn.Spawns)
                        {
                            if (spawnPoint.GetType() != typeof(Vec3))
                                continue;
                            Vec3 position = (Vec3)spawnPoint;

                            NPC npc = new NPC();
                            Player player = new Player("NPC");

                            Program.idCount += 1;
                            player.id = Program.idCount;
                            player.actualMap = Player.getMap(spawn.world);
                            player.instance = spawn.instance.Trim().ToUpper();
                            player.isSpawned = true;
                            player.pos = new float[] { position.x, position.y, position.z };
                            player.isNPC = true;
                            player.NPC = npc;


                            npc.isStatic = true;
                            npc.npcPlayer = player;
                            npc.controller = null;
                            npc.spawn = new float[] { position.x, position.y, position.z };

                            Program.npcList.Add(npc);
                            Program.playerDict.Add(player.id, player);
                            Program.playerList.Add(player);
                            Program.playerList.Sort(new Network.Player.PlayerComparer());
                        }
                    }
                    break;
                }
            }


            
        }











        #region ControllerUpdates
        static long lastUpdateController;
        static long lastUpdateControllerRemove;
        public static void updateControllers()
        {
            if (lastUpdateController + 10000 * 100 > DateTime.Now.Ticks)
                return;


            
            foreach (NPC npc in npcList)
            {
                if (lastUpdateControllerRemove + 10000 * 1000 * 10 < DateTime.Now.Ticks && npc.controller != null && server.server.GetAveragePing(npc.controller.guid) > 300)
                {
                    new NPCControllerMessage().Write(server.receiveBitStream, server, npc, false);
                    npc.controller = null;
                    lastUpdateControllerRemove = DateTime.Now.Ticks;
                }
                if (npc.controller != null)
                    continue;


                if (npc.npcPlayer.lastHP != 0 && npc.isSummond && npc.npcPlayer.instance.StartsWith("SUMMONED_"))
                {
                    npc.npcPlayer.lastHP = 0;
                    new AllPlayerSynchMessage().Write(server.receiveBitStream, server, -1, npc.npcPlayer.id, (byte)AllPlayerSynchMessageTypes.HP, 0);
                }
                //Neuen Controller finden...
                Player controller = null;
                int ping = -1;

                foreach (Player sPl in Program.playList)
                {
                    int averageping = server.server.GetAveragePing(sPl.guid);
                    if (!Player.isSameMap(sPl.actualMap, npc.npcPlayer.actualMap))
                    {
                        continue;
                    }
                    if (controller == null || (ping > averageping && averageping != -1))
                    {
                        controller = sPl;
                        ping = averageping;
                    }
                }
                npc.controller = controller;
                new NPCControllerMessage().Write(server.receiveBitStream, server, npc, true);
            }

            lastUpdateController = DateTime.Now.Ticks;
        }

        static int getControlDistance = 4000;
        static int removeControlDistance = 5000;
        public static int updateControlDistance = 6000;
        public static void updateControllersNear()
        {
            if (lastUpdateController + 10000 * 100 > DateTime.Now.Ticks)
                return;

            foreach (NPC npc in Program.npcList)
            {
                //Überprüfe ob Controller noch nah genug heran ist
                if (npc.controller != null)
                {
                    Vec3f posNPC = new Vec3f(npc.npcPlayer.pos);
                    Vec3f posPL = new Vec3f(npc.controller.pos);

                    //if... Distanzabfrage
                    if (posNPC.getDistance(posPL) > Program.removeControlDistance)
                    {
                        //Wenn Beschworenes Wesen zu weit weg ist, töte es!
                        if (npc.npcPlayer.lastHP != 0 && npc.isSummond && npc.npcPlayer.instance.StartsWith("SUMMONED_"))
                        {
                            npc.npcPlayer.lastHP = 0;
                            new AllPlayerSynchMessage().Write(server.receiveBitStream, server, -1, npc.npcPlayer.id, (byte)AllPlayerSynchMessageTypes.HP, 0);
                        }

                        //Controller entfernen!
                        //Console.WriteLine("Deleted Controller: " + npc.controller.name + " from npc: " + npc.npcPlayer.instance + " width distance: " + posNPC.getDistance(posPL));
                        new NPCControllerMessage().Write(server.receiveBitStream, server, npc, false);
                        npc.controller = null;
                    }
                    
                }
                //Suche neuen Controller
                if (npc.controller == null)
                {
                    Vec3f posNPC = new Vec3f(npc.npcPlayer.pos);
                    //Alle, sich in der nähe befindenen Controller raussuchen!
                    foreach (Player pl in Program.playList)
                    {
                        Vec3f posPL = new Vec3f(pl.pos);
                        if (!Player.isSameMap(pl.actualMap, npc.npcPlayer.actualMap))
                            continue;

                        //if.... Wenn nahe dann break und nutze pl als neuen Controller
                        if (posNPC.getDistance(posPL) < Program.getControlDistance)
                        {
                            //Controller hinzufügen
                            
                            npc.controller = pl;
                            new NPCControllerMessage().Write(server.receiveBitStream, server, npc, true);
                            Console.WriteLine("Added Controller: " + npc.controller.name + " from npc: " + npc.npcPlayer.instance + " width distance: " + posNPC.getDistance(posPL));
                            break;
                        }
                    }
                }
            }
            lastUpdateController = DateTime.Now.Ticks;
        }

        #endregion
        public static String Datei2MD5(string Dateipfad)
        {
            //Datei einlesen
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(Dateipfad);
            // MD5-Hash aus dem Byte-Array berechnen
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();

            //in string wandeln
            string Berechnet = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            return Berechnet;
        }
        private static bool Datei2MD5(string Dateipfad, string Checksumme)
        {
            //Datei einlesen
            System.IO.FileStream FileCheck = System.IO.File.OpenRead(Dateipfad);
            // MD5-Hash aus dem Byte-Array berechnen
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] md5Hash = md5.ComputeHash(FileCheck);
            FileCheck.Close();

            //in string wandeln
            string Berechnet = BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            // Vergleichen
            if (Berechnet == Checksumme.ToLower())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #region NPC_REMOVE_AFTER_DEAD
        static long LastRemoveDeadNPC;
        public static void RemoveDeadNPC()
        {
            if (Program.config.RemoveNPCTime < 0)
                return;
            if (LastRemoveDeadNPC + 10000 * 1000 > DateTime.Now.Ticks)
                return;
            NPC[] list = Program.npcList.ToArray();
            foreach (NPC npc in list)
            {
                if (npc.npcPlayer.lastHP == 0)
                {
                    npc.deadTimer += 1;
                }

                if (npc.deadTimer >= Program.config.RemoveNPCTime)
                {
                    CommandoMessage.RemoveNPC(npc);
                }
                
                if (npc.npcPlayer.lastHP != 0)
                {
                    npc.deadTimer = 0;
                }

            }

            LastRemoveDeadNPC = DateTime.Now.Ticks;
        }
        #endregion
    }
}
