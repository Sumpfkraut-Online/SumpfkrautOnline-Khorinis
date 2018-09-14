using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinApi.NEW
{
    public struct RegisterMemory
    {
        int address;
        public int Address { get { return this.address; } }
        FastHook hook;
        public FastHook Hook { get { return this.hook; } }

        public RegisterMemory(int address, FastHook hook)
        {
            this.address = address;
            this.hook = hook;
        }
    }
}
