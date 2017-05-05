using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInit_NPC : BaseDestInit
    {

        new public static readonly string _staticName = "DestInit_NPC (s)";
        new public static DestInit_NPC representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_NPC ()
        {
            representative = new DestInit_NPC();
        }

        protected DestInit_NPC ()
        {
            SetObjName("DestInit_NPC");

            //AddOrChange(new DestInitInfo(ChangeDestination.Vob_CDDyn, 
            //    new List<ChangeType>() { ChangeType.Vob_CDDyn_Set }, 
            //    CTC_CDDyn, ATC_CDDyn));
        }


        
        //partial void pCTC_CDDyn (BaseEffectHandler eh, TotalChange tc);
        //public void CTC_CDDyn (BaseEffectHandler eh, TotalChange tc) { pCTC_CDDyn(eh, tc); }
        //partial void pATC_CDDyn (BaseEffectHandler eh, TotalChange tc);
        //public void ATC_CDDyn (BaseEffectHandler eh, TotalChange tc) { pATC_CDDyn(eh, tc); }

    }

}
