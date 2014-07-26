using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using GUC.WorldObjects.Character;

namespace GUC.Server.Scripting
{
    public class ScriptManager
    {
        private Assembly assembly;
        private bool startuped = false;
        private bool initalised = false;

        internal List<Timer> TimerList = new List<Timer>();

        private static ScriptManager _self;

        public static ScriptManager Self { get { return _self; } }

        internal ScriptManager()
        {
            _self = this;
        }

        internal void init()
        {
            if (initalised)
                return;

            if (Program.serverOptions.useScriptedFile)
            {
                load();
            }
            else
            {
                compile(Program.serverOptions.generateScriptedFile);
            }
        }

        private void load()
        {
            try
            {
                assembly = System.Reflection.Assembly.LoadFile(Path.GetFullPath("scripts/_compiled/ServerScripts.dll"));
            }
            catch (Exception ex)
            {
                Log.Logger.log(Log.Logger.LogLevel.ERROR, "ServerScripts.dll could not be found! It has to be in the scripts/_compiled/ folder <br>"+ex.ToString());
            }
        }

        private void compile(bool file)
        {
            System.CodeDom.Compiler.CodeDomProvider CodeDomProvider = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("CSharp");

            System.CodeDom.Compiler.CompilerParameters CompilerParameters = new System.CodeDom.Compiler.CompilerParameters();
            CompilerParameters.ReferencedAssemblies.Add("System.dll");
            CompilerParameters.ReferencedAssemblies.Add("System.Data.dll");
            CompilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
            CompilerParameters.ReferencedAssemblies.Add("System.Linq.dll");
            CompilerParameters.ReferencedAssemblies.Add("RakNetSwig.dll");
            CompilerParameters.ReferencedAssemblies.Add("GMP_Server.exe");


            foreach (String str in Program.serverOptions.AdditionalLibs)
            {
                CompilerParameters.ReferencedAssemblies.Add(str);
            }

            
            
            CompilerParameters.CompilerOptions = "/t:library";///debug:full
            for(int i = 0; i < Program.serverOptions.AdditionalSymbols.Count; i++)
            {
                String str = Program.serverOptions.AdditionalSymbols[i];
                if (i == 0)
                    CompilerParameters.CompilerOptions += "/define: ";
                CompilerParameters.CompilerOptions += str;
                if(i != Program.serverOptions.AdditionalLibs.Count - 1)
                    CompilerParameters.CompilerOptions += ";";
            }                                                

            CompilerParameters.IncludeDebugInformation = true;
            
            if (!file)
            {
                CompilerParameters.GenerateInMemory = true;
            }
            else
            {
                CompilerParameters.GenerateInMemory = false;
                CompilerParameters.OutputAssembly = "scripts/_compiled/ServerScripts.dll";
            }

            List<String> fileList = new List<string>();
            getFileList(fileList, "scripts/server");
            //getFileList(fileList, "scripts/both");

            System.CodeDom.Compiler.CompilerResults CompilerResults = CodeDomProvider.CompileAssemblyFromFile(CompilerParameters, fileList.ToArray());
            if (CompilerResults.Errors.Count > 0)
            {

                foreach (CompilerError col in CompilerResults.Errors)
                {

                    Log.Logger.log(Log.Logger.LOG_ERROR, col.FileName + ":" + col.Line + " \t" + col.ErrorText);
                }
                return;
            }


            assembly = CompilerResults.CompiledAssembly;
        }


        private void getFileList(List<String> filelist, String dir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    getFileList(filelist, d);
                }
                foreach (string f in Directory.GetFiles(dir, "*.cs"))
                {
                    if (f.ToLower().Trim().EndsWith(".cs"))
                        filelist.Add(f);
                }
            }
            catch (Exception excpt)
            {
                Log.Logger.log(Log.Logger.LOG_ERROR, excpt.ToString());
            }
        }


        internal void Startup()
        {
            if (startuped)
                return;

            startuped = true;


            try
            {
                Listener.IServerStartup listener = (Listener.IServerStartup)assembly.CreateInstance("GUC.Server.Scripts.Startup");
                listener.OnServerInit();
            }
            catch (Exception ex)
            {
                Log.Logger.log(Log.Logger.LOG_ERROR, "GUC.Server.Scripts.Startup-Class could not be found!" + "<br>" + ex.Source + "<br>" + ex.Message + "<br>" + ex.StackTrace);
            }
        }

        internal void update()
        {
            long time = DateTime.Now.Ticks;
            Timer[] arr = TimerList.ToArray();
            foreach (Timer timer in arr)
            {
                timer.iUpdate(time);
            }
        }



    }
}
