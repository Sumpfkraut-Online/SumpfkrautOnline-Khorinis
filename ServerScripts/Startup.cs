using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Log;
using GUC.Scripting;

namespace GUC.Scripts
{
    public class Init : ScriptInterface
    {
		public void OnServerInit()
		{
            Logger.Log("######################## Initalise ########################");
            
            Logger.Log("###################### End Initalise ######################");
		}
    }
}
