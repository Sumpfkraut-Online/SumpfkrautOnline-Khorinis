using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi.Kernel.Exception
{
    public class WindowNotFoundException : Win32Exception
    {
        public WindowNotFoundException() : base("The Window could not be found.", 0)
        {
        }
    }
}
