using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects.Sky;
using WinApi;
using GUC.Types;
using GUC.Network;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class WeatherController : SkyController
    {
        #region Network Messages

        internal static class Messages
        {
            public static void ReadWeather(PacketReader stream)
            {
                var weatherCtrl = World.Current.WeatherCtrl;
                weatherCtrl.ReadSetNextWeight(stream);
                weatherCtrl.ScriptObject.SetNextWeight(weatherCtrl.EndTime, weatherCtrl.EndWeight);
            }

            public static void ReadWeatherType(PacketReader stream)
            {
                var weatherCtrl = World.Current.WeatherCtrl;
                weatherCtrl.ReadSetWeatherType(stream);
                weatherCtrl.ScriptObject.SetWeatherType(weatherCtrl.WeatherType);
            }
        }

        #endregion

        partial void pUpdateWeight()
        {
            int address = Process.ReadInt(zCSkyControler.activeSkyController);
            if (address != 0)
            {
                var gCtrl = new zCSkyControler_Outdoor(address);

                int m_enuWeather = Process.ReadInt(address + 0x30);
                if (m_enuWeather != (int)this.type)
                {
                    Process.Write(address + 0x30, (int)this.type);
                    int rainAddr = Process.ReadInt(address + zCSkyControler_Outdoor.VarOffsets.outdoorRainFX);
                    if (rainAddr != 0)
                    {
                        new zCOutdoorRainFX(rainAddr).SetWeatherType((int)this.type);
                    }
                    else
                    {
                        return; 
                    }
                }

                gCtrl.OutdoorRainFXWeight = this.CurrentWeight;
                if (this.type == WeatherTypes.Rain && this.CurrentWeight > 0.5f)
                    gCtrl.RenderLightning = true;
                else
                    gCtrl.RenderLightning = false;
            }
        }
    }
}
