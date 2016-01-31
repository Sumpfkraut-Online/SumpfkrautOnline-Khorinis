using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.WeatherSystem
{
    public class WeatherHandler : GUC.Utilities.Threading.AbstractRunnable
    {

        new protected static readonly string _staticName = "WeatherHandler (static)";



        public WeatherHandler (bool startOnCreate)
            : base (false, TimeSpan.Zero, false)
        {
            SetObjName("WeatherHandler (default)");

            if (startOnCreate)
            {
                Start();
            }
        }



        public void Loadweather ()
        {

        }

    }
}
