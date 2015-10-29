using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.WeatherSystem
{
    class WeatherState
    {

        public WeatherType weatherType;
        public DateTime startTime;
        public DateTime endTime;
        public string description;

        public WeatherState()
        {
            DateTime dtNow = DateTime.Now;
            this.weatherType = WeatherType.undefined;
            this.startTime = dtNow;
            this.endTime = dtNow;
            this.description = "";
        }

        public WeatherState(WeatherType weatherType, DateTime startTime, DateTime endTime)
            : this(weatherType, startTime, endTime, "")
        { }

        public WeatherState(WeatherType weatherType, DateTime startTime, DateTime endTime,
            string description)
        {
            this.weatherType = weatherType;
            this.startTime = startTime;
            this.endTime = endTime;
            this.description = description;
        }

    }
}
