using GUC.Scripts.Sumpfkraut.EffectSystem.Enumeration;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.Destinations
{

    public class BaseDestination : ExtendedObject
    {

        new public static readonly string _staticName = "BaseDestination (static)";

        // this is mostly used to clarify which types of changes are relevant for the application
        public static readonly ChangeType[] supportedChangeTypes = new ChangeType[] { };

    }

}
