using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Objects.Sky
{
    public class zCSkyControler : zClass
    {
        public abstract class VarOffsets
        {
            public const int WeatherType = 0x30;
        }

        public const int activeSkyController = 0x0099AC8C;

        public zCSkyControler(int address)
            : base(address)
        {
        }

        public zCSkyControler()
        {
        }

        public static zCSkyControler ActiveSkyController
        {
            get { return new zCSkyControler(Process.ReadInt(activeSkyController)); }
            set { Process.Write(value.Address, activeSkyController); }
        }

        public static int SkyEffectsEnabled
        {
            get { return Process.ReadInt(0x008A5DB0); }
            set { Process.Write(value, 0x008A5DB0); }
        }

        public int m_enuWeather
        {
            get { return Process.ReadInt(Address + VarOffsets.WeatherType); }
            set { Process.Write(value, Address + VarOffsets.WeatherType); }
        }
    }
}
