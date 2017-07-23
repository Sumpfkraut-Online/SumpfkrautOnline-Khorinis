using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInit_Item : BaseDestInit
    {

        new public static DestInit_Item representative;

        public static readonly ItemMaterials Default_Material = ItemMaterials.Wood;



        // make sure, the destination makes itself known to its related changes
        static DestInit_Item ()
        {
            representative = new DestInit_Item();
        }

        protected DestInit_Item ()
        {
            AddOrChange(new DestInitInfo(ChangeDestination.Item_Material, 
                new List<ChangeType>() { ChangeType.Item_Material_Set }, 
                CTC_Material, ATC_Material));
        }


        
        partial void pCTC_Material (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Material (BaseEffectHandler eh, TotalChange tc) { pCTC_Material(eh, tc); }
        partial void pATC_Material (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Material (BaseEffectHandler eh, TotalChange tc) { pATC_Material(eh, tc); }

    }

}
