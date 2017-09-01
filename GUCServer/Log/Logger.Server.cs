using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using GUC.Options;

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

            DisableQuickEditMode();
        }

        #region Disable Quick Edit Mode

        // Quick Edit Mode halts the process if one marks something in the console window until a key is pressed again

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr GetStdHandle(int nStdHandle);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        static void DisableQuickEditMode()
        {
            var handle = GetStdHandle(-10); // Get standard input device
            uint mode;
            if (GetConsoleMode(handle, out mode))
                SetConsoleMode(handle, mode & ~(uint)0x40); // disable quick edit mode
        }
        #endregion

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
            string text = CmdLineMarker;
            if (updateTypedText)
            {
                int oldLen = currentText.Length;
                currentText = typedText.ToString();
                int diff = oldLen - currentText.Length;

                text += currentText;
                if (diff > 0)
                    text += new string(' ', diff);
            }
            else
            {
                text += currentText;
            }

            Console.SetCursorPosition(0, cursorPos[1]);
            Console.Write(text);
            cursorPos[1] = Console.CursorTop - text.Length / Console.BufferWidth;

            int visX = CmdLineMarker.Length + cursorPos[0];
            int visY = cursorPos[1] + visX / Console.BufferWidth;
            visX %= Console.BufferWidth;
            Console.SetCursorPosition(visX, visY);
        }

        static void WriteNewLine(string text)
        {
            Console.SetCursorPosition(0, cursorPos[1]);
            Console.Write(text);
            Console.Write(new string(' ', Console.BufferWidth - Console.CursorLeft));
            cursorPos[1] = Console.CursorTop;
        }

        static object lock_KeyObject = new object();
        internal static void RunLog()
        {
            while (true)
            {
                try
                {
                    WriteCurrentText(true);

                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    lock (lock_KeyObject)
                    {
                        switch (cki.Key)
                        {
                            case ConsoleKey.Enter:
                                string text = currentText;
                                if (text.Length > 0)
                                {
                                    if (previousTexts.Count == MaxPreviousTexts)
                                        previousTexts.RemoveAt(previousTexts.Count - 1);

                                    previousTexts.Insert(0, text);
                                    WriteNewLine(text);

                                    typedText.Clear();
                                    currentText = string.Empty;
                                    cursorPos[0] = 0;

                                    if (!ServerOptions.ProcessCmd(text) && OnCommand != null)
                                    {
                                        OnCommand(text);
                                    }
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
                                if (previousIndex >= 0)
                                {
                                    previousIndex--;
                                }
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
