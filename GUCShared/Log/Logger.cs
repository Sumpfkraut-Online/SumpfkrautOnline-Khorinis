﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Log
{
    public static partial class Logger
    {
        public const int LOG_PRINT = 0;
        public const int LOG_INFO = 1;
        public const int LOG_WARNING = 2;
        public const int LOG_ERROR = 3;

        public enum LogLevels
        {
            Print = LOG_PRINT,
            Info = LOG_INFO,
            Warning = LOG_WARNING,
            Error = LOG_ERROR
        }

        public static void Print(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Print, message, args);
        }

        public static void Log(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Info, message, args);
        }

        public static void LogWarning(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Warning, "[WARNING] " + message, args);
        }

        public static void LogError(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Error, "[ERROR] " + message, args);
        }

        public static void Log(int level, object message, params object[] args)
        {
            Logger.LogMessage((LogLevels)level, message, args);
        }

        public static void LogWarning(LogLevels level, object message, params object[] args)
        {
            Logger.LogMessage(level, "[WARNING] " + message, args);
        }

        static partial void LogMessage(LogLevels level, object message, params object[] args);
    }
}
