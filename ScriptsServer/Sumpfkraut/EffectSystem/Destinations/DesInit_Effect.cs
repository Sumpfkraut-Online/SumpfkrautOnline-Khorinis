using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class DesInit_Effect : BaseDestInit
    {

        new public static readonly string _staticName = "DesInit_Effect (static)";

        //new public static readonly ChangeDestination changeDestination = ChangeDestination.Effect_Name;

        //new public static readonly ChangeType[] supportedChangeTypes = new ChangeType[]
        //{
        //    ChangeType.Effect_Name_Set
        //};



        // make sure, the destination makes itself known to its related changes
        static DesInit_Effect ()
        {
            //ChangeInit_Effect.representative.influencedDestinations.Add(ChangeDestination.Effect_Name);
        }



        protected DesInit_Effect ()
        {
            SetObjName("DesInit_Effect");

            AddSupportedChangeType(ChangeType.Effect_Name_Set);
            AddCalculateTotalChange(CTC_Name);
            AddApplyTotalChange(ATC_Name);
        }



        new public static void CTC_Name (BaseEffectHandler effectHandler)
        {
            // because multiple effects can be registered on one effectHandler,
            // there is no need to actually calculate a TotalChange
            // (see ApplyTotalChange for the individual treatment of each Change in TotalChange.components)
        }

        new public static void ATC_Name (BaseEffectHandler effectHandler)
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

                List<Change> components = totalChange.Components;
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
