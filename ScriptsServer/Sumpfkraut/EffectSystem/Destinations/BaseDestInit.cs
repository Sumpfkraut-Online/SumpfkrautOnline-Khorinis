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
        public static BaseDestInit representative;

        protected ChangeDestination changeDestination = ChangeDestination.Undefined;
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
            // always create own representativ
            representative = new BaseDestInit();
        }



        protected BaseDestInit ()
        {
            changeDestination = ChangeDestination.Undefined;

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



        protected void AddSupportedChangeType (ChangeType changeType)
        {
            if (supportedChangeTypeLists.Contains(changeType)) { return; }
            supportedChangeTypeLists.Add(changeType);
        }

        protected void AddCalculateTotalChange (CalculateTotalChange calcTotalChange)
        {
            if (calculateTotalChanges.Contains(calcTotalChange)) { return; }
            calculateTotalChanges.Add(calcTotalChange);
        }

        protected void AddApplyTotalChange (ApplyTotalChange applyTotalChange)
        {
            if (applyTotalChanges.Contains(applyTotalChange)) { return; }
            applyTotalChanges.Add(applyTotalChange);
        }

    }

}
