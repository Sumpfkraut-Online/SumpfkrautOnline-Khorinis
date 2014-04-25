using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.WorldCommands
{
    class RainMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte wt = 0, starthour = 0, startminute = 0, endhour = 0, endminute;

            stream.Read(out wt);
            stream.Read(out starthour);
            stream.Read(out startminute);
            stream.Read(out endhour);
            stream.Read(out endminute);

            sWorld.WeatherType = wt;
            sWorld.StartRainHour = starthour;
            sWorld.StartRainMinute = startminute;
            sWorld.EndRainHour = endhour;
            sWorld.EndRainMinute = endminute;


            if (!(Program._state is GUC.States.GameState))
                return;

            Process process = Process.ThisProcess();
            if(sWorld.WeatherType != 2)
                oCGame.Game(process).World.SkyControlerOutdoor.SetWeatherType(sWorld.WeatherType);
            oCGame.Game(process).World.SkyControlerOutdoor.setRainTime(sWorld.StartRainHour, sWorld.StartRainMinute, sWorld.EndRainHour, sWorld.EndRainMinute);
            if (sWorld.WeatherType != 2)
                oCGame.Game(process).World.SkyControlerOutdoor.SetWeatherType(wt);
            
            //oCGame.Game(process).WorldTimer.SetTime(hour, minute);
        }
    }
}
