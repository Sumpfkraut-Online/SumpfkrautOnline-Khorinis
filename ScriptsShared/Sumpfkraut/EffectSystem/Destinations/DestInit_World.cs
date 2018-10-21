using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public interface DestInit_World
    {

        void CalculateClock_IsRunning (BaseEffectHandler eh, TotalChange tc);
        void ApplyClock_IsRunning (BaseEffectHandler eh, TotalChange tc);

        void CalculateClock_Rate (BaseEffectHandler eh, TotalChange tc);
        void ApplyClock_Rate (BaseEffectHandler eh, TotalChange tc);

        void CalculateClock_Time (BaseEffectHandler eh, TotalChange tc);
        void ApplyClock_Time (BaseEffectHandler eh, TotalChange tc);

    }

}
