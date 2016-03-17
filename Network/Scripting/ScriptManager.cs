using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using GUC.Network;

namespace GUC.Scripting
{
    public partial interface ScriptInterface
    {
        bool OnClientConnection(GameClient client);
        WorldObjects.Item CreateInvItem(PacketReader stream);
    }

    static class ScriptManager
    {
        static Assembly asm = null;
        public static ScriptInterface Interface { get; private set; }

        public static void StartScripts(string path)
        {
            if (asm != null)
                return;

            try
            {
                asm = Assembly.LoadFile(Path.GetFullPath(path));
                Interface = (ScriptInterface)asm.CreateInstance("GUC.Scripts.GUCScripts");
            }
            catch (ReflectionTypeLoadException ex)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);
                    FileNotFoundException exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null)
                    {
                        if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }
                    sb.AppendLine();
                }
                string errorMessage = sb.ToString();
                //Display or log the error based on your application.
                Log.Logger.LogError(Environment.CurrentDirectory + "\n" + errorMessage);
            }
            catch (Exception e)
            {
                Log.Logger.LogError(Environment.CurrentDirectory + "\n" + e);
            }
        }
    }
}
