using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Utilities
{
    public partial class ExtendedObject
    {

        public static readonly string gothicErrLocation = "ExtendedObject.Client.cs";

        static partial void BuildMessage (ref string msg, object[] options, params object[] args)
        {
            msg = "";
            try
            {
                if ((args == null) || (args.Length < 2))
                {
                    return;
                }

                // additional options
                if (options != null)
                {
                    // print in new line?
                    bool asNewLine = false;
                    try
                    {
                        asNewLine = (bool) options[0];
                    }
                    catch
                    { }
                    if (asNewLine)
                    {
                        msg += "\n";
                    }
                }

                msg += args[0].ToString() + ": ";
                for (int i = 1; i < args.Length; i++)
                {
                    msg += args[i].ToString();
                }
            }
            catch
            {
                return;
            }
        }

        static partial void ToOutputController (int msgType, String msg, params object[] args)
        {
            switch (msgType)
            {
                case 0:
                    Logger.Log(msg);
                    break;
                case 1:
                    Logger.Log(msg);
                    break;
                case 2:
                    Logger.Log(msg);
                    break;
                case 3:
                    Logger.LogWarning(msg);
                    break;
                default:
                    Logger.Log(msg);
                    break;
            }
        }

    }
}
