using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public abstract class BaseAIObservation : ExtendedObject
    {

        new public static readonly string _staticName = "BaseAIObservation (static)";



        public BaseAIObservation ()
        {
            SetObjName("BaseAIObservation (default)");
        }

    }

}
