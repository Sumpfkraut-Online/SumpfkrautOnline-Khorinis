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
            if (Randomizer.GetInt(0, 3) == 0)
            {
                SetNextWeight(this.World.Clock.Time + Randomizer.GetInt(60, 360), (float)Randomizer.GetDouble()); // rain
            }
            else
            {
                SetNextWeight(this.World.Clock.Time + Randomizer.GetInt(60, 360), 0.0f); // sun
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
