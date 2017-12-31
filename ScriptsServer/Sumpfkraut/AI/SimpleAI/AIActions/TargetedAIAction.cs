using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{

    public class TargetedAIAction : BaseAIAction
    {

        protected AITarget aiTarget;
        public AITarget AITarget { get { return aiTarget; } }



        protected TargetedAIAction(AITarget aiTarget)
        {
            this.aiTarget = aiTarget;
        }

    }

}
