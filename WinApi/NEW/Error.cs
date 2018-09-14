using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using WinApi.Kernel.Exception;

namespace WinApi.NEW
{
    static class Error
    {
        public static void GetLast(string message = "")
        {
            int errorcode = Marshal.GetLastWin32Error();
            switch (errorcode)
            {
                case 5:
                    throw new AccessDenied();
                case 299:
                    break;
                default:
                    throw new Exception(errorcode + " " + message);
            }
        }
    }
}
