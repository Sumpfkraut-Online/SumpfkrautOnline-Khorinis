using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects.Sky;
using WinApi;
using GUC.Enumeration;

namespace GUC.WorldObjects.Weather
{
    public partial class SkyController
    {
        partial void pUpdateWeather()
        {
            int address = Process.ReadInt(zCSkyControler.activeSkyController);
            if (address != 0)
            {
                int rainAddr = Process.ReadInt(address + zCSkyControler_Outdoor.VarOffsets.outdoorRainFX);
                if (rainAddr != 0)
                {
                    new zCOutdoorRainFX(rainAddr).SetWeatherType((int)this.type);
                }

                var gCtrl = new zCSkyControler_Outdoor(address);
                gCtrl.OutdoorRainFXWeight = this.currentWeight;
                if (this.type == WeatherTypes.Rain && this.currentWeight > 0.5f)
                    gCtrl.RenderLightning = true;
                else
                    gCtrl.RenderLightning = false;
            }
        }
    }
}
