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
    /// <summary>
    /// The scriptmanager is internally used to load the c# Scripts.
    /// And it manages the Timers. 
    /// The class can be accessed by ScriptManager.Self
    /// </summary>
    public class ScriptManager
    {
        private Assembly m_Assembly;
        private bool m_Startuped = false;
        private bool m_Initalised = false;

        internal List<Timer> m_TimerList = new List<Timer>();

        private static ScriptManager s_Self;

        public static ScriptManager Self { get { return s_Self; } }

        internal ScriptManager()
        {
            s_Self = this;
        }

        internal void Init()
        {
            if (m_Initalised)
                return;

            if (Program.serverOptions.useScriptedFile)
            {
                Load();
            }
            else
            {
                Compile(Program.serverOptions.generateScriptedFile);
            }
        }

        public bool Startuped { get { return m_Startuped; } }

        public int Slots { get { return Program.serverOptions.Slots; } }
        

        private void Load()
        {
            
            try
            {
                m_Assembly = System.Reflection.Assembly.LoadFile(Path.GetFullPath("scripts/_compiled/ServerScripts.dll"));
            }
            catch (Exception ex)
            {
                Log.Logger.log(Log.Logger.LogLevel.ERROR, "ServerScripts.dll could not be found! It has to be in the scripts/_compiled/ folder <br>"+ex.ToString());
            }
        }

        private void Compile(bool file)
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
            GetFileList(fileList, "scripts/server");
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


            m_Assembly = CompilerResults.CompiledAssembly;
        }


        private void GetFileList(List<String> filelist, String dir)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(dir))
                {
                    GetFileList(filelist, d);
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
            if (m_Startuped)
                return;

            
            

            try
            {
                Listener.IServerStartup listener = (Listener.IServerStartup)m_Assembly.CreateInstance("GUC.Server.Scripts.Startup");
                try
                {
                    listener.OnServerInit();
                }
                catch (Exception ex2)
                {
                    Log.Logger.log(Log.Logger.LOG_ERROR, ex2.Source + "<br>" + ex2.Message + "<br>" + ex2.StackTrace);

                }
            }
            catch (Exception ex)
            {
                Log.Logger.log(Log.Logger.LOG_ERROR, "GUC.Server.Scripts.Startup-Class could not be found!" + "<br>" + ex.Source + "<br>" + ex.Message + "<br>" + ex.StackTrace);
            }
            Log.Logger.log(Log.Logger.LOG_INFO, "GUC Server - Initalisation Complete GUC-Version: " + GUC.Options.Constants.VERSION);
            m_Startuped = true;
        }

        internal void Update()
        {
            long time = DateTime.Now.Ticks;
            Timer[] arr = m_TimerList.ToArray();
            foreach (Timer timer in arr)
            {
                timer.iUpdate(time);
            }
        }



    }
}
