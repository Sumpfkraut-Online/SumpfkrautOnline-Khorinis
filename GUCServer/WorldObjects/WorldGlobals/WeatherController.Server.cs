using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class WeatherController : SkyController
    {
        #region Network Messages

        internal static class Messages
        {
            public static void WriteSetWeight(WeatherController weatherController)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldWeatherMessage);
                weatherController.WriteNextWeight(stream);
                weatherController.World.ForEachClient(client => client.Send(stream, NetPriority.Low, NetReliability.Reliable, 'W'));
            }

            public static void WriteSetType(WeatherController weatherController)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldWeatherTypeMessage);
                weatherController.WriteSetWeatherType(stream);
                weatherController.World.ForEachClient(client => client.Send(stream, NetPriority.Low, NetReliability.Reliable, 'W'));
            }
        }

        #endregion

        partial void pSetNextWeight()
        {
            if (this.World.IsCreated)
                Messages.WriteSetWeight(this);
        }

        partial void pSetWeatherType()
        {
            if (this.World.IsCreated)
                Messages.WriteSetType(this);
        }
    }
}
