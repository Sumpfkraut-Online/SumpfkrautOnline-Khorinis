using GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers;
using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public abstract class BaseDestination : ExtendedObject
    {

        new public static readonly string _staticName = "BaseDestination (static)";

        public static readonly ChangeDestination changeDestination = ChangeDestination.Undefined;

        // this is mostly used to clarify which types of changes are relevant for the application
        public static readonly ChangeType[] supportedChangeTypes = new ChangeType[] { };



        public static void CalculateTotalChange (BaseEffectHandler effectHandler)
        {
            throw new NotImplementedException();
        }

        public static void ApplyTotalChange (BaseEffectHandler effectHandler)
        {
            throw new NotImplementedException();
        }

    }

}
