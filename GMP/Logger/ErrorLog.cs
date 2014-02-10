using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GMP.Logger
{
    public class ErrorLog
    {
        public enum LogLevel
        {
            CirticalError,
            Warning,
            Info
        }

        public static void Log(LogLevel ll,Type cl, String message)
        {
            //StreamWriter logF = new StreamWriter("log.txt", true);
            using (StreamWriter writer = new StreamWriter(@"C:\log.txt", true))
            {
                writer.WriteLine(ll.ToString()+" "+cl.FullName + " " + message);
            }
        }

        public static void Log(Type cl, String message)
        {
            //StreamWriter logF = new StreamWriter("log.txt", true);
            //using (StreamWriter writer = new StreamWriter(@"C:\log.txt", true))
            //{
            //    writer.WriteLine(cl.FullName + " " + message);
            //}
        }
    }
}
