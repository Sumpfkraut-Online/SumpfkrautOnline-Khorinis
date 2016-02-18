using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Types
{

    public struct WeatherEvent
    {

        private WeatherType _weatherType;
        public WeatherType weatherType
        {
            get 
            {
                return _weatherType; 
            }
            set
            {
                if (Enum.IsDefined(typeof(WeatherType), value))
                {
                    _weatherType = value;
                }
                else
                {
                    _weatherType = WeatherType.undefined;
                }
            }
        }

        public IgTime startTime;
        public IgTime endTime;

        public static WeatherEvent weatherOverride = new WeatherEvent(WeatherType.rain,
            new IgTime(0, 0, 0), new IgTime(0, 0, 1));


        public WeatherEvent (WeatherEvent weatherEvent)
        {
            _weatherType = weatherEvent.weatherType;
            startTime = weatherEvent.startTime;
            endTime = weatherEvent.endTime;
        }

        public WeatherEvent (WeatherType wt, IgTime st, IgTime et)
        {
            _weatherType = WeatherType.undefined;
            startTime = st;
            endTime = et;

            weatherType = wt;
        }



        public static bool operator ==(WeatherEvent we1, WeatherEvent we2)
        {
            return (we1.weatherType == we2.weatherType) 
                && (we1.startTime == we2.startTime)
                && (we1.endTime == we2.endTime);
        }

        public static bool operator !=(WeatherEvent we1, WeatherEvent we2)
        {
            return !(we1 == we2);
        }



        public static bool InInterval (IgTime igTime, WeatherEvent weatherEvent)
        {
            bool inInterval = false;

            // days are irrelevant on the weather interval time scale
            igTime.day = weatherEvent.startTime.day = weatherEvent.endTime.day = 0;
            
            Console.WriteLine(igTime);
            Console.WriteLine(weatherEvent);

            if (weatherEvent.startTime > weatherEvent.endTime)
            {
                //Console.WriteLine("#1");
                // weather-interval spans 2 days
                if ((igTime >= weatherEvent.startTime) 
                    && (igTime >= weatherEvent.endTime))
                {
                    //Console.WriteLine("#1a");
                    //                   X
                    // ------->      |---------
                    // |----------------------|
                    inInterval = true;
                }
                if ((igTime <= weatherEvent.startTime) 
                    && (igTime <= weatherEvent.endTime))
                {
                    //Console.WriteLine("#1b");
                    //    X
                    // ------->      |---------
                    // |----------------------|
                    inInterval = true;
                }
            }
            else
            {
                //Console.WriteLine("#2");
                // weather-interval lies within a single day
                if ((igTime >= weatherEvent.startTime) 
                    && (igTime <= weatherEvent.endTime))
                {
                    //          X
                    //     |-------------->
                    // |----------------------|
                    inInterval = true;
                }
            }

            return inInterval;
        }

        public override string ToString()
        {
            return String.Format("weatherType {0} startTime {1} endTime {2}", 
                this.weatherType, this.startTime, this.endTime);
        }

    }

}
