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
          if (myWriter != null) ;
          {
            myWriter.Flush();
            myWriter.Close();
            myWriter.Dispose();
          }
        }

        public static void log(LogLevel ll, String Message)
        {
            log((int)ll, Message);
        }

        public static void logWarning(String Message)
        {
          log((int)LogLevel.WARNING, Message);
        }

        public static void logError(String Message)
        {
          log((int)LogLevel.ERROR, Message);
        }
        
        public static void log(String Message)
        {
          log((int)LogLevel.INFO, Message);
        }

        public static void log(int level, String Message)
        {
            try
            {
                sb.Clear();
                sb.Append("<span class=\"level_").Append(level).Append("\">").Append(Message).Append("</span><br>");
                myWriter.WriteLine(sb.ToString());


                String[] Messages = Regex.Split(Message, "(<br>)|(</br>)|(<br/>)", RegexOptions.IgnoreCase); 
                
                foreach (String message in Messages)
                {
                    Console.WriteLine(message);
                }
            }
            catch (Exception ex)
            { }
        }
    }
}
