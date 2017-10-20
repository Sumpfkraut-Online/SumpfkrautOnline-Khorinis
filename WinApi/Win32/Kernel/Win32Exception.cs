using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace WinApi.Kernel
{
    public class Win32Exception : Exception
    {
        public static void ThrowLastError(string message = "")
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), message);
        }

        int errorCode;
        public int ErrorCode { get { return errorCode; } }

        public Win32Exception(int errorCode, string message = "") : base(string.Format("({0}) {1}", errorCode, message))
        {
            this.errorCode = errorCode;
        }
    }
}
