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
using GUC.Server.Scripts.Sumpfkraut.Database;

namespace GUC.Server.Scripts
{
	public class Startup : IServerStartup
	{

        //public delegate void MyEventHandler (Sumpfkraut.Utilities.Threading.Runnable runnable);
        //public MyEventHandler MyEvent;

		public void OnServerInit()
		{
            Log.Logger.log("######################## Initalise ########################");

            Animations.AniCtrl.InitAnimations();

            Accounts.AccountSystem.Init();

            Instances.VobInstances.Init();

            DamageScript.Init();

            CmdHandler.Init();

            new World("newworld", "newworld\\newworld.zen", null).Create();
            
            Logger.log(Logger.LogLevel.INFO, "###################### End Initalise ######################");
		}

        private void AgentBlack_ReceivedResults(object sender, Sumpfkraut.Database.DBAgent.ReceivedResultsEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
