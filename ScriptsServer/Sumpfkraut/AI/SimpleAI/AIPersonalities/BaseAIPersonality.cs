using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIPersonalities
{

    public abstract class BaseAIPersonality : ExtendedObject
    {

        new public static readonly string _staticName = "BaseAIPersonality (static) ";



        public BaseAIPersonality ()
        {
            SetObjName("BaseAIPersonality (default)");
        }

    }

}
