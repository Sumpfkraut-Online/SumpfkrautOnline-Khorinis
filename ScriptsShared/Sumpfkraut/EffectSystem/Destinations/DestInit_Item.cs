using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{
    /// <summary>
    /// Basic behavior methods of item vobs when managed by the EffectSystem.
    /// </summary>
    public interface DestInit_Item
    {

        /// <summary>
        /// Calculate the final material value.
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="tc"></param>
        void CalculateMaterial (BaseEffectHandler eh, TotalChange tc);

        /// <summary>
        /// Apply the final material value.
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="tc"></param>
        void ApplyMaterial (BaseEffectHandler eh, TotalChange tc);

    }

}
