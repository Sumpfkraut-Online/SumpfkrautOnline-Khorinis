using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Log
{
    public static partial class Logger
    {
        public const int LOG_INFO = 0;
        public const int LOG_WARNING = 1;
        public const int LOG_ERROR = 2;

        public enum LogLevels
        {
            Info,
            Warning,
            Error
        }

        public static void Log(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Info, message, args);
        }

        public static void LogWarning(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Warning, message, args);
        }

        public static void LogError(object message, params object[] args)
        {
            Logger.LogMessage(LogLevels.Error, message, args);
        }

        public static void Log(int level, object message, params object[] args)
        {
            Logger.LogMessage((LogLevels)level, message, args);
        }

        public static void LogWarning(LogLevels level, object message, params object[] args)
        {
            Logger.LogMessage(level, message, args);
        }

        static partial void LogMessage(LogLevels level, object message, params object[] args);
    }
}
