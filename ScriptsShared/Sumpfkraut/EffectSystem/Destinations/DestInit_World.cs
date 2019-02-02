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
    /// Initialize basic behavior of worlds when managed by the EffectSystem.
    /// </summary>
    public partial class DestInit_World : BaseDestInit
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static DestInit_World representative;



        // make sure, the destination makes itself known to its related changes
        static DestInit_World ()
        {
            representative = new DestInit_World();
        }

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for worlds.
        /// </summary>
        protected DestInit_World ()
        {
            AddOrChange(new DestInitInfo(ChangeDestination.World_Clock_IsRunning,
                new List<ChangeType>() { ChangeType.World_Clock_IsRunning_Set },
                CTC_Clock_IsRunning, ATC_Clock_IsRunning));

            AddOrChange(new DestInitInfo(ChangeDestination.World_Clock_Rate,
                new List<ChangeType>() { ChangeType.World_Clock_Rate_Set },
                CTC_Clock_Rate, ATC_Clock_Rate));

            AddOrChange(new DestInitInfo(ChangeDestination.World_Clock_Time,
                new List<ChangeType>() { ChangeType.World_Clock_Time_Set },
                CTC_Clock_Time, ATC_Clock_Time));
        }



        partial void pCTC_Clock_IsRunning (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Clock_IsRunning (BaseEffectHandler eh, TotalChange tc) { pCTC_Clock_IsRunning(eh, tc); }
        partial void pATC_Clock_IsRunning (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Clock_IsRunning (BaseEffectHandler eh, TotalChange tc) { pATC_Clock_IsRunning(eh, tc); }

        partial void pCTC_Clock_Rate (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Clock_Rate (BaseEffectHandler eh, TotalChange tc) { pCTC_Clock_Rate(eh, tc); }
        partial void pATC_Clock_Rate (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Clock_Rate (BaseEffectHandler eh, TotalChange tc) { pATC_Clock_Rate(eh, tc); }

        partial void pCTC_Clock_Time (BaseEffectHandler eh, TotalChange tc);
        public void CTC_Clock_Time (BaseEffectHandler eh, TotalChange tc) { pCTC_Clock_Time(eh, tc); }
        partial void pATC_Clock_Time (BaseEffectHandler eh, TotalChange tc);
        public void ATC_Clock_Time (BaseEffectHandler eh, TotalChange tc) { pATC_Clock_Time(eh, tc); }

    }

}
