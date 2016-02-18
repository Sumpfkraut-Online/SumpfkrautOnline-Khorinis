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

        protected WeatherEvent activeWeather;
        protected List<WeatherEvent> weatherComponents;
        protected WeatherEvent lastWeatherComponent;
        protected object weatherLock;



        public WeatherController (WorldClock worldClock)
        {
            SetObjName("WeatherController (default)");

            this.worldClock = worldClock;
            worldClock.OnIgTimeChange += OnIgTimeChange;

            this.activeWeather = WeatherEvent.weatherOverride;
            this.weatherComponents = new List<WeatherEvent> { WeatherEvent.weatherOverride };
            this.lastWeatherComponent = WeatherEvent.weatherOverride;

            this.weatherLock = new object();
        }



        protected void ApplyWeather (WeatherEvent we)
        {
            lock (weatherLock)
            {
                float gothicStart = ((((we.startTime.hour + 12f) % 24f) * 60f)
                    + (we.startTime.minute)) / (24f * 60f);
                float gothicEnd = ((((we.endTime.hour + 12f) % 24f) * 60f)
                    + (we.endTime.minute)) / (24f * 60f);


                //zCSkyControler.SkyEffectsEnabled(Program.Process, 0);

                oCGame.Game(Program.Process).World.SkyControlerOutdoor.SetWeatherType((int) we.weatherType);
                oCGame.Game(Program.Process).World.SkyControlerOutdoor.StartRainTime = gothicStart;
                oCGame.Game(Program.Process).World.SkyControlerOutdoor.EndRainTime = gothicEnd;
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.SetRainFXWeight(0.1f, 2.0f);

                //zCSkyControler.SkyEffectsEnabled(Program.Process, 1);

                // experimental
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.Interpolate();
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.RenderSky();
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.RenderSkyPFX();
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.CalcGlobalWind();
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.ProcessRainFX();
                //oCGame.Game(Program.Process).World.SkyControlerOutdoor.UpdateWorldDependencies();

                oCGame.Game(Program.Process).World.SkyControlerOutdoor.SetTime(0.5f);
                WinApi.FloatArg myTime = oCGame.Game(Program.Process).World.SkyControlerOutdoor.GetTime();
                Print((float) myTime);

                Print(gothicStart + " ==> " + gothicEnd);
            }
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

                weatherComponents = weatherEvents;

                //// reset Weather to undefined (no precipitation) for clean setting later on
                //// -> do it 2x because 1x is not suffice for Gothic 2 for some reason
                //// -> do it with several delays to give the game process some time to adjust
                //Utilities.Threading.Runnable replaceWorker = new Utilities.Threading.Runnable(
                //    false, new TimeSpan(0, 0, 0, 0, 250), false);
                //int counter = 0;
                //replaceWorker.OnRun = delegate (Utilities.Threading.Runnable sender)
                //{
                //    if (counter < 2)
                //    {
                //        ApplyWeather(WeatherEvent.weatherOverride);
                //    }
                //    else if (counter == 2)
                //    {
                //        UpdateWeather(worldClock.GetIgTime());
                //    }
                //    else
                //    {
                //        replaceWorker.Suspend();
                //    }
                //    counter++;
                //};
                //replaceWorker.Start();

                //ApplyWeather(WeatherEvent.weatherOverride);
                //ApplyWeather(WeatherEvent.weatherOverride);
                //ApplyWeather(WeatherEvent.weatherOverride);

                //// apply the new current weather
                //weatherComponents = weatherEvents;
                //UpdateWeather(worldClock.GetIgTime());

                UpdateWeather(worldClock.GetIgTime());



                return;
            }
        }

        public void OnIgTimeChange (IgTime igTime)
        {
            UpdateWeather(igTime);
        }

        public void UpdateWeather (IgTime igNow)
        {
            foreach (WeatherEvent we in weatherComponents)
            {
                //Print(">>> " + igNow);
                //Print(">>> " + lastWeatherComponent);
                //Print(">>> " + we);
                //Print(WeatherEvent.InInterval(igNow, we));
                //Print(lastWeatherComponent != we);
                if (WeatherEvent.InInterval(igNow, we))
                {
                    if (lastWeatherComponent != we)
                    {
                        //Print("~~~~~~> " + weatherComponents.Count);
                        lastWeatherComponent = we;
                        ApplyWeather(we);

                        //float gothicStart = ((((we.startTime.hour + 12f) % 24f) * 60f)
                        //    + (we.startTime.minute)) / (24f * 60f);
                        //float gothicEnd = ((((we.endTime.hour + 12f) % 24f) * 60f)
                        //    + (we.endTime.minute)) / (24f * 60f);

                        //oCGame.Game(Program.Process).World.SkyControlerOutdoor.SetWeatherType((int) we.weatherType);
                        //oCGame.Game(Program.Process).World.SkyControlerOutdoor.StartRainTime = gothicStart;
                        //oCGame.Game(Program.Process).World.SkyControlerOutdoor.EndRainTime = gothicEnd;
                        //oCGame.Game(Program.Process).World.SkyControlerOutdoor.SetRainFXWeight(0.1f, 2.0f);
                    }
                    break;
                }
            }
        }

    }

}
