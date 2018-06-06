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
        const long InvisibleTimeMin = 30 * TimeSpan.TicksPerSecond;
        const long InvisibleTimeMax = 3 * TimeSpan.TicksPerMinute;

        const long VisibleTimeMin = 1 * TimeSpan.TicksPerSecond;
        const long VisibleTimeMax = 20 * TimeSpan.TicksPerSecond;

        const int TransitionSecondsMin = 2;
        const int TransitionSecondsMax = 10;

        GUCTimer timer = new GUCTimer();

        void ShowBarrier()
        {
            long transition = Randomizer.GetInt(TransitionSecondsMin, TransitionSecondsMax) * TimeSpan.TicksPerSecond;
            SetNextWeight(GameTime.Ticks + transition, 1.0f); // barrier visible

            timer.SetInterval(VisibleTimeMin + (long)(Randomizer.GetDouble() * VisibleTimeMax));
            timer.SetCallback(HideBarrier);
        }

        void HideBarrier()
        {
            long transition = Randomizer.GetInt(TransitionSecondsMin, TransitionSecondsMax) * TimeSpan.TicksPerSecond;
            SetNextWeight(GameTime.Ticks + transition, 0.0f); // barrier invisible

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
