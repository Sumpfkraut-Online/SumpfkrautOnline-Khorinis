using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace GUC.Scripting
{
    static class ScriptManager
    {
        static Assembly asm = null;

        public static void StartScripts(string path)
        {
            if (asm != null)
                return;

            try
            {
                asm = Assembly.LoadFile(Path.GetFullPath(path));
                asm.CreateInstance("GUC.Scripts.Init");
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
