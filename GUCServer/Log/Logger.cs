﻿using System;
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
        static Logger ()
        {
            sb = new StringBuilder();
            if (!File.Exists("serverlog.html"))
                File.Create("serverlog.html").Close();
            //File.Delete("serverlog.html");

            myWriter = new StreamWriter(File.Open(@"serverlog.html", FileMode.Append));

        }

        ~Logger ()
        {
            if (myWriter != null)
            {
                myWriter.Flush();
                myWriter.Close();
                myWriter.Dispose();
            }
        }

        public static void log (LogLevel ll, object message, params object[] args)
        {
            log((int) ll, message);
        }

        public static void logWarning (object message, params object[] args)
        {
            log((int) LogLevel.WARNING, message);
        }

        public static void logError (object message, params object[] args)
        {
            log((int) LogLevel.ERROR, message);
        }

        public static void log (object message, params object[] args)
        {
            log((int) LogLevel.INFO, message, args);
        }

        public static void log (int level, object message, params object[] args)
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
        public static void print (object message, params object[] args)
        {
            try
            {
                String[] lines = null;

                if ((args == null) || (args.Length < 1))
                {
                    lines = Regex.Split(message.ToString(),
                    "(<br>)|(</br>)|(<br/>)", RegexOptions.IgnoreCase);
                }
                else
                {
                    lines = Regex.Split(String.Format(message.ToString(), args),
                    "(<br>)|(</br>)|(<br/>)", RegexOptions.IgnoreCase);
                }
                

                lock (lock_LogObject)
                {
                    foreach (String line in lines)
                    {
                        Console.WriteLine("\r" + line.PadRight(Console.WindowWidth - 1));
                    }

                    if (typedText.Length > 0)
                    {
                        Console.Write(">" + typedText);
                    }
                }
            }
            catch (Exception ex)
            {
                print("GOTCHA");
            }
        }

        public static Action<string> OnCommand;
        private static object lock_KeyObject = new object();
        private static StringBuilder typedText = new StringBuilder();
        private static String currentText = "";
        private static List<String> previousText = new List<String>();
        private static int maxPreviousText = 20;
        private static int previousTextIndex = previousText.Count;
        private static ConsoleColor consoleBGCol = Console.BackgroundColor;
        private static ConsoleColor consoleFGCol = Console.ForegroundColor;
        private static int[] cursorPosition = new int[]{ 0, 0 }; // { left, top }
        private static ConsoleColor cursorBGCol = consoleFGCol;
        private static ConsoleColor cursorFGCol = consoleBGCol;
        private static String CmdLineMarker = "";
        internal static void RunLog()
        {
            cursorPosition[1] = Console.CursorTop;
            ConsoleKeyInfo cki;
            while (true)
            {
                Console.CursorVisible = false;
                try
                {
                    cki = Console.ReadKey();
                    lock (lock_KeyObject)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Enter:
                                if (OnCommand != null)
                                {
                                    OnCommand(typedText.ToString());
                                }
                                previousText.Add(typedText.ToString());
                                previousTextIndex++;
                                while (previousText.Count > maxPreviousText)
                                {
                                    if (previousText.Count > 0)
                                    {
                                        previousText.RemoveAt(0);
                                        previousTextIndex--;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                typedText.Clear();
                                cursorPosition[0] = 0;
                                break;

                            case ConsoleKey.Escape:
                                Console.SetCursorPosition(0, 0);
                                typedText.Clear();
                                cursorPosition[0] = 0;
                                break;

                            case ConsoleKey.Backspace:
                                if (typedText.Length > 0)
                                {
                                    if ((cursorPosition[0] - 1) >= 0)
                                    {
                                        typedText.Remove(cursorPosition[0] - 1, 1);
                                        cursorPosition[0]--;
                                        if (cursorPosition[0] < 0)
                                        {
                                            cursorPosition[0] = 0;
                                        }
                                    }
                                }
                                break;

                            case ConsoleKey.Delete:
                                if (typedText.Length > 0)
                                {
                                    if (cursorPosition[0] < typedText.Length)
                                    {
                                        typedText.Remove(cursorPosition[0], 1);
                                    }
                                }
                                break;

                            case ConsoleKey.UpArrow:
                                if (previousText.Count > 0)
                                {
                                    previousTextIndex--;
                                    if (previousTextIndex < 0)
                                    {
                                        previousTextIndex = 0;
                                    }
                                    typedText.Clear();
                                    typedText.Append(previousText[previousTextIndex]);
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (previousText.Count > 0)
                                {
                                    previousTextIndex++;
                                    if (previousTextIndex >= previousText.Count)
                                    {
                                        previousTextIndex = previousText.Count;
                                        typedText.Clear();
                                        cursorPosition[0] = 0;
                                    }
                                    else
                                    {
                                        typedText.Clear();
                                        typedText.Append(previousText[previousTextIndex]);
                                    }
                                }
                                else
                                {
                                    typedText.Clear();
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                cursorPosition[0]--;
                                if (cursorPosition[0] < 0)
                                {
                                    cursorPosition[0] = 0;
                                }
                                break;

                            case ConsoleKey.RightArrow:
                                cursorPosition[0]++;
                                if (cursorPosition[0] > typedText.Length)
                                {
                                    cursorPosition[0] = typedText.Length;
                                }
                                break;

                            default:
                                if (cki.KeyChar != '\0')
                                {
                                    typedText.Insert(cursorPosition[0], cki.KeyChar);
                                    cursorPosition[0]++;
                                }
                                break;
                        }


                        //print(currentText.Length + " < " + cursorPosition[0] + "|" + cursorPosition[1]);

                        // correct cursor position in respect line breaks
                        //cursorPosition[1] = cursorPosition[0] / (Console.WindowWidth 
                        //    + CmdLineMarker.Length);
                        //cursorPosition[0] = cursorPosition[0] % (Console.WindowWidth 
                        //    + CmdLineMarker.Length);
                        currentText = typedText.ToString();

                        //print(currentText);
                        //print(currentText.Length + " < " + cursorPosition[0] + "|" + cursorPosition[1]);

                        if (currentText.Length > 0)
                        {
                            //Console.Write("\r".PadRight(Console.WindowWidth) + "\r"
                            //    + CmdLineMarker);
                            Console.Write("\r" + CmdLineMarker);

                            //Console.Write(currentText);

                            if (cursorPosition[0] > 1)
                            {
                                Console.Write(currentText.Substring(0, cursorPosition[0] - 1));
                            }
                            Console.BackgroundColor = cursorBGCol;
                            Console.ForegroundColor = cursorFGCol;
                            if (cursorPosition[0] > 0)
                            {
                                Console.Write(currentText.Substring(cursorPosition[0] - 1, 1));
                            }
                            Console.BackgroundColor = consoleBGCol;
                            Console.ForegroundColor = consoleFGCol;

                            if (cursorPosition[0] < currentText.Length)
                            {
                                Console.Write(currentText.Substring(cursorPosition[0],
                                currentText.Length - cursorPosition[0]));
                            }
                        }
                        else
                        {
                            Console.Write("\n\r>");
                        }
                    }
                }
                catch (Exception e)
                {
                    Logger.log(Logger.LOG_ERROR, e.Message);
                    Logger.log(Logger.LOG_ERROR, e.Source);
                    Logger.log(Logger.LOG_ERROR, e.StackTrace);
                }
            }
        }
    }
}