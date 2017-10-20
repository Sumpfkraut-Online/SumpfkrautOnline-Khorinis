using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    partial class ScriptBarrierCtrl
    {
        const long InvisibleTimeMin = 1 * TimeSpan.TicksPerMinute;
        const long InvisibleTimeMax = 5 * TimeSpan.TicksPerMinute;

        const long VisibleTimeMin = 1 * TimeSpan.TicksPerSecond;
        const long VisibleTimeMax = 20 * TimeSpan.TicksPerSecond;

        const int TransitionSecondsMin = 1;
        const int TransitionSecondsMax = 20;

        GUCTimer timer = new GUCTimer();

        void ShowBarrier()
        {
            int transition = (int)(Randomizer.GetInt(TransitionSecondsMin, TransitionSecondsMax) * this.World.Clock.Rate);
            SetNextWeight(this.World.Clock.Time + transition, Randomizer.GetFloat(0.2f, 1.0f)); // barrier visible
            timer.SetInterval(VisibleTimeMin + (long)(Randomizer.GetDouble() * VisibleTimeMax));
            timer.SetCallback(HideBarrier);
        }

        void HideBarrier()
        {
            int transition = (int)(Randomizer.GetInt(TransitionSecondsMin, TransitionSecondsMax) * this.World.Clock.Rate);
            SetNextWeight(this.World.Clock.Time + transition, 0); // barrier invisible
            timer.SetInterval(InvisibleTimeMin + (long)(Randomizer.GetDouble() * InvisibleTimeMax));
            timer.SetCallback(ShowBarrier);
        }

        public void StartTimer()
        {
            HideBarrier();
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
            HideBarrier();
        }
    }
}
