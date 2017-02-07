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



        // make sure, the destination makes itself known to its related changes
        static DestInit_Effect ()
        {
            representative = new DestInit_Effect();
        }

        protected DestInit_Effect ()
        {
            SetObjName("DestInit_Effect");

            AddOrChange(ChangeDestination.Effect_Name, new List<ChangeType>() { ChangeType.Effect_Name_Set }, 
                CTC_Name, ATC_Name);
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

                if (!effectHandler.DestToTotalChange.TryGetValue(
                    ChangeDestination.Effect_Name, out totalChange))
                {
                    return;
                }
                if (totalChange == null) { return; }

                List<Change> components = totalChange.Components;
                for (int c = 0; c < components.Count; c++)
                {
                    components[c].Effect.SetEffectName((string) components[c].Parameters[0]);
                }
            }
            catch (Exception ex)
            {
                MakeLogError("Error while applying TotalChange via ATC_Name: " + ex);
            }
        }

    }

}
