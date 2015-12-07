using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace GUC.Server.Log
{
    public class Logger
    {
        public static int LOG_INFO = 0;
        public static int LOG_WARNING = 1;
        public static int LOG_ERROR = 2;

        public enum LogLevel{
            INFO = 0,
            WARNING = 1,
            ERROR = 2
        }



        static StringBuilder sb = null;
        static StreamWriter myWriter = null;
        static Logger()
        {
            sb = new StringBuilder();
            if (!File.Exists("serverlog.html"))
              File.Create("serverlog.html").Close();
                //File.Delete("serverlog.html");

            myWriter = new StreamWriter(File.Open(@"serverlog.html", FileMode.Append));
            
        }
       
        ~Logger()
        {
          if (myWriter != null)
          {
            myWriter.Flush();
            myWriter.Close();
            myWriter.Dispose();
          }
        }

        public static void log(LogLevel ll, object message)
        {
            log((int)ll, message);
        }

        public static void logWarning(object message)
        {
          log((int)LogLevel.WARNING, message);
        }

        public static void logError(object message)
        {
          log((int)LogLevel.ERROR, message);
        }
        
        public static void log(object message)
        {
          log((int)LogLevel.INFO, message);
        }

        public static void log(int level, object message)
        {
            try
            {
                sb.Clear();
                sb.Append("<span class=\"level_").Append(level);
                sb.Append("\">").Append(message).Append("</span><br>");
                myWriter.WriteLine(sb.ToString());

                String[] lines = Regex.Split(message.ToString(), "(<br>)|(</br>)|(<br/>)", 
                    RegexOptions.IgnoreCase); 
                
                foreach (String line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
