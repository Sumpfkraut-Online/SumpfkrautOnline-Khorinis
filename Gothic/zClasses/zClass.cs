using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using System.Collections;

namespace Gothic.zClasses
{
    public abstract class zClass : CallValue
    {

        public zClass(Process process, int address)
        {
            this.Initialize(process, address);
        }

        public zClass()
        {
            
        }


        public virtual int SizeOf()
        {
            return 0;
        }
    }
}
