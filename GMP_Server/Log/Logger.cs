using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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



        static StreamWriter myWriter = null;
        static Logger()
        {
            if (File.Exists("serverlog.html"))
                File.Delete("serverlog.html");
            File.Create("serverlog.html").Close();
        }

        public static void log(LogLevel ll, String Message)
        {
            log((int)ll, Message);
        }

        public static void log(int level, String Message)
        {
            try
            {
                StreamWriter myWriter = new StreamWriter(File.Open(@"serverlog.html", FileMode.Append));
                myWriter.WriteLine("<span class=\"level_" + level + "\">" + Message + "</span><br>");
                myWriter.Close();
                myWriter.Dispose();

                String consoleMessage = Message;
                String[] Messages = consoleMessage.Split(new string[]{"<br>", "<BR>", "</BR>", "</br>", "<br/>", "<BR/>"}, StringSplitOptions.None);

                foreach (String message in Messages)
                {
                    Console.WriteLine(message);
                }
                //Console.WriteLine(Message);
            }
            catch (Exception ex)
            { }
        }
    }
}
