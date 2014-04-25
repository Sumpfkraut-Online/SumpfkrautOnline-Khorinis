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


        public void testValues(int sizeTest)
        {
            zERROR.GetZErr(Process).Report(2, 'G', "TestValues: "+sizeTest, 0, "Program.cs", 0);
            for (int i = 0; i < sizeTest; i++ )
            {
                int x = Process.ReadInt(Address + i);
                zCVob vob = new zCVob(Process, x);
                Gothic.zTypes.zString str = new zTypes.zString(Process, x);
                Gothic.zTypes.zString str2 = new zTypes.zString(Process, Address + i);

                zERROR.GetZErr(Process).Report(2, 'G', "Class-Info: " + i + " Int: " + x, 0, "Program.cs", 0);

                zERROR.GetZErr(Process).Report(2, 'G', "Class-Info: " + i + " vob:" + vob.VobType, 0, "Program.cs", 0);
                if (str.Address != 0 && str.Length <= 300)
                    zERROR.GetZErr(Process).Report(2, 'G', "Class-Info:" + i + " Str1:" + str.Value, 0, "Program.cs", 0);
                if (str2.Address != 0 && str2.Length <= 300)
                    zERROR.GetZErr(Process).Report(2, 'G', "Class-Info:" + i + " Str2:" + str2.Value, 0, "Program.cs", 0);
            }
        }
    }
}
