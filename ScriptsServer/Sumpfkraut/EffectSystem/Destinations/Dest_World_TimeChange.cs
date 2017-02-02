using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class Dest_World_TimeChange : BaseDestination
    {

        new public static readonly string _staticName = "Dest_Effect (static)";

        new public static readonly ChangeDestination changeDestination = ChangeDestination.Effect_Name;

        new public static readonly ChangeType[] supportedChangeTypes = new ChangeType[]
        {
            ChangeType.Effect_Name_Set
        };



        // make sure, the destination makes itself known to its related changes
        static Dest_World_TimeChange ()
        {
            Change_Effect_Name.influencedDestinations.Add(ChangeDestination.Effect_Name);
        }



        new public static void CalculateTotalChange (BaseEffectHandler effectHandler)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        new public static void ApplyTotalChange (BaseEffectHandler effectHandler)
        {
            // assuming that the EffectName is only set --> set it in the respective effect of each change
            try
            {
                TotalChange totalChange;

                if (!effectHandler.DestToTotalChange.TryGetValue(
                    changeDestination, out totalChange))
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
                MakeLogErrorStatic(typeof(Dest_Effect_Name), "Error while applying TotalChange: " + ex);
            }
        }

    }

}
