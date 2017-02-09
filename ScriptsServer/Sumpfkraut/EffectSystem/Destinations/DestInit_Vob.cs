using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class DestInit_Vob : BaseDestInit
    {

        new public static readonly string _staticName = "DestInit_Vob (static)";
        new public static DestInit_Vob representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_Vob ()
        {
            representative = new DestInit_Vob();
        }

        protected DestInit_Vob ()
        {
            SetObjName("DestInit_Vob");

            
        }

        public void CTC_CodeName (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange = null;
                Change tc;
                ChangeInitInfo info;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Vob_CodeName, out totalChange)) { return; }
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_CodeName_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange with non-initialized ChangeType "
                        + ChangeDestination.Vob_CodeName);
                    return;
                }

                // last codeName counts
                tc = Change.Create(info, null, 
                    new List<object>() { totalChange.Components[totalChange.Components.Count - 1] });
                totalChange.SetTotal(tc);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_CodeName: " + ex); }
        }

        public void ATC_CodeName (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange = null;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Effect_Name, out totalChange)) { return; }

                // apply

                var linkedObj = effectHandler.GetLinkedObject();
                if      (linkedObj is VobDef)
                {
                    linkedObj = linkedObj as VobDef;
                    // set the codeName
                }
                else if (linkedObj is VobInst)
                {
                    linkedObj = linkedObj as VobInst;
                    // set the codeName
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_CodeName: " + ex); }
        }

    }

}
