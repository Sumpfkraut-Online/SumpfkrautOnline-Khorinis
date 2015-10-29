using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Server.WorldObjects;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripts.Sumpfkraut.VobSystem;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Server.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Server.Scripts.Sumpfkraut;

namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{
		public void OnServerInit()
		{


            //Type bla = typeof(ScriptObject);
            ////Console.WriteLine(bla.GetProperty("_staticName"));
            ////Console.WriteLine(bla.GetProperty("_staticName").GetGetMethod());
            //Console.WriteLine(bla.GetMembers().Length);
            //Console.WriteLine(bla.GetMember("_staticName").Length);
            //Console.WriteLine(bla.GetMember("_staticName").GetValue(0));
            //System.Reflection.MemberInfo[] mInfo = bla.GetMember("_staticName");

            //for (int i = 0; i < mInfo.Length; i++)
            //{

            //    Console.WriteLine(mInfo[i]);
            //}

            //object temp;
            //int i = 0;
            //while (true)
            //{
            //    try
            //    {
            //        temp = mInfo.GetValue(i);
            //        Console.WriteLine(temp.GetType());
            //        Console.WriteLine(temp);
            //        Console.WriteLine("--------------------------");
            //    } catch
            //    {
            //        break;
            //    }
            //    i++;
            //}


            //Type type = typeof(ScriptObject); // MyClass is static class with static properties
            //foreach (var p in type.GetProperties())
            //{
            //    var v = p.GetValue(null, null); // static classes cannot be instanced, so use null...
            //    Console.WriteLine(v.GetType());
            //    Console.WriteLine(v);
            //    Console.WriteLine("--------------------------");
            //}

            //Type t = typeof(ScriptObject);
            //System.Reflection.PropertyInfo prop = t.GetProperty("_staticName");
            //object value = prop.GetValue(null, null);
            //Console.WriteLine(value);

            //Console.WriteLine(t.GetField("_staticName"));
            //Console.WriteLine(t.GetField("_staticName").GetValue(""));
            //Console.WriteLine(typeof(ScriptObject).GetField("_staticName").GetValue(""));
            //Console.WriteLine(typeof(VobHandler).GetField("_staticName").GetValue(""));

            //VobDef def = new VobDef();
            //def.Print("Hello World!");

            //VobDef.PrintStatic(typeof(VobHandler), "GOTCHA!");


            //ItemDef.MakeLogErrorStatic(typeof(ItemDef), 
            //    String.Format("Prevented attempt of adding a definition to the dictionaries:"
            //        + " The {0} {1} is already taken.", "id", 9999));

            //Func<bool, bool, bool> check = (x, y) => x && y;
            //Console.WriteLine(check.Invoke(true, false));

            //ItemDef def0 = new ItemDef(); def0.setId(0); ItemDef.Add(def0);
            //ItemDef def1 = new ItemDef(); def1.setId(1); ItemDef.Add(def1);
            //ItemDef def2 = new ItemDef(); def2.setId(2); ItemDef.Add(def2);
            //ItemDef def2_ = new ItemDef(); def2_.setId(2); ItemDef.Add(def2_);

            //ItemInstance ii = new ItemInstance("Schmarn");
            //ii.Name = "Schmarn (Ya mei!)";
            //ii.Material = Enumeration.ItemMaterial.Metal;
            //ii.Type = Enumeration.ItemType.Food_Small;
            //ii.Weight = 99;
            //ii.Description = String.Format("Lorem ipsum dolor sit amet, consectetur adipiscing elit. "
            //    +"Sed faucibus magna sem, at lobortis magna dignissim ac. " 
            //    + "Suspendisse vitae augue ultrices velit suscipit pellentesque et quis ligula. "
            //    + "Duis vitae pharetra nisl. Aenean id sollicitudin ligula, ut mattis arcu. "
            //    + "Maecenas varius euismod ornare. Etiam id leo sodales, facilisis leo ac, "
            //    + "tincidunt enim. Nulla eget efficitur sapien, eu egestas dolor. Nullam fermentum "
            //    + "tincidunt massa, non tempus magna lacinia a. In feugiat vel risus ac vestibulum.");
            //ii.Visual = "ItFo_FishSoup.3ds";
            //Item item = new Item(ii);
            //item.Amount = 9001;
            //item.Position = new GUC.Types.Vec3f(0.0f, 0.0f, 0.0f);
            ////item.CDDyn = false;
            ////item.CDStatic = true;
            ////World.NewWorld.ItemDict.Add(1, item);
            ////item.Spawn(World.NewWorld);
            //item.Drop(World.NewWorld, item.Position);

            //IGTime newTime = new IGTime();
            //newTime.day = 4; newTime.hour = 22; newTime.minute = 30;
            //Console.WriteLine(">>>> " + newTime.day + " " + newTime.hour + " " + newTime.minute);
            //World.NewWorld.ChangeTime(newTime.day, newTime.hour, newTime.minute);
            //Console.WriteLine(String.Format(">>> CHANGED TIME to day {0} {1}:{2}<<<",
            //    World.NewWorld.GetIGTime().day, 
            //    World.NewWorld.GetIGTime().hour, 
            //    World.NewWorld.GetIGTime().minute));


            //Sumpfkraut.Utilities.Threading.TestRunnable timeRunner =
            //    new Sumpfkraut.Utilities.Threading.TestRunnable(true, new TimeSpan(0, 0, 2), false);


            //Sumpfkraut.WeatherSystem.Weather weather_1 = 
            //    new Sumpfkraut.WeatherSystem.Weather(false, "MyWeather");
            //weather_1.Start();


            Logger.log(Logger.LogLevel.INFO, "######################## Initalise ########################");
            
            //DefaultItems.Init();
            

            //DayTime.Init();
            //DayTime.setTime(0, 12, 0);

#if SSM_AI
            //AI.AISystem.Init();
#endif
            //DefaultWorld.Init();


            

            //DamageScript.Init();

            

#if SSM_CHAT
            //chat = new Chat();
            //chat.Init();

            //important: register notification types for notification areas!
            /*NotificationManager.GetNotificationManager().AddNotificationArea(100, 100, 50, 8,
              new NotificationType[] { NotificationType.ChatMessage, NotificationType.ServerMessage,
                NotificationType.PlayerStatusMessage, NotificationType.MobsiMessage, NotificationType.Sound });*/
            //CommandInterpreter.GetCommandInterpreter();
            //Chat.GetChat();
            //EventNotifier.GetEventNotifier();

      

#endif

            

            //Modules.Init();



            //Modules.addModule(new Test.ListTestModule());


#if SSM_ACCOUNT
            //AccountSystem accSystem = new AccountSystem();
            //accSystem.Init();
#endif
            
#if SSM_WEB
            //Web.http_server.Init();
#endif

            //Sumpfkraut.SOKChat.SOKChat SOKChat = new Sumpfkraut.SOKChat.SOKChat();

            //Server.Sumpfkraut.Trade trade = new Server.Sumpfkraut.Trade();

            Instances.VobInstances.Init();

            Accounts.AccountSystem.Get(); //init

            DamageScript.Init();

            //Server.Network.Messages.AnimationMenuMessage.Init();
            
            Logger.log(Logger.LogLevel.INFO, "###################### End Initalise ######################");
		}
    }
}
