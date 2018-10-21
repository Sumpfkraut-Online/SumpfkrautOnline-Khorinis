using GUC.Scripts.Sumpfkraut.EffectSystem.Changes;
using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using System;
using System.Collections.Generic;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    /// <summary>
    /// ChangeDestination registration and functionality for named vobs.
    /// </summary>
    public class SDestInit_NamedVob : SDestInit_Vob, DestInit_NamedVob
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static readonly SDestInit_NamedVob representative;

        /// <summary>
        /// Default display name.
        /// </summary>
        public readonly string Default_Name = "";

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for named vobs.
        /// </summary>
        static SDestInit_NamedVob ()
        {
            representative = new SDestInit_NamedVob();
        }

        /// <summary>
        /// Called on static constructor to initialize singleton object.
        /// </summary>
        protected SDestInit_NamedVob ()
        {
            SetObjName(typeof(SDestInit_NamedVob).Name);
            AddOrChange(new DestInitInfo(ChangeDestination.NamedVob_Name,
                new List<ChangeType>() { ChangeType.NamedVob_Name_Set },
                CalculateName, ApplyName));
        }

        public void CalculateName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }

                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.NamedVob_Name_Set, out info)) { return; }

                // last entry counts
                var finalChange = Change.Create(info, 
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCalculateName: " + ex); }
        }

        public void ApplyName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.Host;
                if      (linkedObj is NamedVobDef)
                {
                    var vobDef = linkedObj as NamedVobDef;
                    vobDef.Name = (string) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is NamedVobInst)
                {
                    var vobInst = linkedObj as VobInst;
                    // TO DO when there is the possiblity to change the name in VobSystem
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pApplyName: " + ex); }
        }

    }

}
