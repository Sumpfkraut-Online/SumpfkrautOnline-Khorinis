using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Server.WorldObjects;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;

namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{
		public void OnServerInit()
		{
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

            Player.OnEnterWorld += EnterWorld;

            InitItemInstances();

            Accounts.AccountSystem.Get(); //init

            //Server.Network.Messages.AnimationMenuMessage.Init();
            
            Logger.log(Logger.LogLevel.INFO, "###################### End Initalise ######################");
		}

        void EnterWorld(NPC pl)
        {
            (new Item("ITFO_APPLE")).Spawn(World.NewWorld);

            for (int i = 0; i < ItemInstance.InstanceList.Count; i++)
                pl.AddItem(ItemInstance.InstanceList[i], i+1);
        }

        void InitItemInstances()
        {
            var q = from t in System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace.StartsWith("GUC.Server.Scripts.Items") && !t.IsAbstract && t.IsSubclassOf(typeof(ItemInstance))
                    select t;
            foreach (Type t in q.ToList())
            {
                Activator.CreateInstance(t);
            }
        }
    }


}
