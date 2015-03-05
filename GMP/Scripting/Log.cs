using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace GUC.Scripting
{
    public class Log
    {
        protected static Log m_Log = null;
        public static Log Get()
        {
            if (m_Log == null)
            {
                m_Log = new Log();
            }
            return m_Log;
        }

        protected Log()
        {

        }


        public void log(String message)
        {
            log(2, message, "Log.cs", 0);
        }

        public void log(byte LogLevel, String message)
        {
            log(LogLevel, message, "Log.cs", 0);
        }

        public void log(byte LogLevel, String message, String filename, int line)
        {
            zERROR.GetZErr(Process.ThisProcess()).Report(LogLevel, 'M', message, 0, filename, line);
        }
    }
}
