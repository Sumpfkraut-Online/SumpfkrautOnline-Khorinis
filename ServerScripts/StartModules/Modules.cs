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


namespace GUC.Server.Scripts.StartModules
{
	public class Modules
	{
		protected static List<AbstractModule> moduleList = new List<AbstractModule>();
		public static void addModule(AbstractModule module)
		{
			if(moduleList.Count != 0) {
				moduleList[moduleList.Count-1].Next = module;
				module.Previous = moduleList[moduleList.Count-1];
			}

			moduleList.Add(module);
		}

		public static void Init() {
			Player.playerConnects += new Events.PlayerEventHandler(playerConnected);
		}

		public static void playerConnected(Player pl) {
			if(moduleList.Count == 0)
				return;
			moduleList[0].start(pl);
		}
    }
}
