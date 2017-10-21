using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Kernel.Exception
{
    class AccessDenied : Win32Exception
    {
        public AccessDenied() : base("Access denied", 5)
        {
        }
    }
}
