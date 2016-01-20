using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Client;

namespace GUC.Utilities
{
    public partial class ExtendedObject
    {

        public static readonly String gothicErrLocation = "ExtendedObject.Client.cs";

        static partial void ToOutputController (int msgType, String msg, params object[] args)
        {
            switch (msgType)
            {
                case 0:
                    zERROR.GetZErr(Program.Process).Report(2, 'G', msg, 0, gothicErrLocation, 0);
                    break;
                case 1:
                    zERROR.GetZErr(Program.Process).Report(2, 'G', msg, 0, gothicErrLocation, 0);
                    break;
                case 2:
                    zERROR.GetZErr(Program.Process).Report(2, 'G', msg, 0, gothicErrLocation, 0);
                    break;
                case 3:
                    zERROR.GetZErr(Program.Process).Report(3, 'G', msg, 0, gothicErrLocation, 0);
                    break;
                default:
                    zERROR.GetZErr(Program.Process).Report(2, 'G', msg, 0, gothicErrLocation, 0);
                    break;
            }
        }

    }
}
