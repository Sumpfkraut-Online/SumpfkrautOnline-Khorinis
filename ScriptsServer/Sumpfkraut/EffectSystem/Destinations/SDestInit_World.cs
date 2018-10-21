using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using System;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// ChangeDestination registration and functionality for worlds.
    /// </summary>
    public class SDestInit_World : BaseDestInit, DestInit_World
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static readonly SDestInit_World representative;

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for worlds.
        /// </summary>
        static SDestInit_World ()
        {
            representative = new SDestInit_World();
        }

        /// <summary>
        /// Called on static constructor to initialize singleton object.
        /// </summary>
        protected SDestInit_World ()
        {
            SetObjName(typeof(SDestInit_World).Name);

            //AddOrChange(new DestInitInfo(ChangeDestination.World_Clock_IsRunning,
            //    new List<ChangeType>() { ChangeType.World_Clock_IsRunning_Set },
            //    CalculateClock_IsRunning, ApplyClock_IsRunning));

            //AddOrChange(new DestInitInfo(ChangeDestination.World_Clock_Rate,
            //    new List<ChangeType>() { ChangeType.World_Clock_Rate_Set },
            //    CalculateClock_Rate, ApplyClock_Rate));

            //AddOrChange(new DestInitInfo(ChangeDestination.World_Clock_Time,
            //    new List<ChangeType>() { ChangeType.World_Clock_Time_Set },
            //    CalculateClock_Time, ApplyClock_Time));
        }

        public void ApplyClock_IsRunning (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }

        public void ApplyClock_Rate (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }

        public void ApplyClock_Time (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }

        public void CalculateClock_IsRunning (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }

        public void CalculateClock_Rate (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }

        public void CalculateClock_Time (BaseEffectHandler eh, TotalChange tc)
        {
            throw new NotImplementedException();
        }








        //partial void pCalculateClock_IsRunning (BaseEffectHandler eh, TotalChange tc)
        //{
        //    try
        //    {
        //        // stop here when there are no Changes to process
        //        if (tc.Components.Count < 1) { return; }

        //        ChangeInitInfo info_IsRunning_Set;
        //        if (!ValidateChangeInit(ChangeType.World_Clock_IsRunning_Set, out info_IsRunning_Set)) { return; }

        //        // last entry counts
        //        var finalChange = Change.Create(info, 
        //            new List<object>() { tc.Components[tc.Components.Count - 1] });
        //        tc.SetTotal(finalChange);
        //    }
        //    catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCalculateClock_IsRunning: " + ex); }
        //}

        //partial void pApplyClock_IsRunning (BaseEffectHandler eh, TotalChange tc)
        //{
        //    try
        //    {
        //        var linkedObj = eh.GetLinkedObject();
        //        if      (linkedObj is NamedVobDef)
        //        {
        //            var vobDef = linkedObj as NamedVobDef;
        //            vobDef.Name = (string) tc.GetTotal().GetParameters()[0];
        //        }
        //        else if (linkedObj is NamedVobInst)
        //        {
        //            var vobInst = linkedObj as VobInst;
        //            // TO DO when there is the possiblity to change the name in VobSystem
        //        }
        //    }
        //    catch (Exception ex) { MakeLogError("Error while applying TotalChange via pApplyClock_IsRunning: " + ex); }
        //}

    }

}
