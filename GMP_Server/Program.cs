using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using GUC.Options;
using GUC.Types;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using GUC.Server.Network.Messages.NpcCommands;

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
            if (!Directory.Exists("AvailableServerModules"))
                Directory.CreateDirectory("AvailableServerModules");
            if (!Directory.Exists("ServerModules"))
                Directory.CreateDirectory("ServerModules");
            if (!Directory.Exists("Modules"))
                Directory.CreateDirectory("Modules");
            if (!Directory.Exists("Config"))
                Directory.CreateDirectory("Config");

            if (!Directory.Exists("scripts"))
                Directory.CreateDirectory("scripts");
            if (!Directory.Exists("scripts/server"))
                Directory.CreateDirectory("scripts/server");
        }


        static void Main(string[] args)
        {
            initFolders();
            loadServerConfig();
            initClientModule();

            try
            {
                server = new Network.Server();
                server.Start((ushort)serverOptions.Port, (ushort)serverOptions.Slots, serverOptions.password);
                //ModuleLoader.loadAllModules();

                scriptManager = new Scripting.ScriptManager();
                scriptManager.init();
                scriptManager.Startup();

                while (true)
                {
                    long ticks = DateTime.Now.Ticks;
                    Player.sUpdateNPCList(ticks);

                    //ModuleLoader.updateAllModules();
                    scriptManager.update();
                    server.Update();
                    updateNPCController(ticks);
                    
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.Source);
                Console.WriteLine(ex.StackTrace);
            }
            Console.Read();
        }


        static int index = 0;
        static long lastNPC = 0;
        static void updateNPCController(long now)
        {
            if (sWorld.NpcList.Count == 0)
                return;
            if (sWorld.PlayerList.Count == 0)
                return;
                    
            int x = (sWorld.NpcList.Count / 250 == 0) ? 1 : (sWorld.NpcList.Count / 250);



            if (lastNPC + 10000 * (250 / (sWorld.NpcList.Count / x)) > now)
                return;

            if (index >= sWorld.NpcList.Count)
                index = 0;
            int c = 250;
            int endIndex = (index + c > sWorld.NpcList.Count) ? sWorld.NpcList.Count : index + c;

            

            for (; index < endIndex; index++)
            {
                
                //Checking NPC-Ranges!
                NPC npc = sWorld.NpcList[index];

                if (!npc.IsSpawned)
                    continue;
                if (npc.NpcController != null)
                {
                    //Check if npc is in range of controller:
                    if (!npc.Map.Equals(npc.NpcController.Map) || npc.Position.getDistance(npc.NpcController.Position) > 4500)
                    {
                        //Send Controller-Message and set NpcController to null
                        NPCControllerMessage.Write(npc, npc.NpcController, false);

                        npc.NpcController.NPCControlledList.Remove(npc);
                        npc.NpcController = null;

                        
                    }
                }

                if (npc.NpcController == null)//Search new Player to control the npc!
                {
                    Scripting.Objects.Character.Player player = npc.ScriptingNPC.getNearestPlayers(4000.0f);
                    if (player != null)
                    {
                        //Send new controller message!

                        npc.NpcController = (Player)player.proto;
                        npc.NpcController.NPCControlledList.Add(npc);
                        
                        NPCControllerMessage.Write(npc, npc.NpcController, true);

                    }
                }
            }

            lastNPC = now;

        }
    }
}
