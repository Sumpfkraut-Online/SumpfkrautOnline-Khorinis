using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;
using GUC.Types;
using Gothic.zClasses;
using GUC.Enumeration;

namespace GUC.Client.WorldObjects
{

    public class WeatherController : ExtendedObject
    {

        new public static readonly String _staticName = "WeatherController (static)";

        WorldClock worldClock;

        //protected WeatherType weatherType;
        //protected IgTime startTime;
        //protected IgTime endTime;
        protected WeatherEvent activeWeather;
        protected List<WeatherEvent> weatherComponents;
        public static readonly WeatherEvent weatherOverride = new WeatherEvent(
            WeatherType.undefined, new IgTime(0, 0, 0), new IgTime(0, 11, 59));

        protected object weatherLock;



        public WeatherController (WorldClock worldClock)
        {
            SetObjName("WeatherController (default)");

            this.worldClock = worldClock;
            worldClock.OnApplyIgTime += OnIgTimeChange;

            this.activeWeather.weatherType = WeatherType.undefined;
            this.activeWeather.startTime = new IgTime(0, 0, 0);
            this.activeWeather.endTime = new IgTime(0, 23, 59);
            this.weatherComponents = new List<WeatherEvent> { this.activeWeather };

            this.weatherLock = new object();
        }



        public void ApplyWeather (WeatherEvent we)
        {
            float gothicStart = ((((we.startTime.hour + 12f) % 24f) * 60f)
                + (we.startTime.minute)) / (24f * 60f);
            float gothicEnd = ((((we.endTime.hour + 12f) % 24f) * 60f)
                + (we.endTime.minute)) / (24f * 60f);

            oCGame.Game(Program.Process).World.SkyControlerOutdoor.StartRainTime = gothicStart;
            oCGame.Game(Program.Process).World.SkyControlerOutdoor.EndRainTime = gothicEnd;
            oCGame.Game(Program.Process).World.SkyControlerOutdoor.SetWeatherType((int) we.weatherType);

            Print(gothicStart + " ==> " + gothicEnd);
        }

        public void ChangeWeather ()
        {
            ChangeWeather(activeWeather.weatherType, activeWeather.startTime, 
                activeWeather.endTime);
        }

        public void ChangeWeather (WeatherType weatherType, IgTime startTime, IgTime endTime)
        {
            lock (weatherLock)
            {
                List<WeatherEvent> weatherEvents = new List<WeatherEvent>();

                // days are unnecessary on weather-interval time-scale of Gothic 2
                startTime.day = endTime.day = 0;

                // prevent IgTime-objects with 12 o'clock because this will result
                // in faulty weather in Gothic 2 (hardcoded limitation)
                if ((startTime.hour == 12) && (startTime.minute == 0))
                {
                    startTime.minute = 1;
                }
                if ((endTime.hour == 12) && (endTime.minute == 0))
                {
                    endTime.minute = 1;
                }

                // prevent same times as this will result in undefined weather
                // (no controlled change in weather)
                if (startTime == endTime)
                {
                    endTime--;
                }

                Print(startTime.hour + ":" + startTime.minute);
                Print(endTime.hour + ":" + endTime.minute);

                //int daySpan = endTime.day - startTime.day;

                float tempStart = ((((startTime.hour + 12f) % 24f) * 60f)
                    + (startTime.minute)) / (24f * 60f);
                float tempEnd = ((((endTime.hour + 12f) % 24f) * 60f)
                    + (endTime.minute)) / (24f * 60f);

                WeatherEvent tempWE;

                if (tempEnd > tempStart)
                {
                    // requires only 1 additional time interval
                    tempWE = new WeatherEvent();
                    tempWE.weatherType = weatherType;
                    tempWE.startTime = startTime;
                    tempWE.endTime = endTime;
                    weatherEvents.Add(tempWE);
                }
                else
                {
                    // requires 2 additional time intervals
                    // first WeatherEvent goes till almost 1f / 23:59
                    tempWE = new WeatherEvent();
                    tempWE.weatherType = weatherType;
                    tempWE.startTime = startTime;
                    tempWE.endTime = new IgTime(0, 11, 59);
                    weatherEvents.Add(tempWE);
                    // second WeatherEvent fills the residual time iterval
                    tempWE = new WeatherEvent();
                    tempWE.weatherType = weatherType;
                    tempWE.startTime = new IgTime(0, 12, 1);
                    tempWE.endTime = endTime;
                    weatherEvents.Add(tempWE);
                }

                // reset Weather to undefined (no precipitation) for clean setting later on
                ApplyWeather(weatherOverride);

                float gothicStart = 0f;
                float gothicEnd = 0f;

                foreach (WeatherEvent we in weatherEvents)
                {
                    // apply WeaterEvents in Gothic 2-process
                    ApplyWeather(we);
                }

                return;
            }
        }

        public void OnIgTimeChange (IgTime igTime)
        {
            foreach (WeatherEvent we in weatherComponents)
            {
                if (WeatherEvent.InInverval(igTime, we))
                {

                    break;
                }
            }
        }

    }

}
