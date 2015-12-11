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
        public const int LOG_INFO = 0;
        public const int LOG_WARNING = 1;
        public const int LOG_ERROR = 2;

        public enum LogLevel
        {
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

        public static void log(LogLevel ll, object message, params object[] args)
        {
            log((int)ll, message);
        }

        public static void logWarning(object message, params object[] args)
        {
            log((int)LogLevel.WARNING, message);
        }

        public static void logError(object message, params object[] args)
        {
            log((int)LogLevel.ERROR, message);
        }

        public static void log(object message, params object[] args)
        {
            log((int)LogLevel.INFO, message, args);
        }

        public static void log(int level, object message, params object[] args)
        {
            try
            {
                sb.Clear();
                sb.Append("<span class=\"level_").Append(level).Append("\">");
                sb.Append(String.Format(message.ToString(), args)).Append("</span><br>");
                myWriter.WriteLine(sb.ToString());

                print(message);
            }
            catch (Exception ex)
            {
            }
        }

        static object lock_LogObject = new object();
        public static void print(object message, params object[] args)
        {
            try
            {
                String[] lines = Regex.Split(String.Format(message.ToString(), args),
                    "(<br>)|(</br>)|(<br/>)", RegexOptions.IgnoreCase);

                lock (lock_LogObject)
                {
                    foreach (String line in lines)
                    {
                        Console.WriteLine("\r" + line.PadRight(Console.WindowWidth-1));
                    }

                    if (typedText.Length > 0)
                    {
                        Console.Write(">" + typedText);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static Action<string> OnCommand;
        static object lock_KeyObject = new object();
        static StringBuilder typedText = new StringBuilder();
        internal static void RunLog()
        {
            ConsoleKeyInfo cki;
            while (true)
            {
                try
                {
                    cki = Console.ReadKey();
                    lock (lock_KeyObject)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Enter:
                                //Console.WriteLine("\r" + typedText.ToString().PadRight(Console.WindowWidth-1));
                                if (OnCommand != null)
                                {
                                    OnCommand(typedText.ToString());
                                }
                                typedText.Clear();
                                break;
                            case ConsoleKey.Escape:
                                Console.SetCursorPosition(0, 0);
                                typedText.Clear();
                                break;
                            case ConsoleKey.Backspace:
                                typedText.Remove(typedText.Length - 1, 1);
                                break;
                            default:
                                if (cki.KeyChar != '\0')
                                {
                                    typedText.Append(cki.KeyChar);
                                }
                                break;
                        }
                        Console.Write("\r".PadRight(Console.WindowWidth));
                        Console.Write("\r>" + typedText);
                    }
                }
                catch (Exception e)
                {
                    Log.Logger.log(Log.Logger.LOG_ERROR, e.Message);
                    Log.Logger.log(Log.Logger.LOG_ERROR, e.Source);
                    Log.Logger.log(Log.Logger.LOG_ERROR, e.StackTrace);
                }
            }
        }
    }
}
