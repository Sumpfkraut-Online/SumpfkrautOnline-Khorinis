using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIActions
{
    public abstract class BaseAIAction : ExtendedObject
    {

        protected Enumeration.AiActionType actionType;
        public Enumeration.AiActionType ActionType { get { return actionType; } }

        protected object attributeLock;

        protected AITarget aiTarget;
        public AITarget AITarget { get { return aiTarget; } }
        public void SetAITarget (AITarget value)
        {
            lock (attributeLock)
            {
                this.aiTarget = value;
            }
        }



        public BaseAIAction (AITarget aiTarget)
        {
            //SetObjName("BaseAIAction");
            this.attributeLock = new object();
            //this.actionType = actionType;
            this.aiTarget = aiTarget;
        }

    }
}
