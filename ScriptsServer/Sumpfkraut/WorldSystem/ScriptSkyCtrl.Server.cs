using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class ScriptSkyCtrl
    {
        GUCTimer rainTimer;

        partial void pConstruct()
        {
            rainTimer = new GUCTimer(2 * TimeSpan.TicksPerMinute, OnRainChange);
        }

        Random rand = new Random();
        void OnRainChange()
        {
            if (rand.Next(0, 3) == 0)
            {
                SetRainTime(this.World.Clock.Time + rand.Next(60, 360), (float)rand.NextDouble()); // rain
            }
            else
            {
                SetRainTime(this.World.Clock.Time + rand.Next(60, 360), 0.0f); // sun
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

        partial void pSetRainTime(WorldTime time, float weight)
        {
            rainTimer.Restart();
        }
    }
}
