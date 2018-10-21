using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Basic behavior methods of Effect objects when managed by the EffectSystem.
    /// </summary>
    public interface DestInit_Effect
    {

        void CalculateGlobalID (BaseEffectHandler eh, TotalChange tc);
        void ApplyGlobalID (BaseEffectHandler eh, TotalChange tc);

        void CalculateName (BaseEffectHandler eh, TotalChange tc);
        void ApplyName (BaseEffectHandler eh, TotalChange tc);

        void CalculateParent (BaseEffectHandler eh, TotalChange tc);
        void ApplyParent (BaseEffectHandler eh, TotalChange tc);

        void CalculatePermanentFlag (BaseEffectHandler eh, TotalChange tc);
        void ApplyPermanentFlag (BaseEffectHandler eh, TotalChange tc);

    }

}
