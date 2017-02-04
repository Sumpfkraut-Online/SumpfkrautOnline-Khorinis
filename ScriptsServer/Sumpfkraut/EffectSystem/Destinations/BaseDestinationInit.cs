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

    public abstract class BaseDestinationInit : ExtendedObject
    {

        new public static readonly string _staticName = "BaseDestinationInit (static)";

        protected ChangeDestination changeDestination = ChangeDestination.Undefined;
        public ChangeDestination ChangeDestination { get { return changeDestination; } }

        // this is mostly used to clarify which types of changes are relevant for the application
        protected List<ChangeType> supportedChangeTypes = new List<ChangeType> { };
        public List<ChangeType> SupportedChangeTypes { get { return supportedChangeTypes; } }

        protected List<CalculateTotalChange> calculateTotalChanges;
        public List<CalculateTotalChange> GetCalculateTotalChanges () { return calculateTotalChanges; }

        protected List<ApplyTotalChange> applyTotalChanges;
        public List<ApplyTotalChange> GetApplyTotalChanges () { return applyTotalChanges; }


        public BaseDestinationInit ()
        {
            changeDestination = ChangeDestination.Undefined;

            if (supportedChangeTypes == null)
            {
                supportedChangeTypes = new List<ChangeType>();
                MakeLogWarning("Missing supportedChangeTypes in subsclass-constructor of BaseDestinationInit!");
            }

            if (supportedChangeTypes == null)
            {
                supportedChangeTypes = new List<ChangeType>();
                MakeLogWarning("Missing supportedChangeTypes in subsclass-constructor of BaseDestinationInit!");
            }

            if (supportedChangeTypes == null)
            {
                supportedChangeTypes = new List<ChangeType>();
                MakeLogWarning("Missing supportedChangeTypes in subsclass-constructor of BaseDestinationInit!");
            }
        }



        public abstract void CalculateTotalChange (BaseEffectHandler effectHandler);
        public abstract void ApplyTotalChange (BaseEffectHandler effectHandler);

    }

}
