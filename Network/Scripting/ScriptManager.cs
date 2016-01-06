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
        static IScriptInterface scripts = null;

        public static void StartScripts(string path)
        {

            if (asm == null)
            {
                try
                {
                    asm = Assembly.LoadFile(Path.GetFullPath(path));
                    asm.CreateInstance("GUC.Scripts.WriteFile");

                    foreach (Type t in asm.GetTypes())
                        Log.Logger.Log(t.AssemblyQualifiedName);

                    scripts = (IScriptInterface)asm.CreateInstance("GUC.Scripts.Startup");
                    scripts.Start();
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
                    Log.Logger.LogError(errorMessage);
                }
                catch (Exception e)
                {
                    Log.Logger.LogError(e);
                }
            }
        }
    }
}
