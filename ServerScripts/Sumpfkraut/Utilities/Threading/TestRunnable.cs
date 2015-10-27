using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.Utilities.Threading
{
    class TestRunnable : Runnable
    {

        int day = 0;
        int hour = 0;
        int minute = 0;

        WeatherType weatherType = WeatherType.rain;
        IGTime weatherStartTime = new IGTime();
        IGTime weatherEndTime = new IGTime();

        public TestRunnable (bool startOnCreate, TimeSpan timeout, bool runOnce)
            : base(startOnCreate, timeout, runOnce)
        { }

        public override void Run ()
        {
            Console.WriteLine("TestRunnable: " + DateTime.Now + " " + this.runOnce);
            Console.WriteLine("TestRunnable: " + day + " " + hour + " " + minute);
            World.NewWorld.ChangeTime(day, hour, minute);

            weatherStartTime.day = 0;
            weatherStartTime.hour = 12;
            weatherStartTime.minute = 0;
            weatherEndTime.day = 0;
            weatherEndTime.hour = 16;
            weatherEndTime.minute = 0;
            World.NewWorld.ChangeWeather(weatherType, weatherStartTime, weatherEndTime);
            
            hour++;
        }

    }
}
