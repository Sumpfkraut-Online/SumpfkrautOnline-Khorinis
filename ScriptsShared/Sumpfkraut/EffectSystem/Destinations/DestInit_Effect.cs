using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInit_Effect : BaseDestInit
    {

        new public static readonly string _staticName = "DestInit_Effect (static)";
        new public static DestInit_Effect representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_Effect ()
        {
            representative = new DestInit_Effect();
        }

        protected DestInit_Effect ()
        {
            SetObjName("DestInit_Effect");

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_GlobalID, 
                new List<ChangeType>() { ChangeType.Effect_GlobalID_Set }, 
                CTC_GlobalID, ATC_GlobalID));

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_Name, 
                new List<ChangeType>() { ChangeType.Effect_Name_Set }, 
                CTC_Name, ATC_Name));

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_Parent, 
                new List<ChangeType>() { ChangeType.Effect_Parent_Add }, 
                CTC_Parent, ATC_Parent));
        }



        public void CTC_GlobalID (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_GlobalID (BaseEffectHandler eh, TotalChange tc)
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

        public void CTC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_Name (BaseEffectHandler eh, TotalChange tc)
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

        public void CTC_Parent (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_Parent (BaseEffectHandler eh, TotalChange tc)
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
