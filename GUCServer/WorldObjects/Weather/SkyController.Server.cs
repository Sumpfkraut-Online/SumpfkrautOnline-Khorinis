using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network.Messages;

namespace GUC.WorldObjects.Weather
{
    public partial class SkyController
    {
        partial void pSetRainTime()
        {
            if (this.world.IsCreated)
                WorldMessage.WriteWeatherMessage(this.world, this);
        }

        partial void pSetWeatherType()
        {
            if (this.world.IsCreated)
                WorldMessage.WriteWeatherTypeMessage(this.world, this);
        }
    }
}
