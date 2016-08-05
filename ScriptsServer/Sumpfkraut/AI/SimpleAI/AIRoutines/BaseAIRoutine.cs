using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines
{

    public abstract class BaseAIRoutine : ExtendedObject
    {

        new public static readonly string _staticName = "BaseAIRoutine (static)";



        public BaseAIRoutine ()
        {
            SetObjName("BaseAIRoutine (default)");
        }

    }

}
