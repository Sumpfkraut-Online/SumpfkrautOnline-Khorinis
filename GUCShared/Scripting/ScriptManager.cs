using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Animations;
using GUC.Models;
using GUC.WorldObjects.Definitions;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects.VobGuiding;
using GUC.Types;

namespace GUC.Scripting
{
    public partial interface ScriptInterface
    {
        GameClient CreateClient();
        Overlay CreateOverlay();
        AniJob CreateAniJob();
        Animation CreateAnimation();
        ModelInstance CreateModelInstance();
        GUCBaseVobInst CreateVob(byte type);
        GUCBaseVobDef CreateInstance(byte type);
        World CreateWorld();
        GuideCmd CreateGuideCommand(byte type);
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
            catch (ReflectionTypeLoadException refException)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Exception exSub in refException.LoaderExceptions)
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
            catch(System.IO.FileNotFoundException e)
            {
                Log.Logger.LogError("Server coudln't find file: " + Path.GetFileName(path));
            }
            catch (Exception e)
            {
                Log.Logger.LogError(Environment.CurrentDirectory + "\n" + e);
            }
        }
    }
}
