using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIRoutines
{

    public abstract class BaseAIRoutine : ExtendedObject
    {

        protected BaseAIRoutine ()
        { }



        public abstract void FollowRoutine (AIAgent aiAgent);

    }

}
