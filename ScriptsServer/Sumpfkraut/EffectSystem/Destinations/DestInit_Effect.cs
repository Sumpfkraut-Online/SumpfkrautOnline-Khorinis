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
 
                var components = totalChange.Components;
                Effect effect;

                if (components.Count > 0)
                {
                    effect = components[components.Count - 1].GetEffect();
                    if (Effect.GlobalEffectExists(effect.))
                }
                for (int c = 0; c < components.Count; c++)
                {
                    // allow only 1 copy of the effect in the globals
                    
                    
                    //components[c].GetEffect().SetEffectName((string) components[c].GetParameters()[0]);
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

    }

}
