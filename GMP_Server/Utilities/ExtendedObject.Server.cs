using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Log;

namespace GUC.Utilities
{

    public abstract partial class ExtendedObject
    {

        static partial void ToOutputController (int msgType, String msg, params object[] args)
        {
            switch (msgType)
            {
                case 0:
                    Logger.log(msg);
                    break;
                case 1:
                    Logger.log(msg);
                    break;
                case 2:
                    Logger.logError(msg);
                    break;
                case 3:
                    Logger.logWarning(msg);
                    break;
                default:
                    Logger.log(msg);
                    break;
            }
        }

    }

}
