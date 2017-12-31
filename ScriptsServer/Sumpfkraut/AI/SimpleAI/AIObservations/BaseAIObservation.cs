using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public abstract class BaseAIObservation : ExtendedObject
    {

        protected AITarget aiTarget;
        public AITarget AITarget { get { return aiTarget; } }



        public BaseAIObservation (AITarget aiTarget)
        {
            this.aiTarget = aiTarget;
        }

    }

}
