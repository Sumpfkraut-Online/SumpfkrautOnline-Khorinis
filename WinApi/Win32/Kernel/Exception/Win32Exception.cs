using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Kernel.Exception
{
    class Win32Exception : System.Exception
    {
        public int ErrorCode { get; protected set; }

        public Win32Exception(String message, int ec) : base(message)
        {
            this.ErrorCode = ec;
        }

        public Win32Exception(int ec)
        {
            ErrorCode = ec;
        }
    }
}
