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

    public partial class DestInit_Item
    {

        partial void pCTC_Material (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Item_Material_Set, out info)) { return; }

                ItemMaterials material;
                if (tc.Components.Count < 1) { material = Default_Material; }
                else
                {
                    // last entry counts
                    material = (ItemMaterials) tc.Components[0].GetParameters()[0];
                }

                var finalChange = Change.Create(info, new List<object>() { material });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCTC_Material: " + ex); }
        }

        partial void pATC_Material (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.GetHost();
                if      (linkedObj is ItemDef)
                {
                    var vobDef = linkedObj as ItemDef;
                    vobDef.Material = (ItemMaterials) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is ItemInst)
                {
                    var vobInst = linkedObj as ItemInst;
                    // TO DO when there is the possiblity to change the name in VobSystem
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pATC_Material: " + ex); }
        }

    }

}
