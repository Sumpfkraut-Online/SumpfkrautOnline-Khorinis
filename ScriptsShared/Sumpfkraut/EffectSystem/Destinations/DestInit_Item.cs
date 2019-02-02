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

    /// <summary>
    /// Initialize basic behavior of item vobs when managed by the EffectSystem.
    /// </summary>
    public partial class DestInit_Item : BaseDestInit
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static DestInit_Item representative;

        public static readonly ItemMaterials Default_Material = ItemMaterials.Wood;



        /// <summary>
        /// Make sure, the destination makes itself known to its related changes.
        /// </summary>
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
