using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public class TargetedObservation : BaseAIObservation
    {

        protected AITarget aiTarget;
        public AITarget AITarget { get { return aiTarget; } }



        public TargetedObservation(AITarget aiTarget)
        {
            this.aiTarget = aiTarget;
        }

    }

}
