using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers.BaseEffectHandler;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public partial class DestInitInfo
    {

        public ChangeDestination ChangeDestination;
        public List<ChangeType> SupportedChangeTypes;
        public CalculateTotalChange CalculateTotalChange;
        public ApplyTotalChange ApplyTotalChange;


        public DestInitInfo (ChangeDestination changeDestination, List<ChangeType> supportedChangeTypes,
            CalculateTotalChange calculateTotalChange, ApplyTotalChange applyTotalChange)
        {
            ChangeDestination = changeDestination;
            SupportedChangeTypes = supportedChangeTypes ?? new List<ChangeType>();
            CalculateTotalChange = calculateTotalChange;
            ApplyTotalChange = applyTotalChange;
        }

    }

}
