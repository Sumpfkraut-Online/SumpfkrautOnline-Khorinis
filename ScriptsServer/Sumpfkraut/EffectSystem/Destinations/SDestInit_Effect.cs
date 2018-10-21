using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// ChangeDestination registration and functionality for Effect-objects.
    /// </summary>
    public class SDestInit_Effect : BaseDestInit, DestInit_Effect
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static readonly SDestInit_Effect representative;

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for Effect objects.
        /// </summary>
        static SDestInit_Effect ()
        {
            representative = new SDestInit_Effect();
        }

        /// <summary>
        /// Called on static constructor to initialize singleton object.
        /// </summary>
        protected SDestInit_Effect ()
        {
            SetObjName(typeof(SDestInit_Effect).Name);

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_GlobalID,
                new List<ChangeType>() { ChangeType.Effect_GlobalID_Set },
                CalculateGlobalID, ApplyGlobalID));

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_Name,
                new List<ChangeType>() { ChangeType.Effect_Name_Set },
                CalculateName, ApplyName));

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_Parent,
                new List<ChangeType>() { ChangeType.Effect_Parent_Add },
                CalculateParent, ApplyParent));
        }

        public void CalculateGlobalID (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ApplyGlobalID (BaseEffectHandler eh, TotalChange tc)
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
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pApplyGlobalID: " + ex); }
        }

        public void CalculateName (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ApplyName (BaseEffectHandler eh, TotalChange tc)
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
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ApplyName: " + ex); }
        }

        public void CalculateParent (BaseEffectHandler eh, TotalChange tc)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ApplyParent (BaseEffectHandler eh, TotalChange tc)
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
                            + " to add as parent effect in ApplyParent");
                    }
                    effect.AddParent(parent);
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pApplyParent: " + ex); }
        }

        public void CalculatePermanentFlag (BaseEffectHandler eh, TotalChange tc)
        {
            // there is no need to actually calculate a TotalChange
            // because multiple effects can be registered on one effectHandler,
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        public void ApplyPermanentFlag (BaseEffectHandler eh, TotalChange tc)
        {
            // do nothing because permanent Effects are dealt with when being added to EffectHandlers automatically
        }

    }

}
