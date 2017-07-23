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

    public partial class DestInit_NamedVob
    {

        partial void pCTC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }

                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.NamedVob_Name_Set, out info)) { return; }

                // last entry counts
                var finalChange = Change.Create(info, 
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCTC_Name: " + ex); }
        }

        partial void pATC_Name (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.Host;
                if      (linkedObj is NamedVobDef)
                {
                    var vobDef = linkedObj as NamedVobDef;
                    vobDef.Name = (string) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is NamedVobInst)
                {
                    var vobInst = linkedObj as VobInst;
                    // TO DO when there is the possiblity to change the name in VobSystem
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pATC_Name: " + ex); }
        }

    }

}
