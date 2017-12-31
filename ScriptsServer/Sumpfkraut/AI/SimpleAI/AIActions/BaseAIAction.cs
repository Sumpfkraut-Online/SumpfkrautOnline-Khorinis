using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{
    public abstract class BaseAIAction : ExtendedObject
    {

        protected AITarget aiTarget;
        public AITarget AITarget { get { return aiTarget; } }



        protected BaseAIAction (AITarget aiTarget)
        {
            this.aiTarget = aiTarget;
        }

    }
}
