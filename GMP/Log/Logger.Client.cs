using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;

namespace GUC.Log
{
    public static partial class Logger
    {
        static partial void LogMessage(LogLevels level, object message, params object[] args)
        {
            zERROR.GetZErr(GUC.Client.Program.Process).Report((int)level + 1, 'G', String.Format(message.ToString(), args), 0, "GUC", 0);
        }
    }
}
