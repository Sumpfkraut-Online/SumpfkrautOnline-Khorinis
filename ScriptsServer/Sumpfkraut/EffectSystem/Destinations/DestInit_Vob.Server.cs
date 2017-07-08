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

    public partial class DestInit_Vob : BaseDestInit
    {

        partial void pCTC_CDDyn (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_CDDyn_Set, out info)) { return; }

                // use default or value from last Change
                bool val = Default_CDDyn;
                if (tc.Components.Count > 0)
                {
                    val = (bool) tc.Components.Last().GetParameters()[0];
                }

                var finalChange = Change.Create(info, new List<object>() { val });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCTC_CDDyn: " + ex); }
        }

        partial void pATC_CDDyn (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.GetHost();
                if (linkedObj is VobDef)
                {
                    var vobDef = linkedObj as VobDef;
                    vobDef.CDDyn = (bool) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is VobInst)
                {
                    // TO DO: add similar code as for VobDef when VobSystem supports it
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pATC_CDDyn: " + ex); }
        }

        partial void pCTC_CDStatic (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_CDStatic_Set, out info)) { return; }

                // use default or value from last Change
                bool val = Default_CDStatic;
                if (tc.Components.Count > 0)
                {
                    val = (bool) tc.Components.Last().GetParameters()[0];
                }

                var finalChange = Change.Create(info, new List<object>() { val });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCTC_CDStatic: " + ex); }
        }

        partial void pATC_CDStatic (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.GetHost();
                if (linkedObj is VobDef)
                {
                    var vobDef = linkedObj as VobDef;
                    vobDef.CDStatic = (bool) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is VobInst)
                {
                    // TO DO: add similar code as for VobDef when VobSystem supports it
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pATC_CDStatic: " + ex); }
        }

        partial void pCTC_CodeName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }

                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_CodeName_Set, out info)) { return; }

                // last entry counts
                var finalChange = Change.Create(info,
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_CodeName: " + ex); }
        }

        partial void pATC_CodeName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.GetHost();
                if (linkedObj is VobDef)
                {
                    var vobDef = linkedObj as VobDef;
                    vobDef.CodeName = (string) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is VobInst)
                {
                    var vobInst = linkedObj as VobInst;
                    // ??? TO DO if codeName should be changable for a VobInst
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_CodeName: " + ex); }
        }

        partial void pCTC_VobType (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }

                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_VobType_Set, out info)) { return; }

                // TO DO: only change VobType if the linked object is actually able to !!!
                // last entry counts
                var finalChange = Change.Create(info,
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCTC_VobType: " + ex); }
        }

        partial void pATC_VobType (BaseEffectHandler eh, TotalChange tc)
        {
            // TO DO: switch VobType of linekd object if possible 
            //        (or simply refrain from changes after creation completely)
        }

    }

}
