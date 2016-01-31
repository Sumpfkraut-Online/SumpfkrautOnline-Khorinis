using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    
    public enum WeatherType : byte
    {
        undefined = 0,
        rain = undefined + 1,
        snow = rain + 1,
    }

}
