using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Basic behavior methods of vobs in general when managed by the EffectSystem.
    /// </summary>
    public interface DestInit_Vob
    {

        void CalculateCDDyn (BaseEffectHandler eh, TotalChange tc);
        void ApplyCDDyn (BaseEffectHandler eh, TotalChange tc);

        void CalculateCDStatic (BaseEffectHandler eh, TotalChange tc);
        void ApplyCDStatic (BaseEffectHandler eh, TotalChange tc);

        void CalculateCodeName (BaseEffectHandler eh, TotalChange tc);
        void ApplyCodeName (BaseEffectHandler eh, TotalChange tc);

        void CalculateVobType (BaseEffectHandler eh, TotalChange tc);
        void ApplyVobType (BaseEffectHandler eh, TotalChange tc);

    }

}
