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

        new public static DestInit_NPC representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_NPC ()
        {
            representative = new DestInit_NPC();
        }

        protected DestInit_NPC ()
        {
            AddOrChange(new DestInitInfo(ChangeDestination.NPC_TestPoison,
                new List<ChangeType>() { ChangeType.NPC_TestPoison_Add },
                CTC_TestPoison, ATC_TestPoison));
        }



        partial void pCTC_TestPoison (BaseEffectHandler eh, TotalChange tc);
        public void CTC_TestPoison (BaseEffectHandler eh, TotalChange tc) { pCTC_TestPoison(eh, tc); }
        partial void pATC_TestPoison (BaseEffectHandler eh, TotalChange tc);
        public void ATC_TestPoison (BaseEffectHandler eh, TotalChange tc) { pATC_TestPoison(eh, tc); }

    }

}
