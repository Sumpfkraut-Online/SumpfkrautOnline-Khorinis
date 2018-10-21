using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Changes
{

    /// <summary>
    /// Data container to relate ChangeTypes with the types of their parameter list
    /// as well as ChangeDestination they influence. The latter is a conventient boost 
    /// to access time for performance.
    /// </summary>
    public class ChangeInitInfo
    {

        /// <summary>
        /// Indicates the type of a change.
        /// </summary>
        public ChangeType ChangeType;
        /// <summary>
        /// List of datatypes to allow casting and parsing the list of parameters properly later on.
        /// </summary>
        public List<Type> ParameterTypes;
        /// <summary>
        /// Indicators which parts of the program (like vob types) are influenced by this
        /// type of change.
        /// </summary>
        public List<ChangeDestination> InfluencedDestinations;

        /// <summary>
        /// Creates a container to relate a ChangeType with the types of their parameter list
        /// as well as ChangeDestination they influence. The latter is left to later initialization
        /// processes and simply serves as convenient shortcut when doing real-time processing later.
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="influencedDestinations"></param>
        public ChangeInitInfo (ChangeType changeType, List<Type> parameterTypes, 
            List<ChangeDestination> influencedDestinations = null)
        {
            ChangeType = changeType;
            ParameterTypes = parameterTypes ?? new List<Type>();
            InfluencedDestinations = influencedDestinations ?? new List<ChangeDestination>();
        }

    }

}
