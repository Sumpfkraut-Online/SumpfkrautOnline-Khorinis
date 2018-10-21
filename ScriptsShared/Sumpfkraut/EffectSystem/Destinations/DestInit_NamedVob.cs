using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Basic behavior methods of named vobs when managed by the EffectSystem.
    /// </summary>
    public interface DestInit_NamedVob
    {

        void CalculateName (BaseEffectHandler eh, TotalChange tc);
        void ApplyName (BaseEffectHandler eh, TotalChange tc);

    }

}
