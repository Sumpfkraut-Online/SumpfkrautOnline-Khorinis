using System;
using System.Collections.Generic;
using System.Text;

namespace WinApi
{
    public class HookInfos
    {
        public IntPtr NewFuncAddr;
        public int NewFuncSize;
        public IntPtr oldFuncAddr;
        public IntPtr oldFuncInNewFunc;
        public int oldFuncSize;
        public byte[] oldFunc;
    }
}
