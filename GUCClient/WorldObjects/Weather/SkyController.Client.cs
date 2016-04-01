using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects.Sky;
using WinApi;

namespace GUC.WorldObjects.Weather
{
    public partial class SkyController
    {
        partial void pUpdateWeather()
        {
            int address = Process.ReadInt(zCSkyControler.activeSkyController);
            if (address != 0)
                new zCSkyControler_Outdoor(address).OutdoorRainFXWeight = this.currentWeight;
        }
    }
}
