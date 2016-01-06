using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GUC.Scripts
{
    public class Startup : Scripting.IScriptInterface
    {
        public void Start()
        {
            Log.Logger.Log("erfolgreich");
            Thread.Sleep(1000);
            Log.Logger.LogWarning("erfolgreich!!");
        }
    }
}
