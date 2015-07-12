using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects;

namespace GUC.Network.Messages.WorldCommands
{
    class TimerMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int day = 0;
            byte hour = 0, minute = 0;

            stream.Read(out day);
            stream.Read(out hour);
            stream.Read(out minute);

            sWorld.Day = day;
            sWorld.Hour = hour;
            sWorld.Minute = minute;

            if (!(Program._state is GUC.States.GameState))
                return;

            Process process = Process.ThisProcess();
            oCGame.Game(process).WorldTimer.SetDay(day);
            oCGame.Game(process).WorldTimer.SetTime(hour, minute);

            if (sWorld.WeatherType != 2)
                oCGame.Game(process).World.SkyControlerOutdoor.SetWeatherType(sWorld.WeatherType);
            oCGame.Game(process).World.SkyControlerOutdoor.setRainTime(sWorld.StartRainHour, sWorld.StartRainMinute, sWorld.EndRainHour, sWorld.EndRainMinute);
        }
    }
}
