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
                var gCtrl = new zCSkyControler_Outdoor(address);

                int m_enuWeather = Process.ReadInt(address + 0x30);
                if (m_enuWeather != (int)this.type)
                {
                    Process.Write((int)this.type, address + 0x30);
                    int rainAddr = Process.ReadInt(address + zCSkyControler_Outdoor.VarOffsets.outdoorRainFX);
                    if (rainAddr != 0)
                    {
                        new zCOutdoorRainFX(rainAddr).SetWeatherType((int)this.type);
                    }
                    else
                    {
                        throw new Exception("Wtf");
                    }
                }

                gCtrl.OutdoorRainFXWeight = this.currentWeight;
                if (this.type == WeatherTypes.Rain && this.currentWeight > 0.5f)
                    gCtrl.RenderLightning = true;
                else
                    gCtrl.RenderLightning = false;
            }
        }
    }
}
