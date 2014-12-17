using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zTypes
{
    public class zMat4 : zClass
    {
        public zMat4(Process process, int address)
            : base(process, address)
        {
            
        }

        public void SetAtVector(zVec3 vec)
        {
            Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x0056B960, new CallValue[] { vec });
        }




    }
}
