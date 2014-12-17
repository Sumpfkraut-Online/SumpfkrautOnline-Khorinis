using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zTypes;

namespace Gothic.zClasses
{
    public class zFile : zClass
    {


        public zFile(Process process, int address)
            :base(process, address)
        {}

        public zFile() { }





        public static zFile Create(Process process, String value)
        {

            //0x1B0 => Size

            IntPtr thissArr = process.Alloc(0x1B0);

            zString str = zString.Create(process, value);

            process.THISCALL<NullReturnCall>((uint)thissArr.ToInt32(), (uint)0x00443450, new CallValue[] { str });

            //str.Dispose();



            return new zFile(process, thissArr.ToInt32());
        }


        public override uint ValueLength()
        {
            return 4;
        }


    }
}
