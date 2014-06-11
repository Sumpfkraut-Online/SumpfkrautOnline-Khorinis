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
namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{
		public static Chat chat = null;
		public static Cursor cursor;
		public Button connection;
		public void OnServerInit()
		{
            Logger.log(Logger.LogLevel.INFO, "######################## Initalise ########################");
            cursor = Cursor.getCursor();



            
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
            chat = new Chat();
            chat.Init();
#endif

            

            Modules.Init();
            

            



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
