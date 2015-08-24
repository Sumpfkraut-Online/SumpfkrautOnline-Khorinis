using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;

using GUC.Server.Scripts.StartModules;
#if SSM_ACCOUNT
using GUC.Server.Scripts.Accounts;
#endif

using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripts.AI.NPC_Def;
using GUC.Server.Scripts.AI.DataTypes;
using GUC.Server.Scripts.AI.NPC_Def.Monster;
using GUC.Server.Scripts.AI.NPC_Def.Human;
using GUC.Server.Scripts.Items;
using GUC.Server.Scripts.Communication;
using GUC.Server.Scripts.Utils;

using GUC.Server.Scripts.Sumpfkraut.WeatherSystem;

namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{
		//public static Chat chat = null;
		public static Cursor cursor;
		public Button connection;
		public void OnServerInit()
		{

            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            //List<int> myList = new List<int>();
            //for (int i = 0; i <= 10; i++)
            //{
            //    myList.Add(i);
            //}
            //myList.Insert(5, 999);
            //myList.RemoveRange(0, 4);
            //for (int i = 0; i < myList.Count; i++)
            //{
            //    Console.WriteLine(myList[i]);
            //}

            //List<Object> objList = new List<Object>();
            //Object obj1 = new Object();
            //Object obj2 = obj1;
            //objList.Add(obj1);
            //Object obj3 = objList[0];
            //Console.WriteLine((obj1 == obj2) + " " + (obj1 == obj3) + " " + (obj2 == obj3));

            Weather w = new Weather(false);

            w.FillUpQueue();
            //Console.WriteLine(">>> " + w.weatherStateQueue.Count);
            for (int i = 0; i < w.weatherStateQueue.Count; i++)
            {
                Console.WriteLine(i + ": " + w.weatherStateQueue[i].startTime + " ## " + 
                    w.weatherStateQueue[i].endTime);
            }

            w.InsertWeatherState(new WeatherState(World.WeatherType.Snow, 
                w.weatherStateQueue[5].startTime, 
                w.weatherStateQueue[7].endTime));
            //Console.WriteLine(">>> " + w.weatherStateQueue.Count);
            for (int i = 0; i < w.weatherStateQueue.Count; i++)
            {
                Console.WriteLine(i + ": " + w.weatherStateQueue[i].startTime + " ## " + 
                    w.weatherStateQueue[i].endTime);
            }

            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");




            Logger.log(Logger.LogLevel.INFO, "######################## Initalise ########################");
            cursor = Cursor.getCursor();
            RandomManager.GetRandom();

            Test.Text3DTest.Init();

            
            
            ItemInit.Init();
            DefaultItems.Init();
            

            DayTime.Init();
            DayTime.setTime(0, 12, 0);


#if SSM_AI
            AI.AISystem.Init();
#endif
            DefaultWorld.Init();



            

            DamageScript.Init();



#if SSM_CHAT
            //chat = new Chat();
            //chat.Init();

            //important: register notification types for notification areas!
            NotificationManager.GetNotificationManager().AddNotificationArea(100, 100, 50, 8,
              new NotificationType[] { NotificationType.ChatMessage, NotificationType.ServerMessage,
                NotificationType.PlayerStatusMessage, NotificationType.MobsiMessage, NotificationType.Sound });
            CommandInterpreter.GetCommandInterpreter();
            Chat.GetChat();
            EventNotifier.GetEventNotifier();
      

#endif

            

            Modules.Init();



            //Modules.addModule(new Test.ListTestModule());


#if SSM_ACCOUNT
            AccountSystem accSystem = new AccountSystem();
            accSystem.Init();
#endif
            
#if SSM_WEB
            Web.http_server.Init();
#endif


            
            Logger.log(Logger.LogLevel.INFO, "###################### End Initalise ######################");
		}
    }
}
