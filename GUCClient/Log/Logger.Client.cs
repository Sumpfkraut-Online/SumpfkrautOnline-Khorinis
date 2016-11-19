using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.System;

namespace GUC.Log
{
    public static partial class Logger
    {
        static partial void LogMessage(LogLevels level, object message, params object[] args)
        {
            zError.Report((int)level+1, "M:" + string.Format(message.ToString(), args), 0, 0, "GUC");
        }
    }
}
