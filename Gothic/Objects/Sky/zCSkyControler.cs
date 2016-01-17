using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects.Sky
{
    public class zCSkyControler : zClass
    {
        public enum Offsets
        {
            WeatherType = 0x30,

        }
        public zCSkyControler(int address)
            : base(address)
        {
        }

        public zCSkyControler()
        {
        }

        public static zCSkyControler ActiveSkyController
        {
            get { return new zCSkyControler(Process.ReadInt(0x0099AC8C)); }
            set { Process.Write(value.Address, 0x0099AC8C); }
        }

        public static int SkyEffectsEnabled
        {
            get { return Process.ReadInt(0x008A5DB0); }
            set { Process.Write(value, 0x008A5DB0); }
        }
    }
}
