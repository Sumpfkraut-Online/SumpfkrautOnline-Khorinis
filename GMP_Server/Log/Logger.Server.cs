using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace GUC.Log
{
    public static partial class Logger
    {
        const string LoggerPath = "Log";
        const string LoggerFile = "ServerLog.html";

        static StringBuilder builder = null;
        static StreamWriter writer = null;

        static Logger()
        {
            builder = new StringBuilder();
            if (!Directory.Exists(LoggerPath))
            {
                Directory.CreateDirectory(LoggerPath);
            }

            writer = new StreamWriter(LoggerPath + "\\" + LoggerFile, true);
        }

        static partial void LogMessage(LogLevels level, object message, params object[] args)
        {
            try
            {
                string msg = String.Format(message.ToString(), args);

                if (level != LogLevels.Print)
                {
                    builder.Clear();
                    builder.Append("<span class=\"level_").Append((int)level).Append("\">");
                    builder.Append(msg).Append("</span><br>");
                    writer.WriteLine(builder.ToString());
                    writer.Flush();
                }

                print(msg);
            }
            catch (Exception e)
            {
                print(e);
            }
        }

        static object lock_LogObject = new object();
        static void print(object message, params object[] args)
        {
            try
            {
                String[] lines = null;

                if ((args == null) || (args.Length < 1))
                {
                    lines = Regex.Split(message.ToString(), "(<br>)|(</br>)|(<br/>)", RegexOptions.IgnoreCase);
                }
                else
                {
                    lines = Regex.Split(String.Format(message.ToString(), args), "(<br>)|(</br>)|(<br/>)", RegexOptions.IgnoreCase);
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
            catch (Exception e)
            {
                Console.WriteLine(e);
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
        private static int[] cursorPosition = new int[] { 0, 0 }; // { left, top }
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
                    Logger.LogError(e);
                }
            }
        }
    }
}
