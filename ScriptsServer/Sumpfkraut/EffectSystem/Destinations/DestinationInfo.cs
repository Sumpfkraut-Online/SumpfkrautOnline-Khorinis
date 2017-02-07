using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers.BaseEffectHandler;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public struct DestinationInfo
    {

        public ChangeDestination changeDestination;
        public List<ChangeType> supportedChangeTypes;
        public CalculateTotalChange calculateTotalChange;
        public ApplyTotalChange applyTotalChange;


        public DestinationInfo (ChangeDestination changeDestination, List<ChangeType> supportedChangeTypes,
            CalculateTotalChange calculateTotalChange, ApplyTotalChange applyTotalChange)
        {
            this.changeDestination = changeDestination;
            this.supportedChangeTypes = supportedChangeTypes ?? new List<ChangeType>();
            this.calculateTotalChange = calculateTotalChange;
            this.applyTotalChange = applyTotalChange;
        }

    }

}
