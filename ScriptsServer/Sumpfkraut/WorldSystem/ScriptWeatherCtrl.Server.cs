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
            long transition = Randomizer.GetInt(60, 360) * TimeSpan.TicksPerSecond;
            if (Randomizer.GetInt(0, 5) == 0)
            {
                SetNextWeight(GameTime.Ticks + transition, (float)Randomizer.GetDouble()); // rain
            }
            else
            {
                SetNextWeight(GameTime.Ticks + transition, 0.0f); // sun
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
