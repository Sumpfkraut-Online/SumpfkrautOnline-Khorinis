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

    public class SDestInit_Vob : BaseDestInit, DestInit_Vob
    {

        /// <summary>
        /// Singleton which serves as cache for quasi-static data.
        /// </summary>
        new public static readonly SDestInit_Vob representative;

        public readonly bool Default_CDDyn = false;
        public readonly bool Default_CDStatic = false;

        /// <summary>
        /// Ensures coupling of ChangeDestinations to >= 1 ChangeTypes
        /// which are relevant for all vobs.
        /// </summary>
        static SDestInit_Vob ()
        {
            representative = new SDestInit_Vob();
        }

        /// <summary>
        /// Called on static constructor to initialize singleton object.
        /// </summary>
        protected SDestInit_Vob ()
        {
            SetObjName(typeof(SDestInit_Vob).Name);

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CDDyn,
                new List<ChangeType>() { ChangeType.Vob_CDDyn_Set },
                CalculateCDDyn, ApplyCDDyn));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CDStatic,
                new List<ChangeType>() { ChangeType.Vob_CDStatic_Set },
                CalculateCDStatic, ApplyCDStatic));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_CodeName,
                new List<ChangeType>() { ChangeType.Vob_CodeName_Set },
                CalculateCodeName, ApplyCodeName));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_VobType,
                new List<ChangeType>() { ChangeType.Vob_VobType_Set },
                CalculateVobType, ApplyVobType));
        }

        public void CalculateCDDyn (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_CDDyn_Set, out info)) { return; }

                // use default or value from last Change
                bool val = Default_CDDyn;
                if (tc.Components.Count > 0)
                {
                    val = (bool) tc.Components.Last().GetParameters()[0];
                }

                var finalChange = Change.Create(info, new List<object>() { val });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCalculateCDDyn: " + ex); }
        }

        public void ApplyCDDyn (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.Host;
                if (linkedObj is VobDef)
                {
                    var vobDef = linkedObj as VobDef;
                    vobDef.CDDyn = (bool) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is VobInst)
                {
                    // TO DO: add similar code as for VobDef when VobSystem supports it
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pApplyCDDyn: " + ex); }
        }

        public void CalculateCDStatic (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_CDStatic_Set, out info)) { return; }

                // use default or value from last Change
                bool val = Default_CDStatic;
                if (tc.Components.Count > 0)
                {
                    val = (bool) tc.Components.Last().GetParameters()[0];
                }

                var finalChange = Change.Create(info, new List<object>() { val });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCalculateCDStatic: " + ex); }
        }

        public void ApplyCDStatic (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.Host;
                if (linkedObj is VobDef)
                {
                    var vobDef = linkedObj as VobDef;
                    vobDef.CDStatic = (bool) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is VobInst)
                {
                    // TO DO: add similar code as for VobDef when VobSystem supports it
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via pApplyCDStatic: " + ex); }
        }

        public void CalculateCodeName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }

                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_CodeName_Set, out info)) { return; }

                // last entry counts
                var finalChange = Change.Create(info,
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CalculateCodeName: " + ex); }
        }

        public void ApplyCodeName (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                var linkedObj = eh.Host;
                if (linkedObj is VobDef)
                {
                    var vobDef = linkedObj as VobDef;
                    vobDef.CodeName = (string) tc.GetTotal().GetParameters()[0];
                }
                else if (linkedObj is VobInst)
                {
                    var vobInst = linkedObj as VobInst;
                    // ??? TO DO if codeName should be changable for a VobInst
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ApplyCodeName: " + ex); }
        }

        public void CalculateVobType (BaseEffectHandler eh, TotalChange tc)
        {
            try
            {
                // stop here when there are no Changes to process
                if (tc.Components.Count < 1) { return; }

                ChangeInitInfo info;
                if (!ValidateChangeInit(ChangeType.Vob_VobType_Set, out info)) { return; }

                // TO DO: only change VobType if the linked object is actually able to !!!
                // last entry counts
                var finalChange = Change.Create(info,
                    new List<object>() { tc.Components[tc.Components.Count - 1] });
                tc.SetTotal(finalChange);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via pCalculateVobType: " + ex); }
        }

        public void ApplyVobType (BaseEffectHandler eh, TotalChange tc)
        {
            // TO DO: switch VobType of linekd object if possible 
            //        (or simply refrain from changes after creation completely)
        }

    }

}
