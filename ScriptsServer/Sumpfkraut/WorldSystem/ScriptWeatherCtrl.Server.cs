using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Types;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class ScriptWeatherCtrl
    {
        GUCTimer rainTimer;

        partial void pConstruct()
        {
            rainTimer = new GUCTimer(2 * TimeSpan.TicksPerMinute, OnRainChange);
        }
        
        void OnRainChange()
        {
            if (Randomizer.GetInt(0, 5) == 0)
            {
                SetNextWeight(Randomizer.GetInt(60, 360) * TimeSpan.TicksPerSecond, (float)Randomizer.GetDouble()); // rain
            }
            else
            {
                SetNextWeight(Randomizer.GetInt(60, 360) * TimeSpan.TicksPerSecond, 0.0f); // sun
            }
        }

        public void StartRainTimer()
        {
            rainTimer.Start();
        }

        public void StopRainTimer()
        {
            rainTimer.Stop();
        }
    }
}
