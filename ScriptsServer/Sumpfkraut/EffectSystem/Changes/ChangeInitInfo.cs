using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    public class ChangeInitInfo
    {

        public ChangeType ChangeType;
        public List<Type> ParameterTypes;
        public List<ChangeDestination> InfluencedDestinations;

        public ChangeInitInfo (ChangeType changeType, List<Type> parameterTypes, 
            List<ChangeDestination> influencedDestinations)
        {
            ChangeType = changeType;
            ParameterTypes = parameterTypes ?? new List<Type>();
            InfluencedDestinations = influencedDestinations ?? new List<ChangeDestination>();
        }

    }

}
