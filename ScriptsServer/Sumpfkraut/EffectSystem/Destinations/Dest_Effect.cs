using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class Dest_Effect : BaseDestination
    {

        new public static readonly string _staticName = "Dest_Effect (static)";

        new public static readonly ChangeType[] supportedChangeTypes = new ChangeType[]
        {
            ChangeType.Effect_Name_Set
        };



        public static void ApplyEffectNames (EffectHandlers.BaseEffectHandler effectHandler)
        {
            // assuming that the EffectName is only set --> set it in the respective effect of each change
            try
            {
                TotalChange totalChange;

                if (!effectHandler.DestinationToTotal.TryGetValue(
                    ChangeDestination.Effect_Name, out totalChange))
                {
                    return;
                }
                if (totalChange == null) { return; }

                List<BaseChange> components = totalChange.Components;
                for (int c = 0; c < components.Count; c++)
                {
                    components[c].Effect.SetEffectName((string) components[c].Parameters[0]);
                }
            }
            catch (Exception ex)
            {
                MakeLogErrorStatic(typeof(Dest_Effect), ex);
            }
        }

    }

}
