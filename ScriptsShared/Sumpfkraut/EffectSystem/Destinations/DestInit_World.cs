using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Basic behavior methods of worlds when managed by the EffectSystem.
    /// </summary>
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
