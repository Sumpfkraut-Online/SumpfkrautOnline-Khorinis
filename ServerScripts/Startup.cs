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
using GUC.Server.Scripts.Accounts;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripts.AI.NPC_Def;
using GUC.Server.Scripts.AI.DataTypes;
using GUC.Server.Scripts.AI.NPC_Def.Monster;
using GUC.Server.Scripts.AI.NPC_Def.Human;
namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{
		Chat chat = null;
		public static Cursor cursor;
		public Button connection;
		public void OnServerInit()
		{
            Player.EnableAllPlayerKeys(true);

            Console.WriteLine("#################### Initalise ############################");
            DefaultItems.Init();
            DefaultVobs.Init();
            DefaultWorldItem.Init();

            DayTime dt = new DayTime();
            dt.Init();
            dt.setTime(0, 12, 0);

            DamageScript.Init();

            chat = new Chat();
            chat.Init();

            cursor = Cursor.getCursor();

            Modules.Init();
            AccountSystem accSystem = new AccountSystem();
            accSystem.Init();

            AI.AISystem.Init();

            //Player.sPlayerKeyEvent += new Events.PlayerKeyEventHandler(keyEvent);

            //for (int i = 0; i < 100; i++)
            //{
            //    WayPoint wp = AI.AISystem.WayNets[@"NEWWORLD\NEWWORLD.ZEN"].getRandomWaypoint();
            //    Young_Gobbo_Green wolf = new Young_Gobbo_Green();
            //    wolf.Spawn(@"NEWWORLD\NEWWORLD.ZEN", wp.Position, null);
            //}
		}
    }
}
