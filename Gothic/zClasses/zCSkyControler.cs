using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.zClasses
{
    public class zCSkyControler : zClass
    {

        public zCSkyControler(Process process, int address)
            : base(process, address)
        {
        }

        public zCSkyControler() { }



        public override uint ValueLength()
        {
            return 4;
        }


        public static zCSkyControler ActiveSkyController(Process process)
        {
            return new zCSkyControler(process, process.ReadInt(0x0099AC8C));
        }

        public static void ActiveSkyController(Process process, zCSkyControler controler)
        {
            process.Write(controler.Address, 0x0099AC8C);
        }

        public static int SkyEffectsEnabled(Process process)
        {
            return process.ReadInt(0x008A5DB0);
        }

        public static void SkyEffectsEnabled(Process process, int x)
        {
            process.Write(x, 0x008A5DB0);
        }
    }
}
