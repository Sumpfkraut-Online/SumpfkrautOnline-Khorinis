using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers.BaseEffectHandler;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class BaseDestInit : ExtendedObject
    {

        new public static readonly string _staticName = "BaseDestinationInit (static)";
        protected static Dictionary<ChangeDestination, DestinationInfo> changeDestinationToInfo;
        public static BaseDestInit representative;

        protected ChangeDestination changeDestination;
        public ChangeDestination ChangeDestination { get { return changeDestination; } }

        // this is mostly used to clarify which types of changes are relevant for the application
        protected List<List<ChangeType>> supportedChangeTypeLists;
        public List<List<ChangeType>>  SupportedChangeTypeLists { get { return supportedChangeTypeLists; } }

        protected List<CalculateTotalChange> calculateTotalChanges;
        public List<CalculateTotalChange> GetCalculateTotalChanges () { return calculateTotalChanges; }

        protected List<ApplyTotalChange> applyTotalChanges;
        public List<ApplyTotalChange> GetApplyTotalChanges () { return applyTotalChanges; }


        static BaseDestInit ()
        {
            // init changeDestinationToInfo which is used by all children
            changeDestinationToInfo = new Dictionary<ChangeDestination, DestinationInfo>();
            // always create own representative
            representative = new BaseDestInit();
            representative.SetObjName("BaseDestInit");
        }

        protected BaseDestInit ()
        {
            if (supportedChangeTypeLists == null)
            {
                supportedChangeTypeLists = new List<List<ChangeType>>();
                MakeLogWarning("Missing supportedChangeTypes in subsclass-constructor of BaseDestinationInit!");
            }

            if (calculateTotalChanges == null)
            {
                calculateTotalChanges = new List<CalculateTotalChange>();
                MakeLogWarning("Missing calculateTotalChanges in subsclass-constructor of BaseDestinationInit!");
            }

            if (applyTotalChanges == null)
            {
                applyTotalChanges = new List<ApplyTotalChange>();
                MakeLogWarning("Missing applyTotalChanges in subsclass-constructor of BaseDestinationInit!");
            }
        }



        protected void AddOrChange (ChangeDestination changeDestination, List<ChangeType> supportedChangeTypes,
            CalculateTotalChange calculateTotalChange, ApplyTotalChange applyTotalChange)
        {
            DestinationInfo info;
            if (changeDestinationToInfo.TryGetValue(changeDestination, out info))
            {
                MakeLogWarning("Overwriting changeDestination: " + changeDestination);
                info.supportedChangeTypes = supportedChangeTypes;
                info.calculateTotalChange = calculateTotalChange;
                info.applyTotalChange = applyTotalChange;
            }
            else
            {
                info = new DestinationInfo(changeDestination, supportedChangeTypes,
                    calculateTotalChange, applyTotalChange);
                changeDestinationToInfo.Add(changeDestination, info);
            }
        }

    }

}
