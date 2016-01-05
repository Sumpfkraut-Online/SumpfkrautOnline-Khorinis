using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using System.Threading;
using GUC.Scripting;
using GUC.Log;

namespace GUC.Client
{
    class Program
    {
        public readonly static Process Process = Process.ThisProcess();

        public static Int32 InjectedMain(String message)
        {
            try
            {
                ScriptManager.StartScripts("ClientScripts.dll");

                while (true)
                {
                    Thread.Sleep(10000);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Source + " " + ex.Message);
            }
            return 0;
        }
    }
}
