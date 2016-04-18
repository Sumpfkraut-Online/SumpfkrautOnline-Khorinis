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
        public static void print(object message, params object[] args)
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
                        WriteNewLine(line);
                    }
                    WriteCurrentText();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        const int MaxPreviousTexts = 20;
        static List<string> previousTexts = new List<string>(MaxPreviousTexts);
        static int previousIndex = -1;

        const string CmdLineMarker = ">";
        public static Action<string> OnCommand;

        static StringBuilder typedText = new StringBuilder();
        static string currentText = typedText.ToString();
        static int[] cursorPos = { 0, Console.CursorTop };

        static void WriteCurrentText(bool updateTypedText = false)
        {
            int[] visPos = { CmdLineMarker.Length + cursorPos[0], cursorPos[1] };

            visPos[1] += visPos[0] / Console.WindowWidth;
            visPos[0] %= Console.WindowWidth;

            Console.SetCursorPosition(0, cursorPos[1]);

            if (updateTypedText)
            {
                int oldLen = currentText.Length;
                currentText = typedText.ToString();
                Console.Write(CmdLineMarker + currentText);
                int diff = oldLen - currentText.Length;
                if (diff > 0)
                    Console.Write(new string(' ', diff+1));
            }
            else
            {
                Console.Write(CmdLineMarker + currentText);
            }

            Console.SetCursorPosition(visPos[0], visPos[1]);
        }

        static void WriteNewLine(string text)
        {
            Console.SetCursorPosition(0, cursorPos[1]);
            Console.Write(text);
            Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));

            cursorPos[1] += 1 + text.Length / Console.WindowWidth;
        }

        static object lock_KeyObject = new object();
        internal static void RunLog()
        {
            while (true)
            {
                try
                {
                    WriteCurrentText(true);

                    ConsoleKeyInfo cki = Console.ReadKey();
                    lock (lock_KeyObject)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Enter:
                                if (currentText.Length > 0)
                                {
                                    if (OnCommand != null)
                                    {
                                        OnCommand(currentText);
                                    }

                                    if (previousTexts.Count == MaxPreviousTexts)
                                        previousTexts.RemoveAt(previousTexts.Count - 1);

                                    previousTexts.Insert(0, currentText);
                                    WriteNewLine(currentText);
                                    typedText.Clear();
                                    cursorPos[0] = 0;
                                }
                                previousIndex = -1;
                                break;

                            case ConsoleKey.Escape:
                                typedText.Clear();
                                cursorPos[0] = 0;
                                previousIndex = -1;
                                break;

                            case ConsoleKey.Backspace:
                                if (cursorPos[0] > 0 && cursorPos[0] <= currentText.Length)
                                {
                                    typedText.Remove(cursorPos[0] - 1, 1);
                                    cursorPos[0]--;
                                }
                                break;

                            case ConsoleKey.Delete:
                                if (cursorPos[0] >= 0 && cursorPos[0] < currentText.Length)
                                {
                                    typedText.Remove(cursorPos[0], 1);
                                }
                                break;

                            case ConsoleKey.UpArrow:
                                if (previousTexts.Count > 0 && previousIndex < previousTexts.Count - 1)
                                {
                                    previousIndex++;
                                    typedText.Clear();
                                    typedText.Append(previousTexts[previousIndex]);
                                    cursorPos[0] = previousTexts[previousIndex].Length;
                                }
                                break;
                            case ConsoleKey.DownArrow:
                                if (previousTexts.Count > 0 && previousIndex >= 0)
                                {
                                    previousIndex--;
                                    typedText.Clear();
                                    if (previousIndex >= 0)
                                    {
                                        typedText.Append(previousTexts[previousIndex]);
                                        cursorPos[0] = previousTexts[previousIndex].Length;
                                    }
                                    else
                                    {
                                        cursorPos[0] = 0;
                                    }
                                }
                                break;
                            case ConsoleKey.LeftArrow:
                                if (cursorPos[0] > 0)
                                {
                                    cursorPos[0]--;
                                }
                                break;

                            case ConsoleKey.RightArrow:
                                if (cursorPos[0] < currentText.Length)
                                {
                                    cursorPos[0]++;
                                }
                                break;

                            default:
                                if (cki.KeyChar != '\0')
                                {
                                    typedText.Insert(cursorPos[0], cki.KeyChar);
                                    cursorPos[0]++;
                                }
                                break;
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
