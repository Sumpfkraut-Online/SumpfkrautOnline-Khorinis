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
