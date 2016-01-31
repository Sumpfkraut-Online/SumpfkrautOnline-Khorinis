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



        public WeatherEvent (WeatherType wt, IgTime st, IgTime et)
        {
            _weatherType = WeatherType.undefined;
            startTime = st;
            endTime = et;

            weatherType = wt;
        }



        public static bool InInverval (IgTime igTime, WeatherEvent weatherEvent)
        {
            bool inInterval = false;

            if ((igTime >= weatherEvent.startTime) 
                && (igTime <= weatherEvent.endTime))
            {
                inInterval = true;
            }

            return inInterval;
        }

    }

}
