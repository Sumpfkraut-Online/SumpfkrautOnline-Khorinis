using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInit_Vob : BaseDestInit
    {

        new public static DestInit_Vob representative;

        public static readonly bool Default_CDDyn = false;
        public static readonly bool Default_CDStatic = false;



        // make sure, the destination makes itself known to its related changes
        static DestInit_Vob ()
        {
            representative = new DestInit_Vob();
        }

        protected DestInit_Vob ()
        {
            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CDDyn, 
                new List<ChangeType>() { ChangeType.Vob_CDDyn_Set }, 
                CTC_CDDyn, ATC_CDDyn));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CDStatic, 
                new List<ChangeType>() { ChangeType.Vob_CDStatic_Set }, 
                CTC_CDStatic, ATC_CDStatic));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CodeName, 
                new List<ChangeType>() { ChangeType.Vob_CodeName_Set }, 
                CTC_CodeName, ATC_CodeName));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_VobType, 
                new List<ChangeType>() { ChangeType.Vob_VobType_Set }, 
                CTC_VobType, ATC_VobType));
        }


        
        partial void pCTC_CDDyn (BaseEffectHandler eh, TotalChange tc);
        public void CTC_CDDyn (BaseEffectHandler eh, TotalChange tc) { pCTC_CDDyn(eh, tc); }
        partial void pATC_CDDyn (BaseEffectHandler eh, TotalChange tc);
        public void ATC_CDDyn (BaseEffectHandler eh, TotalChange tc) { pATC_CDDyn(eh, tc); }

        partial void pCTC_CDStatic (BaseEffectHandler eh, TotalChange tc);
        public void CTC_CDStatic (BaseEffectHandler eh, TotalChange tc) { pCTC_CDStatic(eh, tc); }
        partial void pATC_CDStatic (BaseEffectHandler eh, TotalChange tc);
        public void ATC_CDStatic (BaseEffectHandler eh, TotalChange tc) { pATC_CDStatic(eh, tc); }

        partial void pCTC_CodeName (BaseEffectHandler eh, TotalChange tc);
        public void CTC_CodeName (BaseEffectHandler eh, TotalChange tc) { pCTC_CodeName(eh, tc); }
        partial void pATC_CodeName (BaseEffectHandler eh, TotalChange tc);
        public void ATC_CodeName (BaseEffectHandler eh, TotalChange tc) { pATC_CodeName(eh, tc); }

        partial void pCTC_Name (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Name (BaseEffectHandler eh, TotalChange tc) { pCTC_Name(eh, tc); }
        partial void pATC_Name (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Name (BaseEffectHandler eh, TotalChange tc) { pATC_Name(eh, tc); }

        partial void pCTC_VobType (BaseEffectHandler eh, TotalChange tc);
        public void CTC_VobType (BaseEffectHandler eh, TotalChange tc) { pCTC_VobType(eh, tc); }
        partial void pATC_VobType (BaseEffectHandler eh, TotalChange tc);
        public void ATC_VobType (BaseEffectHandler eh, TotalChange tc) { pATC_VobType(eh, tc); }

    }

}
