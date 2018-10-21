using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public interface DestInit_NPC
    {

        void CalculateTestPoison (BaseEffectHandler eh, TotalChange tc);
        void ApplyTestPoison (BaseEffectHandler eh, TotalChange tc);

    }

}
