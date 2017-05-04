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

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CodeName, 
                new List<ChangeType>() { ChangeType.Vob_CodeName_Set }, 
                CTC_CodeName, ATC_CodeName));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_Name, 
                new List<ChangeType>() { ChangeType.Vob_Name_Set }, 
                CTC_Name, ATC_Name));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_VobType, 
                new List<ChangeType>() { ChangeType.Vob_VobType_Set }, 
                CTC_VobType, ATC_VobType));
        }


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

        //partial void pCTC_VobDefType (BaseEffectHandler eh, TotalChange tc);
        //public void CTC_VobDefType (BaseEffectHandler eh, TotalChange tc) { pCTC_VobDefType(eh, tc); }
        //partial void pATC_VobDefType (BaseEffectHandler eh, TotalChange tc);
        //public void ATC_VobDefType (BaseEffectHandler eh, TotalChange tc) { pATC_VobDefType(eh, tc); }

        //partial void pCTC_VobInstType (BaseEffectHandler eh, TotalChange tc);
        //public void CTC_VobInstType (BaseEffectHandler eh, TotalChange tc) { pCTC_VobInstType(eh, tc); }
        //partial void pATC_VobInstType (BaseEffectHandler eh, TotalChange tc);
        //public void ATC_VobInstType (BaseEffectHandler eh, TotalChange tc) { pATC_VobInstType(eh, tc); }

    }

}
