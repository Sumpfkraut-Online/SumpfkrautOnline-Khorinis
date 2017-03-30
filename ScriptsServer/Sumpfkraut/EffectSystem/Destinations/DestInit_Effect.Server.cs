using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInit_Effect
    {
        
        partial void pCTC_GlobalID (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        partial void pATC_GlobalID (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                Effect effect;
                string globalID;
                var components = tc.Components;
                int i;
                for (i = 0; i < components.Count; i++)
                {
                    effect = components[i].GetEffect();
                    globalID = (string) components[i].GetParameters()[0];
                    if (globalID != null) { Effect.AddGlobalEffect(globalID, effect); }
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_Name: " + ex); }
        }

        partial void pCTC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        partial void pATC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            // assuming that the EffectName is only set --> set it in the respective effect of each change
            try
            {
                List<Change> components = tc.Components;
                for (int c = 0; c < components.Count; c++)
                {
                    components[c].GetEffect().SetEffectName((string) components[c].GetParameters()[0]);
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_Name: " + ex); }
        }

        partial void pCTC_Parent (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        partial void pATC_Parent (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                Effect effect;
                Effect parent;
                string parentGlobalID;
                List<Change> components = tc.Components;
                for (int c = 0; c < components.Count; c++)
                {
                    effect = components[c].GetEffect();
                    parentGlobalID = (string) components[c].GetParameters()[0];
                    if (!Effect.TryGetGlobalEffect(parentGlobalID, out parent))
                    {
                        MakeLogWarning("Did not find effect behind globalID " + parentGlobalID
                            + " to add as parent effect in ATC_Parent");
                    }
                    effect.AddParent(parent);
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_Name: " + ex); }
        }

    }

}
