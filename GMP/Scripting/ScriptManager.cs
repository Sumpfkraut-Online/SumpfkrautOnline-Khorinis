using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using GUC.timer;
using System.IO;

namespace GUC.Scripting
{
    public class ScriptManager
    {
        private Assembly m_Assembly;
        private bool m_Startuped = false;
        private bool m_Initalised = false;

        private static ScriptManager s_Self;

        public static ScriptManager Self { get { return s_Self; } }


        protected bool ScriptFound = false;

        protected static WinApi.Process s_Process = null;
        public static WinApi.Process getProcess() {
            if (s_Process == null)
                s_Process = WinApi.Process.ThisProcess();
            return s_Process;
        }

        internal ScriptManager()
        {
            s_Self = this;
        }

        internal void Init()
        {
            if (m_Initalised)
                return;

            LoadAll();
        }

        public bool Startuped { get { return m_Startuped; } }

        private void LoadAll()
        {
            if (File.Exists(States.StartupState.getDownloadPath() + "ClientScripts.dll"))
            {
                Load(States.StartupState.getDownloadPath() + "ClientScripts.dll");
            }
        }

        private void Load(String file)
        {

            try
            {
                m_Assembly = System.Reflection.Assembly.LoadFile(Path.GetFullPath(file));
                ScriptFound = true;
            }
            catch (Exception ex)
            {
                
            }
        }


        internal void Startup()
        {
            if (m_Startuped)
                return;

            if (!ScriptFound)
                return;

            try
            {
                Listener.IClientStartup listener = (Listener.IClientStartup)m_Assembly.CreateInstance("GUC.Client.Scripts.Startup");
                listener.OnClientInit();
            }
            catch (Exception ex)
            {
                
            }
            m_Startuped = true;
        }


    }
}
