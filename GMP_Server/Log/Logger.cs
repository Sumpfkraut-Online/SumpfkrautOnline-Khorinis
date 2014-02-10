using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GMP_Server.Log
{
    public class Logger
    {
        public static int LOG_INFO = 0;
        public static int LOG_WARNING = 1;
        public static int LOG_ERROR = 2;

        static StreamWriter myWriter = null;
        static Logger()
        {
            if (File.Exists("serverlog.html"))
                File.Delete("serverlog.html");
            File.Create("serverlog.html").Close();
            //myWriter = new StreamWriter(File.Open(@"serverlog.html", FileMode.Append, FileAccess.ReadWrite));
        }

        public static void log(int level, String Message)
        {
            try
            {
                StreamWriter myWriter = new StreamWriter(File.Open(@"serverlog.html", FileMode.Append));
                myWriter.WriteLine(Message + "<br>");
                myWriter.Close();
                myWriter.Dispose();

                Console.WriteLine(Message);
            }
            catch (Exception ex)
            { }
        }
    }
}
