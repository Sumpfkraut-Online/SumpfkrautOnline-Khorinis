using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.AI.SimpleAI.AIObservations
{

    public abstract class BaseAIObservation : ExtendedObject
    {

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



        public BaseAIObservation (AITarget aiTarget)
        {
            //SetObjName("BaseAIObservation");
            this.attributeLock = new object();
            this.aiTarget = aiTarget;
        }

    }

}
