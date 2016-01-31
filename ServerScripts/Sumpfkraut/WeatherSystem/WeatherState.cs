using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Enumeration;

namespace GUC.Server.Scripts.Sumpfkraut.WeatherSystem
{
    public class WeatherState : GUC.Utilities.ExtendedObject
    {

        new protected static readonly string _staticName = "WeatherState (static)";

        public WeatherType weatherType;
        public DateTime startTime;
        public DateTime endTime;
        public string description;

        public WeatherState()
        {
            SetObjName("WeatherState (default)");
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
            SetObjName("WeatherState (default)");
            this.weatherType = weatherType;
            this.startTime = startTime;
            this.endTime = endTime;
            this.description = description;
        }

    }
}
