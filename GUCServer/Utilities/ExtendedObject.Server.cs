using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.Utilities
{

    public abstract partial class ExtendedObject
    {

        static partial void BuildMessage (ref String msg, object[] options, params object[] args)
        {
            msg = "";
            try
            {
                if ((args == null) || (args.Length < 2))
                {
                    return;
                }

                // additional options
                //if (options != null)
                //{
                //    // print in new line?
                //    bool asNewLine = false;
                //    try
                //    {
                //        asNewLine = (bool) options[0];
                //    }
                //    catch (Exception ex)
                //    { }
                //    if (asNewLine)
                //    {
                //        msg += "\n";
                //    }
                //}

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
                    Logger.LogError(msg);
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
