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
        static IGUCScripts scripts;

        public static void StartScripts(string path)
        {
            if (asm == null)
            {
                asm = Assembly.LoadFile(Path.GetFullPath(path));
                scripts = (IGUCScripts)asm.CreateInstance("GUC.Scripts.Startup");
                scripts.Startup();
            }
        }
    }
}
