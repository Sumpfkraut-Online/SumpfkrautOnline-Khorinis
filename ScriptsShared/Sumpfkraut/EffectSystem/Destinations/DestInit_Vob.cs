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

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_VobDefType, 
                new List<ChangeType>() { ChangeType.Vob_VobDefType_Set }, 
                CTC_VobDefType, ATC_VobDefType));

            AddOrChange(new DestInitInfo(ChangeDestination.Vob_VobInstType, 
                new List<ChangeType>() { ChangeType.Vob_VobInstType_Set }, 
                CTC_VobInstType, ATC_VobInstType));
        }

        public void CTC_CodeName (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange = null;
                Change tc;
                ChangeInitInfo info;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Vob_CodeName, out totalChange)) { return; }
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_CodeName_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange with non-initialized ChangeType "
                        + ChangeType.Vob_CodeName_Set);
                    return;
                }

                // stop here when there are no Changes to process
                if (totalChange.Components.Count < 1) { return; }
                // last codeName counts
                tc = Change.Create(info, 
                    new List<object>() { totalChange.Components[totalChange.Components.Count - 1] });
                totalChange.SetTotal(tc);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_CodeName: " + ex); }
        }

        public void ATC_CodeName (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange = null;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Effect_Name, out totalChange)) { return; }

                var linkedObj = effectHandler.GetLinkedObject();
                if      (linkedObj is VobDef)
                {
                    linkedObj = linkedObj as VobDef;
                    // set the codeName
                }
                else if (linkedObj is VobInst)
                {
                    linkedObj = linkedObj as VobInst;
                    // set the codeName
                }
            }
            catch (Exception ex) { MakeLogError("Error while applying TotalChange via ATC_CodeName: " + ex); }
        }

        public void CTC_Name (BaseEffectHandler effectHandler)
        {
            // TO DO
        }

        public void ATC_Name (BaseEffectHandler effectHandler)
        {
            // TO DO
        }

        public void CTC_VobDefType (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange = null;
                Change tc;
                ChangeInitInfo info;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Vob_VobDefType, out totalChange)) { return; }
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_VobDefType_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange via CTC_VobDefType with non-initialized ChangeType "
                        + ChangeType.Vob_VobDefType_Set);
                    return;
                }

                // last component counts as long as the linkedObject still isn't set
                // (changing it afterwards is not possible)
                object linkedObject = effectHandler.GetLinkedObject();
                if (linkedObject != null) { return; }

                // stop here when there are no Changes to process
                if (totalChange.Components.Count < 1) { return; }
                // last codeName counts
                tc = Change.Create(info,
                    new List<object>() { totalChange.Components[totalChange.Components.Count - 1] });
                totalChange.SetTotal(tc);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_VobDefType: " + ex); }
        }

        public void ATC_VobDefType (BaseEffectHandler effectHandler)
        {
            // no application necessary because VobDefType is only used 
            // when creating a new instance of VobDef, not afterwards

        }

        public void CTC_VobInstType (BaseEffectHandler effectHandler)
        {
            try
            {
                TotalChange totalChange = null;
                Change tc;
                ChangeInitInfo info;
                if (!effectHandler.TryGetTotalChange(ChangeDestination.Vob_VobInstType, out totalChange)) { return; }
                if (!BaseChangeInit.TryGetChangeInitInfo(ChangeType.Vob_VobInstType_Set, out info))
                {
                    MakeLogError("Tried to calculate TotalChange via CTC_VobInstType with non-initialized ChangeType "
                        + ChangeType.Vob_VobInstType_Set);
                    return;
                }

                // last component counts as long as the linkedObject still isn't set
                // (changing it afterwards is not possible)
                object linkedObject = effectHandler.GetLinkedObject();
                if (linkedObject != null) { return; }

                // stop here when there are no Changes to process
                if (totalChange.Components.Count < 1) { return; }
                // last codeName counts
                tc = Change.Create(info, 
                    new List<object>() { totalChange.Components[totalChange.Components.Count - 1] });
                totalChange.SetTotal(tc);
            }
            catch (Exception ex) { MakeLogError("Error while caclulating TotalChange via CTC_VobInstType: " + ex); }
        }

        public void ATC_VobInstType (BaseEffectHandler effectHandler)
        {
            // no application necessary because VobInstType is only used 
            // when creating a new instance of VobInst, not afterwards
        }

    }

}
