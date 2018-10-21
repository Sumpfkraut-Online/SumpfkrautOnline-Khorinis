using System;
using System.Collections.Generic;
using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// ChangeDestination registration and functionality for npc vobs.
    /// </summary>
    public class SDestInit_NPC : BaseDestInit, DestInit_NPC
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static readonly SDestInit_NPC representative;

        // make sure, the destination makes itself known to its related changes
        static SDestInit_NPC ()
        {
            representative = new SDestInit_NPC();
        }

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for npc vobs.
        /// </summary>
        protected SDestInit_NPC ()
        {
            AddOrChange(new DestInitInfo(ChangeDestination.NPC_TestPoison,
                new List<ChangeType>() { ChangeType.NPC_TestPoison_Add },
                CalculateTestPoison, ApplyTestPoison));
        }

        /// <summary>
        /// Called on static constructor to initialize singleton object.
        /// </summary>
        public void ApplyTestPoison (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }

        public void CalculateTestPoison (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }
    }
}
