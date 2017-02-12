using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class DestInit_Effect : BaseDestInit
    {

        new public static readonly string _staticName = "DestInit_Effect (static)";
        new public static DestInit_Effect representative;

        static TotalChange tc_GlobalID;
        static TotalChange tc_Name;
        static TotalChange tc_Parent;



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
        }



        public void CTC_Child (BaseEffectHandler effectHandler)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_Child (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Effect_Child, out totalChange))
                { return; }

                Effect effect;
                Effect child;
                string childGlobalID;
                List<Change> components = totalChange.Components;
                for (int c = 0; c < components.Count; c++)
                {
                    effect = components[c].GetEffect();
                    childGlobalID = (string) components[c].GetParameters()[0];
                    if (!Effect.TryGetGlobalEffect(childGlobalID, out child))
                    {
                        MakeLogWarning("Did not find effect behind globalID " + childGlobalID
                            + " to add as child effect in ATC_Child");
                    }
                    effect.AddChild(child);
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_Name: " + ex); }
        }

        public void CTC_GlobalID (BaseEffectHandler effectHandler)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_GlobalID (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Effect_Name, out totalChange))
                { return; }

                Effect effect;
                string globalID;
                var components = totalChange.Components;
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

        public void CTC_Name (BaseEffectHandler effectHandler)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_Name (BaseEffectHandler effectHandler)
        {
            // assuming that the EffectName is only set --> set it in the respective effect of each change
            try
            {
                TotalChange totalChange;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Effect_Name, out totalChange))
                { return; }

                List<Change> components = totalChange.Components;
                for (int c = 0; c < components.Count; c++)
                {
                    components[c].GetEffect().SetEffectName((string) components[c].GetParameters()[0]);
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_Name: " + ex); }
        }

        public void CTC_Parent (BaseEffectHandler effectHandler)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ATC_Parent (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Effect_Parent, out totalChange))
                { return; }

                Effect effect;
                Effect parent;
                string parentGlobalID;
                List<Change> components = totalChange.Components;
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
