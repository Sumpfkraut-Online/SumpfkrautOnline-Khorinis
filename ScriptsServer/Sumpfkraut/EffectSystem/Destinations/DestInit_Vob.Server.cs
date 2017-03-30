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

    public partial class DestInit_Vob
    {

        partial void pCTC_CodeName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                Change finalChange;
                ChangeInitInfo info;
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_CodeName_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange with non-initialized ChangeType "
                        + ChangeType.Vob_CodeName_Set);
                    return;
                }

                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }
                // last codeName counts
                finalChange = Change.Create(info, 
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_CodeName: " + ex); }
        }

        partial void pATC_CodeName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.GetLinkedObject();
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

        partial void pCTC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            // TO DO
        }

        partial void pATC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            // TO DO
        }

        partial void pCTC_VobDefType (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                Change finalChange;
                ChangeInitInfo info;
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_VobDefType_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange via CTC_VobDefType with non-initialized ChangeType "
                        + ChangeType.Vob_VobDefType_Set);
                    return;
                }

                // last component counts as long as the linkedObject still isn't set
                // (changing it afterwards is not possible)
                object linkedObject = eh.GetLinkedObject();
                if (linkedObject != null) { return; }

                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }
                // last codeName counts
                finalChange = Change.Create(info,
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_VobDefType: " + ex); }
        }

        partial void pATC_VobDefType (BaseEffectHandler eh, TotalChange tc)
        {
            // no application necessary because VobDefType is only used 
            // when creating a new instance of VobDef, not afterwards

        }

        partial void pCTC_VobInstType (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                Change finalChange;
                ChangeInitInfo info;
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_VobInstType_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange via CTC_VobInstType with non-initialized ChangeType "
                        + ChangeType.Vob_VobInstType_Set);
                    return;
                }

                // last component counts as long as the linkedObject still isn't set
                // (changing it afterwards is not possible)
                object linkedObject = eh.GetLinkedObject();
                if (linkedObject != null) { return; }

                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }
                // last codeName counts
                finalChange = Change.Create(info, 
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_VobInstType: " + ex); }
        }

        partial void pATC_VobInstType (BaseEffectHandler eh, TotalChange tc)
        {
            // no application necessary because VobInstType is only used 
            // when creating a new instance of VobInst, not afterwards
        }

    }

}
