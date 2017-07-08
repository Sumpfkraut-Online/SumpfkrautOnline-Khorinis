using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInit_Effect : BaseDestInit
    {

        new public static DestInit_Effect representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_Effect ()
        {
            representative = new DestInit_Effect();
        }

        protected DestInit_Effect ()
        {
            SetObjName("DestInit_Effect");

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_GlobalID, 
                new List<ChangeType>() { ChangeType.Effect_GlobalID_Set }, 
                CTC_GlobalID, ATC_GlobalID));

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_Name, 
                new List<ChangeType>() { ChangeType.Effect_Name_Set }, 
                CTC_Name, ATC_Name));

            AddOrChange(new DestInitInfo(ChangeDestination.Effect_Parent, 
                new List<ChangeType>() { ChangeType.Effect_Parent_Add }, 
                CTC_Parent, ATC_Parent));
        }



        partial void pCTC_GlobalID (BaseEffectHandler eh, TotalChange tc);
        public void CTC_GlobalID (BaseEffectHandler eh, TotalChange tc) { pCTC_GlobalID(eh, tc); }
        partial void pATC_GlobalID (BaseEffectHandler eh, TotalChange tc);
        public void ATC_GlobalID (BaseEffectHandler eh, TotalChange tc) { pATC_GlobalID(eh, tc); }

        partial void pCTC_Name (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Name (BaseEffectHandler eh, TotalChange tc) { pCTC_Name(eh, tc); }
        partial void pATC_Name (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Name (BaseEffectHandler eh, TotalChange tc) { pATC_Name(eh, tc); }

        partial void pCTC_Parent (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Parent (BaseEffectHandler eh, TotalChange tc) { pCTC_Parent(eh, tc); }
        partial void pATC_Parent (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Parent (BaseEffectHandler eh, TotalChange tc) { pATC_Parent(eh, tc); }

        partial void pCTC_PermanentFlag (BaseEffectHandler eh, TotalChange tc);
        public void CTC_PermanentFlag (BaseEffectHandler eh, TotalChange tc) { pCTC_PermanentFlag(eh, tc); }
        partial void pATC_PermanentFlag (BaseEffectHandler eh, TotalChange tc);
        public void ATC_PermanentFlag (BaseEffectHandler eh, TotalChange tc) { pATC_PermanentFlag(eh, tc); }

    }

}
