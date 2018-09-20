using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GUC.Injection.Utilities
{
    static class Error
    {
        public static void GetLast(string message = "")
        {
            int errorcode = Marshal.GetLastWin32Error();
            switch (errorcode)
            {
                case 5:
                    throw new Exception("Access Denied!");
                case 299:
                    break;
                default:
                    throw new Exception(errorcode + " " + message);
            }
        }
    }
}
