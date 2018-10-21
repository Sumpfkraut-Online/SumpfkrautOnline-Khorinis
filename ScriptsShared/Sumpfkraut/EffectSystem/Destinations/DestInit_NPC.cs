using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// Basic behavior methods of npc vobs when managed by the EffectSystem.
    /// </summary>
    public interface DestInit_NPC
    {

        void CalculateTestPoison (BaseEffectHandler eh, TotalChange tc);
        void ApplyTestPoison (BaseEffectHandler eh, TotalChange tc);

    }

}
